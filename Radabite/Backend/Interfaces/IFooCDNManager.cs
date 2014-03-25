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
		byte[] Get(string blobID, string mediaType);

		IDictionary<string, dynamic> GetInfo(string blobID);

		byte[] Post(string blobID, string filename);

		HttpResponseMessage Put(string blobID, FooCDNAccessor.StorageType type);

		string CreateBlob(string mimeType);

		HttpResponseMessage Delete(string blobID);

	}
}