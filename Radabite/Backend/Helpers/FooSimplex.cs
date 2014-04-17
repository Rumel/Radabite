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

		public double[] GetAllocated()
		{
			//Estimates for events
			var events = ServiceManager.Kernel.Get<IEventManager>().GetAll();
			double averageViews = EstimateAverageViews();
			double totalStorage = EstimateTotalStorage(events);

			//if we estimate that we can fit all of our media in memory, don't waste time doing simplex
			if(totalStorage < 15 / (0.25 + 0.3 * averageViews))
			{
				return new double[] {15 / (0.25 + 0.3 * averageViews), 0, 0};
			}
			else
			{
				return SimplexAllocate(averageViews, totalStorage);
			}
		}

		public double[] SimplexAllocate(double averageViews, double totalStorage) 
		{
			double[] allocations = new double[3]; 

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
			model.AddConstraint("storage", totalStorage == sizeMem + sizeDisk + sizeTape );
			model.AddGoal("storageValue", GoalKind.Maximize, memConstant * sizeMem + diskConstant * sizeDisk + tapeConstant * sizeTape);

			Solution sol = solver.Solve(new SimplexDirective());

			allocations[0] = sizeMem.ToDouble();
			allocations[1] = sizeDisk.ToDouble();
			allocations[2] = sizeTape.ToDouble();

			return allocations;
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

			foreach(Event e in events)
			{
				if(e.EndTime < DateTime.Now)
				{
					numPeople += e.Guests.Count;

					foreach (Post p in e.Posts)
					{
						if (p is MediaPost)
						{
							var blobId = ((MediaPost)p).BlobId;
							var fooResult = ServiceManager.Kernel.Get<IFooCDNManager>().GetInfo(blobId);
							
							if(fooResult.StatusCode == HttpStatusCode.OK)
							{
								var dictionary = fooResult.Value as Dictionary<string, dynamic>;
								storageUsed = Double.Parse(dictionary["BlobSize"]) / 1e6;
							}							
						}
					}
				}
			}

			double storagePerUser;
			if(numPeople > 0)
			{
				storagePerUser = storageUsed / numPeople;
			}
			else
			{
				storagePerUser = 0;
			}

			double gigabytes = 0;
			foreach(Event e in events)
			{
				if (e.StartTime < DateTime.Now.AddDays(1))
				{
					gigabytes += e.Guests.Count * storagePerUser;
				}
			}

			return gigabytes;
		}

		private double EstimateAverageViews()
		{
			//For now, the average event page is viewed 30 times
			return 30;
		}
	}
}