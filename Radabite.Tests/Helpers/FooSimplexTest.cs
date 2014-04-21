using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Radabite.Backend.Interfaces;
using RadabiteServiceManager;
using Radabite.Tests.Mocks.Accessors;
using Ninject;
using System.Collections.Generic;
using Radabite.Backend.Helpers;
using Radabite.Backend.Managers;
using Radabite.Backend.Database;
using Radabite.Backend.Accessors;

namespace Radabite.Tests.Helpers
{
	[TestClass]
	public class FooSimplexTest
	{
		FooSimplex _simplex = new FooSimplex();

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
			var events = ServiceManager.Kernel.Get<IEventManager>().GetAll();

			var result = _simplex.GetAllocation(events);
			var averageViews = _simplex.EstimateAverageViews(events);

			Assert.AreEqual(15 / (0.25 + 0.3 * averageViews), result.Memory);
			Assert.AreEqual(0, result.Disk);
			Assert.AreEqual(0, result.Tape);
		}

		[TestMethod]
		public void SimplexFillDiskTest()
		{
			/*
			 * If you have a high amount of views or storage,
			 * it should move away from cache and toward memory
			 */
			var numViews = 100;
			var estimatedSize = 2;
			var result = _simplex.SimplexAllocate(numViews, estimatedSize);

			Assert.AreEqual(0, result.Memory);
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
			var numViews = 30;
			var estimatedSize = 2;

			var result = _simplex.SimplexAllocate(numViews, estimatedSize);

			Assert.IsTrue(result.Memory >= 0);
			Assert.IsTrue(result.Disk >= 0);
			Assert.AreEqual(0, result.Tape);
			Assert.IsTrue(CheckCost(result, numViews));
		}

		[TestMethod]
		public void StorageEstimateTest()
		{
			var events = ServiceManager.Kernel.Get<IEventManager>().GetAll();

			var estimate = _simplex.EstimateTotalStorage(events);

			Assert.AreEqual(2, estimate);
		}

		[TestMethod]
		public void ViewEstimateTest()
		{
			var events = ServiceManager.Kernel.Get<IEventManager>().GetAll();

			var estimate = _simplex.EstimateAverageViews(events);
			Assert.AreEqual(0.9, estimate);
		}

		[TestMethod]
		public void AllToMemTest()
		{
			var events = ServiceManager.Kernel.Get<IEventManager>().GetAll();
			SimplexDecision allocation = new SimplexDecision()
			{
				Memory = 10000,
				Disk = 0,
				Tape = 0
			};
			_simplex.AssignEvents(allocation, events);

			foreach(Event e in events)
			{
				Assert.AreEqual(FooCDNAccessor.StorageType.MemCache, e.StorageLocation);
			}
		}

		[TestMethod]
		public void AllToDiskTest()
		{
			var events = ServiceManager.Kernel.Get<IEventManager>().GetAll();
			SimplexDecision allocation = new SimplexDecision()
			{
				Memory = 0,
				Disk = 10000,
				Tape = 0
			};
			_simplex.AssignEvents(allocation, events);

			foreach (Event e in events)
			{
				Assert.AreEqual(FooCDNAccessor.StorageType.Disk, e.StorageLocation);
			}
		}

		[TestMethod]
		public void AllToTapeTest()
		{
			var events = ServiceManager.Kernel.Get<IEventManager>().GetAll();
			SimplexDecision allocation = new SimplexDecision()
			{
				Memory = 0,
				Disk = 0,
				Tape = 100000
			};
			_simplex.AssignEvents(allocation, events);

			foreach (Event e in events)
			{
				Assert.AreEqual(FooCDNAccessor.StorageType.Tape, e.StorageLocation);
			}
		}

		public bool CheckCost(SimplexDecision allocations, double numViews)
		{
			//rounding in case cost is 15.00000000001 (floating point silliness)
			return Math.Round((0.25 + 0.3 * numViews) * allocations.Memory + (0.025 + 0.1 * numViews) * allocations.Disk, 2) <= 15;
		}
	}
}
