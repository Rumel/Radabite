using Radabite.Backend.Accessors;
using Radabite.Backend.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radabite.Tests.Mocks.Accessors
{
	class MockFooCDNAccessor : IFooCDNAccessor
	{
		public FooResponse Get(string blobID, string mediaType)
		{
            return new FooResponse()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Value = new byte[1]
            };
		}

		public FooResponse GetInfo(string blobID)
		{
			Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
			dict.Add("BlobSize", "1000000");

			return new FooResponse()
			{
				Value = dict,
				StatusCode = System.Net.HttpStatusCode.OK
			};
		}

		public FooResponse Post(string blobID, byte[] data)
		{
            return new FooResponse()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Value = new byte[1]
            };
		}

		public FooResponse Put(string blobID, FooCDNAccessor.StorageType type)
		{
            return new FooResponse()
            {
                StatusCode = System.Net.HttpStatusCode.OK
            };
		}

		public FooResponse CreateBlob(string mimeType)
		{
            return new FooResponse()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Value = "blob string"
            };
		}

		public FooResponse Delete(string blobID)
		{
			return new FooResponse();
		}
	}
}
