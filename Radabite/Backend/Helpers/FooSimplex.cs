using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SolverFoundation.Solvers;

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
			//Mixed integer linear programming, with 0,1,2 for the storage types?
					//nah

			/*
			 * Note: changes a lot if you allow the time to load media to depend on its size :)
			 */

			/*
			 * Estimates needed:
			 * 
			 * 1. number of views for the event page ~ invitees + time to event
			 * 2. total size of media for the event ~ invitees
			 * 
			 * use those to create a cost per event page view thingy
			 */

			/*
			 *	Options for file staging algorithm:
			 * 
			 *	Have each event assigned a storage type, put all media for that event in that storage type
			 *			^^^^^I think you want to do this, because if its asynchronously getting things,
			 *					then the time waiting for the things should be equal to the delay for
			 *					worst storage type it uses, so if they were on different things, it would
			 *					be silly
			 *				Also avoids needing to keep track of counters and things for each event post/view/whatever
			 * 
			 *		Once you have the total size for mem and disk, iterate through the events smartly to assign the
			 *		"biggest" ones to mem (while it has room) (also be smart about remainder? internal fragmentation:) )
			 *		then do the same for disk
			 *
			 */

			/*
			 * Decision variables:
			 * sm := total size to allocate to mem
			 * sd := total size to allocate to disk
			 * st := total size to allocate to tape
			 */

			/*
			 * 
			 */

			//objective function: minimize waiting time
			//waiting time = yucky

			//alternative objective function
			/*
			 * maximize: a * (size in mem) + b * sd + c * st
			 * a, b, and c are constants determined by relative access times to the different types
			 */

			//contraints:
				//cost < 15
			/*
			 * cost = (size in mem) * storage in mem + (views/day for an average event) * (size in mem / # events)
			 *			+ same for disk
			 *			+ same for tape
			 */
				


			_solver.Solve(new SimplexSolverParams());
		}
	}
}