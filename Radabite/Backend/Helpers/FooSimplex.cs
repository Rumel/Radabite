using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SolverFoundation.Solvers;

namespace Radabite.Backend.Helpers
{
	public class FooSimplex
	{
		private  SimplexSolver _solver;

		public FooSimplex()
		{
			_solver = new SimplexSolver();
		}

		public void RunLP()
		{
			//lol :)
		}
	}
}