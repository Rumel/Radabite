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

			Assert.IsTrue(result[0] > 0);
			Assert.AreEqual(result[1], 0);
			Assert.AreEqual(result[2], 0);
		}
	}
}
