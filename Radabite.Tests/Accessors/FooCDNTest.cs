using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RadabiteServiceManager;
using Radabite.Backend.Interfaces;
using Ninject;
using Radabite.Backend.Accessors;
using System.Net.Http;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace Radabite.Tests.Accessors
{
	/* NOTE: These tests are currently dependent on FooCDN
	 * If FooCDN is down, they will fail
	 * 
	 * TODO: remove that dependency
	 * 
	 * c1485afb-d055-4f2f-a73e-c4e1bc22d2e9 is a test blob with image/jpeg in it
	 * 49971910-8aa5-4b8c-99fd-c37f6b98be92 is a test blob with text/plain "test" in it
	 * 8a5de7e6-3760-4908-b19d-6c619e19c522 is a test blob with text/plain for testing POST
	 *		If the post has worked correctly, the contents of the blob should be text
	 *		stating the most recent time the test was run
	 * 
	 * Both blobs are stored on tape and small enough that costs are 
	 * nearly negligible with the current amount of testing
	 */

	[TestClass]
	public class FooCDNTest
	{
		[TestInitialize]
		public void Setup()
		{
			ServiceManager.Kernel.Rebind<IFooCDNAccessor>().To<FooCDNAccessor>().InSingletonScope();
		}

		[TestMethod]
		public void FooGetImageTest()
		{
			//GET from the test blob
			var result = ServiceManager.Kernel.Get<IFooCDNAccessor>().Get("c1485afb-d055-4f2f-a73e-c4e1bc22d2e9", "image/jpeg");

			Assert.IsNotNull(result);
		}

		[TestMethod]
		public void FooGetTextPlainTest()
		{
			//GET from the test blob
			var result = ServiceManager.Kernel.Get<IFooCDNAccessor>().Get("49971910-8aa5-4b8c-99fd-c37f6b98be92", "text/plain");
			var mString = System.Text.Encoding.ASCII.GetString(result);
			Assert.AreEqual(mString, "test");
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

		[TestMethod]
		public void FooPostTest()
		{
			string testFilename = "testFile.txt";

			File.WriteAllText(testFilename, "Testing post");
			var result = ServiceManager.Kernel.Get<IFooCDNAccessor>().Post("8a5de7e6-3760-4908-b19d-6c619e19c522", testFilename);
			File.Delete(testFilename);

			Assert.IsNotNull(result);
		}

		// NOTE: This test uses DELETE to avoid creating garbage blobs on runs
		[TestMethod]
		public void FooCreateTest()
		{
			var result = ServiceManager.Kernel.Get<IFooCDNAccessor>().CreateBlob("image/jpeg");

			//the returned result is the new blob's ID
			Assert.IsNotNull(result);

			var deleteResult = ServiceManager.Kernel.Get<IFooCDNAccessor>().Delete(result);
		}

		// NOTE: This test for delete depends on functioning POST (create blob) and POST (upload to blob)
		[TestMethod]
		public void FooDeleteTest()
		{
			/* Creates a new blob, and puts text in it */
			var createdBlob = ServiceManager.Kernel.Get<IFooCDNAccessor>().CreateBlob("text/plain");
			string testFilename = "testFile.txt";
			File.WriteAllText(testFilename, "Testing delete");
			ServiceManager.Kernel.Get<IFooCDNAccessor>().Post(createdBlob, testFilename);
			File.Delete(testFilename);

			var result = ServiceManager.Kernel.Get<IFooCDNAccessor>().Delete(createdBlob);

			/* 
			 * If the blob is empty, FooCDN (as of last check) will successfully delete the blob, but return a 
			 * 500 (InternalServerError) status code
			 * If the blob has contents, it is deleted, and status code is 200 (OK)
			 */
			Assert.IsTrue(result.IsSuccessStatusCode);
		}
	}
}
