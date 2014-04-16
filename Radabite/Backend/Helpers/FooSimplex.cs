using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SolverFoundation.Solvers;
using RadabiteServiceManager;
using Radabite.Backend.Interfaces;
using Ninject;
using Radabite.Backend.Database;

namespace Radabite.Backend.Helpers
{
	public class FooSimplex
	{
		private SimplexSolver _solver;

		public FooSimplex()
		{
			_solver = new SimplexSolver();
		}

		public void RunLP()
		{
			//Estimates for events
			var events = ServiceManager.Kernel.Get<IEventManager>().GetAll();
			double averageViews = EstimateAverageViews();
			double totalStorage = EstimateTotalStorage(events);

			int sizeMem, sizeDisk, sizeTape;

			//Decision variables
			_solver.AddVariable("MemSize", out sizeMem);
			_solver.SetBounds(sizeMem, 0, totalStorage);
			_solver.AddVariable("DiskSize", out sizeDisk);
			_solver.SetBounds(sizeDisk, 0, totalStorage);
			_solver.AddVariable("TapeSize", out sizeTape);
			_solver.SetBounds(sizeTape, 0, totalStorage);



			/*
			 * Decision variables:
			 *		Sm := size allocated to memcache in GB
			 *		Sd
			 *		St
			 */

			/*
			 * Objective function:
			 *		Maximize: Cm * Sm + Cd * Sd + Ct * St
			 *		
			 *		Cm, Cd, Ct are constants based on relative values of storage types
			 */

			/*
			 * Constraints:
			 * 
			 *		Cost:
			 *			(3.025 + 0.3*v) * Sm + (1.0025 + 0.1*v) * Sd <= 15
			 *			
			 *			v is the average number of views per day for an event page
			 *		
			 *		Storage:
			 *			Sm + Sd + St => estimated total content (in GB)
			 *			
			 *		Note: it might be useful to set this ^ estimate to be at least
			 *			15 / (3.025 + .3v), because if we don't have enough content
			 *			to use up all $15 if we put it all in mem, then at least act like
			 *			we do so that it all goes in mem?
			 */

			_solver.Solve(new SimplexSolverParams());
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

					//for all posts, get size, add to storageUsed
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