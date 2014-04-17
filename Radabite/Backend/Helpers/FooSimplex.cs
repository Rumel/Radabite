using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SolverFoundation.Solvers;
using RadabiteServiceManager;
using Radabite.Backend.Interfaces;
using Ninject;
using Radabite.Backend.Database;
using System.Net;
using Microsoft.SolverFoundation.Common;
using Microsoft.SolverFoundation.Services;

namespace Radabite.Backend.Helpers
{
	public class FooSimplex
	{
		private SimplexSolver _solver;

		public FooSimplex()
		{
			_solver = new SimplexSolver();
		}

		public SimplexDecision GetAllocated()
		{
			//Estimates for events
			var events = ServiceManager.Kernel.Get<IEventManager>().GetAll();
			double averageViews = EstimateAverageViews();
			double totalStorage = EstimateTotalStorage(events);

			//if we estimate that we can fit all of our media in memory, don't waste time doing simplex
			if (totalStorage < 15 / (0.25 + 0.3 * averageViews))
			{
				return new SimplexDecision()
				{
					Memory = 15 / (0.25 + 0.3 * averageViews),
					Disk = 0,
					Tape = 0
				};
			}
			else
			{
				return SimplexAllocate(averageViews, totalStorage);
			}
		}

		public SimplexDecision SimplexAllocate(double averageViews, double totalStorage)
		{
			/*
			 * Goal is to maximize the storage constant
			 *		storage constant = (memory constant) * (size in mem) 
			 *					+ (disk constant) * (size in disk) 
			 *					+ (tape constant) * (size in tape)
			 */

			//Assuming value is proportional to cost
			double memConstant = 10,
				diskConstant = 5,
				tapeConstant = 0;

			SolverContext solver = SolverContext.GetContext();
			Model model = solver.CreateModel();

			Decision sizeMem = new Decision(Domain.RealNonnegative, "MemorySize");
			Decision sizeDisk = new Decision(Domain.RealNonnegative, "DiskSize");
			Decision sizeTape = new Decision(Domain.RealNonnegative, "TapeSize");

			model.AddDecisions(sizeMem, sizeDisk, sizeTape);

			model.AddConstraint("cost", 0 <= (0.25 + 0.3 * averageViews) * sizeMem + (0.025 + 0.1 * averageViews) * sizeDisk <= 15);
			model.AddConstraint("storage", totalStorage == sizeMem + sizeDisk + sizeTape);
			model.AddGoal("storageValue", GoalKind.Maximize, memConstant * sizeMem + diskConstant * sizeDisk + tapeConstant * sizeTape);

			Solution sol = solver.Solve(new SimplexDirective());

			return new SimplexDecision()
			{
				Memory = sizeMem.ToDouble(),
				Disk = sizeDisk.ToDouble(),
				Tape = sizeTape.ToDouble()
			};
		}

		private double EstimateTotalStorage(IEnumerable<Event> events)
		{
			/*
			 * Estimates amount of storage needed by the end of the day
			 * based on the amount of storage per user per event for
			 * events that have already finished
			 */

			int numPeople = 0;
			double storageUsed = 0;

			//events that have finished before now
			var previousEvents = events.Where<Event>(e => e.EndTime < DateTime.Now);

			numPeople = previousEvents.Select<Event, int>(e => e.Guests.Count).Sum();
			storageUsed = previousEvents.Select<Event, double>(e => GetEventStorageSize(e)).Sum();

			double storagePerUser = (numPeople > 0) ? (storageUsed / numPeople) : 0;

			//sum of total users from events up until the end of today
			var projectedUsers = events.Where<Event>(e => e.StartTime < DateTime.Now.AddDays(1))
												.Select<Event, int>(e => e.Guests.Count).Sum();

			return projectedUsers * storagePerUser;
		}

		private double EstimateAverageViews()
		{
			//For now, the average event page is viewed 30 times
			return 30;
		}

		private double GetEventStorageSize(Event e)
		{
			//For each MediaPost
			var storageUsed = e.Posts.Where<Post>(p => p is MediaPost)
									.Select<Post, double>(p =>
			{
				//Gets the storage used for that blob from FooCDN
				var blobId = ((MediaPost)p).BlobId;
				var fooResult = ServiceManager.Kernel.Get<IFooCDNManager>().GetInfo(blobId);

				if (fooResult.StatusCode == HttpStatusCode.OK)
				{
					var dictionary = fooResult.Value as Dictionary<string, dynamic>;
					return Double.Parse(dictionary["BlobSize"]) / 1e6;
				}
				else
				{
					return 0;
				}
			}).Sum();

			return storageUsed;
		}
	}

	public class SimplexDecision
	{
		public double Memory { get; set; }
		public double Disk { get; set; }
		public double Tape { get; set; }
	}
}