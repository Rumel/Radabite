using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Radabite.Backend.Interfaces;
using Radabite.Backend.Accessors;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using RadabiteServiceManager;
using Ninject;

namespace Radabite.Backend.Managers
{
	public class FooCDNManager : IFooCDNManager
	{
		public byte[] Get(string blobID)
		{
			return ServiceManager.Kernel.Get<IFooCDNAccessor>().Get(blobID);
		}

		public IDictionary<string, dynamic> GetInfo(string blobID)
		{
			return ServiceManager.Kernel.Get<IFooCDNAccessor>().GetInfo(blobID);
		}

		public HttpResponseMessage Post(string blobID, HttpContent content)
		{
			return ServiceManager.Kernel.Get<IFooCDNAccessor>().Post(blobID, content);
		}

		public HttpResponseMessage Put(string blobID, Radabite.Backend.Accessors.FooCDNAccessor.StorageType type)
		{
			return ServiceManager.Kernel.Get<IFooCDNAccessor>().Put(blobID, type);
		}
	}
}