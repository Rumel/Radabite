using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radabite.Backend.Interfaces;
using RadabiteServiceManager;
using Radabite.Tests.Mocks.Accessors;
using Ninject;
using System.Collections.Generic;
using Radabite.Backend.Helpers;
using Radabite.Backend.Managers;

namespace Radabite.Tests.Helpers
{
	[TestClass]
	public class FooSimplexTest
	{
		[TestInitialize]
		public void Setup()
		{
			ServiceManager.Kernel.Rebind<IFooCDNAccessor>().To<MockFooCDNAccessor>().InSingletonScope();
			ServiceManager.Kernel.Rebind<IEventAccessor>().To<MockEventAccessor>().InSingletonScope();
			ServiceManager.Kernel.Rebind<IEventManager>().To<EventManager>();
			ServiceManager.Kernel.Rebind<IFooCDNManager>().To<FooCDNManager>();
		}

		[TestMethod]
		public void SimplexShortCircuitTest()
		{
			//Events have no content, so it will short circuit the simplex
			FooSimplex simplex = new FooSimplex();

			var result = simplex.GetAllocated();

			Assert.IsTrue(result.Memory > 0);
			Assert.AreEqual(result.Disk, 0);
			Assert.AreEqual(result.Tape, 0);
		}

		[TestMethod]
		public void SimplexFillDiskTest()
		{
			/*
			 * If you have a high amount of views or storage,
			 * it should move away from cache and toward memory
			 */
			FooSimplex simplex = new FooSimplex();
			var numViews = 100;
			var estimatedSize = 2;
			var result = simplex.SimplexAllocate(numViews, estimatedSize);

			Assert.IsTrue(result.Memory == 0);
			Assert.IsTrue(result.Disk >= 0);
			Assert.IsTrue(result.Tape >= 0);
			Assert.IsTrue(CheckCost(result, numViews));
		}

		[TestMethod]
		public void SimplexMemTest()
		{
			/*
			 * At low enough amounts, some will be allocated to memory,
			 * and none to tape
			 */
			FooSimplex simplex = new FooSimplex();
			var numViews = 30;
			var estimatedSize = 2;

			var result = simplex.SimplexAllocate(numViews, estimatedSize);

			Assert.IsTrue(result.Memory >= 0);
			Assert.IsTrue(result.Disk >= 0);
			Assert.IsTrue(result.Tape == 0);
			Assert.IsTrue(CheckCost(result, numViews));
		}

		public bool CheckCost(SimplexDecision allocations, double numViews)
		{
			//rounding in case cost is 15.00000000001 (floating point silliness)
			return Math.Round((0.25 + 0.3 * numViews) * allocations.Memory + (0.025 + 0.1 * numViews) * allocations.Disk, 2) <= 15;
		}
	}
}
