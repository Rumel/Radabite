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
using System.Net;

namespace Radabite.Backend.Managers
{
	public class FooCDNManager : IFooCDNManager
	{
		public FooResponse SaveNewItem(byte[] data, string mimeType, FooCDNAccessor.StorageType storageType)
		{
			FooResponse created = CreateBlob(mimeType);

			if(created.StatusCode == HttpStatusCode.OK)
			{
				string blobId = created.Value as string;

				FooResponse postResult = Post(created.Value as string, data);
				if(postResult.StatusCode == HttpStatusCode.Created)
				{
					FooResponse putResult = Put(blobId, storageType);
					if(putResult.StatusCode == HttpStatusCode.NoContent)
					{
						//create result has the blobId
						return created;
					}
					else
					{
						return putResult;
					}
				}
				else
				{
					return postResult;
				}
			}
			else
			{
				return created;
			}
		}

		public FooResponse Get(string blobID, string mediaType)
		{
			return ServiceManager.Kernel.Get<IFooCDNAccessor>().Get(blobID, mediaType);
		}

		public FooResponse GetInfo(string blobID)
		{
			return ServiceManager.Kernel.Get<IFooCDNAccessor>().GetInfo(blobID);
		}

		public FooResponse Post(string blobID, byte[] data)
		{
			return ServiceManager.Kernel.Get<IFooCDNAccessor>().Post(blobID, data);
		}

		public FooResponse Put(string blobID, FooCDNAccessor.StorageType type)
		{
			return ServiceManager.Kernel.Get<IFooCDNAccessor>().Put(blobID, type);
		}

		public FooResponse CreateBlob(string mimeType)
		{
			return ServiceManager.Kernel.Get<IFooCDNAccessor>().CreateBlob(mimeType);
		}

		public FooResponse Delete(string blobID)
		{
			return ServiceManager.Kernel.Get<IFooCDNAccessor>().Delete(blobID);
		}

	}
}