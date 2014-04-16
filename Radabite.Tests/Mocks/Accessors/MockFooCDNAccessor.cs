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
			return new FooResponse();
		}

		public FooResponse GetInfo(string blobID)
		{
			Dictionary<string, dynamic> dict = new Dictionary<string, dynamic>();
			dict.Add("BlobSize", "10");

			return new FooResponse()
			{
				Value = dict,
				StatusCode = System.Net.HttpStatusCode.OK
			};
		}

		public FooResponse Post(string blobID, byte[] data)
		{
			return new FooResponse();
		}

		public FooResponse Put(string blobID, FooCDNAccessor.StorageType type)
		{
			return new FooResponse();
		}

		public FooResponse CreateBlob(string mimeType)
		{
			return new FooResponse();
		}

		public FooResponse Delete(string blobID)
		{
			return new FooResponse();
		}
	}
}
