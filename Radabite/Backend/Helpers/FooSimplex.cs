using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SolverFoundation.Solvers;
using RadabiteServiceManager;
using Radabite.Backend.Interfaces;
using Ninject;

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
			var eventRequest = ServiceManager.Kernel.Get<IEventManager>().GetAll();

			/*
			 * Estimates needed:
			 *		average number of views for an event in a day
			 *		total storage that will be needed by the end of the day
			 */




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
	}
}