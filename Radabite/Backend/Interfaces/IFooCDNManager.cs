using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Radabite.Backend.Interfaces
{
	public interface IFooCDNManager
	{
		byte[] Get(string blobID);

		IDictionary<string, dynamic> GetInfo(string blobID);

		HttpResponseMessage Post(string blobID, HttpContent content);

		HttpResponseMessage Put(string blobID, Radabite.Backend.Accessors.FooCDNAccessor.StorageType type);
	}
}