using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RadabiteServiceManager;
using Radabite.Backend.Interfaces;
using Ninject;
using Radabite.Backend.Accessors;

namespace Radabite.Tests.Accessors
{
	/* NOTE: These tests are currently dependent on FooCDN
	 * If FooCDN is down, they will fail */

	[TestClass]
	public class FooCDNTest
	{
		[TestInitialize]
		public void Setup()
		{
			ServiceManager.Kernel.Rebind<IFooCDNAccessor>().To<FooCDNAccessor>().InSingletonScope();
		}

		[TestMethod]
		public void FooGetTest()
		{
			//GET from the test blob
			var result = ServiceManager.Kernel.Get<IFooCDNAccessor>().Get("c1485afb-d055-4f2f-a73e-c4e1bc22d2e9", "image/jpeg");

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void FooGetInfoTest()
		{
			var result = ServiceManager.Kernel.Get<IFooCDNAccessor>().GetInfo("c1485afb-d055-4f2f-a73e-c4e1bc22d2e9");

			Assert.AreEqual(result["BlobID"], "c1485afb-d055-4f2f-a73e-c4e1bc22d2e9");
			Assert.AreEqual(result["MimeType"], "image/jpeg");
		}

		[TestMethod]
		public void FooPutTest()
		{
			var result = ServiceManager.Kernel.Get<IFooCDNAccessor>().Put("c1485afb-d055-4f2f-a73e-c4e1bc22d2e9", FooCDNAccessor.StorageType.Tape);

			Assert.IsTrue(result.IsSuccessStatusCode);
		}
	}
}
