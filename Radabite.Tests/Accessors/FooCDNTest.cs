using Microsoft.VisualStudio.TestTools.UnitTesting;
using RadabiteServiceManager;
using Radabite.Backend.Interfaces;
using Ninject;
using Radabite.Backend.Accessors;
using System.Collections.Generic;
using System.Net;
using System.Text;

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
    const string jpegBlob = "c1485afb-d055-4f2f-a73e-c4e1bc22d2e9";
    const string plainTextBlob = "49971910-8aa5-4b8c-99fd-c37f6b98be92";
    const string postTextBlob = "8a5de7e6-3760-4908-b19d-6c619e19c522";

        
        [TestInitialize]
		public void Setup()
		{
			ServiceManager.Kernel.Rebind<IFooCDNAccessor>().To<FooCDNAccessor>().InSingletonScope();
		}

		[TestMethod]
		public void FooGetImageTest()
		{
			//GET from the test blob
			var result = ServiceManager.Kernel.Get<IFooCDNAccessor>().Get(jpegBlob, "image/jpeg");

			Assert.IsNotNull(result.Value);
		}

		[TestMethod]
		public void FooGetTextPlainTest()
		{
			//GET from the test blob
			var result = ServiceManager.Kernel.Get<IFooCDNAccessor>().Get(plainTextBlob, "text/plain");
			var mString = Encoding.ASCII.GetString(result.Value as byte[]);
			Assert.AreEqual(mString, "test");
		}

		[TestMethod]
		public void FooGetInfoTest()
		{
			var result = ServiceManager.Kernel.Get<IFooCDNAccessor>().GetInfo(jpegBlob).Value as Dictionary<string, dynamic>;

			Assert.AreEqual(result["BlobID"], jpegBlob);
			Assert.AreEqual(result["MimeType"], "image/jpeg");
		}

		[TestMethod]
		public void FooPutTest()
		{
			var result = ServiceManager.Kernel.Get<IFooCDNAccessor>().Put(jpegBlob, FooCDNAccessor.StorageType.Tape);

			//Status code 204 (No Content) is intentional, because we do not pass content for PUT
			Assert.IsTrue(result.StatusCode == HttpStatusCode.NoContent);
		}

		[TestMethod]
		public void FooPostTest()
		{
			byte[] testData = Encoding.ASCII.GetBytes("Testing Post");

			var result = ServiceManager.Kernel.Get<IFooCDNAccessor>().Post(postTextBlob, testData);

			Assert.IsNotNull(result.Value);
			Assert.IsTrue(result.StatusCode == HttpStatusCode.Created);
		}

		// NOTE: This test uses DELETE to avoid creating garbage blobs on runs
		[TestMethod]
		public void FooCreateTest()
		{
			var result = ServiceManager.Kernel.Get<IFooCDNAccessor>().CreateBlob("image/jpeg");

			//the returned result is the new blob's ID
			Assert.IsNotNull(result);

			var deleteResult = ServiceManager.Kernel.Get<IFooCDNAccessor>().Delete(result.Value as string);
		}

		// NOTE: This test for delete depends on functioning POST (create blob) and POST (upload to blob)
		[TestMethod]
		public void FooDeleteTest()
		{
			/* Creates a new blob, and puts text in it */
			var createdBlob = ServiceManager.Kernel.Get<IFooCDNAccessor>().CreateBlob("text/plain").Value as string;
			byte[] testData = Encoding.ASCII.GetBytes("Testing Delete");
			ServiceManager.Kernel.Get<IFooCDNAccessor>().Post(createdBlob, testData);

			var result = ServiceManager.Kernel.Get<IFooCDNAccessor>().Delete(createdBlob);

			/* 
			 * If the blob is empty, FooCDN (as of last check) will successfully delete the blob, but return a 
			 * 500 (InternalServerError) status code
			 * If the blob has contents, it is deleted, and status code is 200 (OK)
			 */
			Assert.IsTrue(result.StatusCode == HttpStatusCode.OK);
		}

		[TestMethod]
		public void FooChainTest()
		{
			string originalString = "Testing the Foo chain";

			/* Creates a new blob, and puts text in it */
			var createdBlob = ServiceManager.Kernel.Get<IFooCDNAccessor>().CreateBlob("text/plain").Value as string;
			byte[] testData = Encoding.ASCII.GetBytes(originalString);
			ServiceManager.Kernel.Get<IFooCDNAccessor>().Post(createdBlob, testData);

			var getResult = ServiceManager.Kernel.Get<IFooCDNAccessor>().Get(createdBlob, "text/plain");

			string fooString = Encoding.ASCII.GetString(getResult.Value as byte[]);
			Assert.AreEqual(fooString, originalString);

			//for cleanup
			ServiceManager.Kernel.Get<IFooCDNAccessor>().Delete(createdBlob);
		}
	}
}
