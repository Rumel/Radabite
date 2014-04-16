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
			double[] allocations = new double[3]; 

			/*
			 * Goal is to maximize the storage constant
			 *		storage constant = (memory constant) * (size in mem) 
			 *					+ (disk constant) * (size in disk) 
			 *					+ (tape constant) * (size in tape)
			 */
			double memConstant = 10,
				diskConstant = 5,
				tapeConstant = 1;


			//Estimates for events
			var events = ServiceManager.Kernel.Get<IEventManager>().GetAll();
			double averageViews = EstimateAverageViews();
			double totalStorage = EstimateTotalStorage(events);

			//if we estimate that we can fit all of our media in memory, do so immediately
			if(totalStorage < 15 / (3.025 + 0.3 * averageViews))
			{
				return new double[] {15 / (3.025 + 0.3 * averageViews), 0, 0};
			}

			int sizeMem, sizeDisk, sizeTape, storageConstant, cost, storage;

			//Decision variables
			_solver.AddVariable("MemSize", out sizeMem);
			_solver.SetBounds(sizeMem, 0, totalStorage);
			_solver.AddVariable("DiskSize", out sizeDisk);
			_solver.SetBounds(sizeDisk, 0, totalStorage);
			_solver.AddVariable("TapeSize", out sizeTape);
			_solver.SetBounds(sizeTape, 0, totalStorage);

			//Constraints
				// cost: (3.025 + 0.3*views) * sizeMem + (1.0025 + 0.1*views) * sizeDisk <= 15
			_solver.AddRow("cost", out cost);
			_solver.SetCoefficient(cost, sizeMem, 3.025 + 0.3 * averageViews);
			_solver.SetCoefficient(cost, sizeDisk, 1.0025 * 0.1 * averageViews);
			_solver.SetBounds(cost, 0, 15);

				// storage: 
			_solver.AddRow("storage", out storage);
			_solver.SetCoefficient(storage, sizeMem, 1);
			_solver.SetCoefficient(storage, sizeDisk, 1);
			_solver.SetCoefficient(storage, sizeTape, 1);
			_solver.SetBounds(storage, totalStorage, Rational.PositiveInfinity);

			//Objective row: maximize storage constant
			_solver.AddRow("StorageConstant", out storageConstant);
			_solver.SetCoefficient(storageConstant, sizeMem, memConstant);
			_solver.SetCoefficient(storageConstant, sizeDisk, diskConstant);
			_solver.SetCoefficient(storageConstant, sizeTape, tapeConstant);
			_solver.SetBounds(storageConstant, 0, Rational.PositiveInfinity);
			_solver.AddGoal(storageConstant, 1, false);

			_solver.Solve(new SimplexSolverParams());

			allocations[0] = _solver.GetValue(sizeMem).ToDouble();
			allocations[1] = _solver.GetValue(sizeDisk).ToDouble();
			allocations[2] = _solver.GetValue(sizeTape).ToDouble();

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
							var fooResult = ServiceManager.Kernel.Get<IFooCDNAccessor>().GetInfo(blobId);
							
							if(fooResult.StatusCode == HttpStatusCode.OK)
							{
								var dictionary = fooResult.Value as Dictionary<string, dynamic>;
								storageUsed = dictionary["BlobSize"] / 1e6;
							}							
						}
					}
				}
			}

			double gigabytes = 0;
			foreach(Event e in events)
			{
				if (e.StartTime < DateTime.Now.AddDays(1))
				{
					gigabytes += e.Guests.Count * (storageUsed / numPeople);
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