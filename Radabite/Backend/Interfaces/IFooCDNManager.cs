using Radabite.Backend.Accessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Radabite.Backend.Interfaces
{
	public interface IFooCDNManager
	{
		FooResponse Get(string blobID, string mediaType);

		FooResponse GetInfo(string blobID);

		FooResponse Post(string blobID, byte[] data);

		FooResponse Put(string blobID, FooCDNAccessor.StorageType type);

		FooResponse CreateBlob(string mimeType);

		FooResponse Delete(string blobID);
	}
}