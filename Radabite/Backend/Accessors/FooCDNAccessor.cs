using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Radabite.Backend.Interfaces;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Drawing;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;

namespace Radabite.Backend.Accessors
{
	public class FooCDNAccessor : IFooCDNAccessor
	{
		public enum StorageType
		{
			MemCache = 0,
			Disk = 1,
			Tape = 2
		}

		JavaScriptSerializer _serializer = new JavaScriptSerializer();
		
		public byte[] Get(string blobID, string mediaType)
		{
			using (var fooCDN = new HttpClient())
			{
				fooCDN.BaseAddress = new Uri("http://foocdn.azurewebsites.net");
				fooCDN.DefaultRequestHeaders.Accept.Clear();
				fooCDN.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

				HttpResponseMessage response = fooCDN.GetAsync("/api/content/" + blobID).Result;

				if (response.IsSuccessStatusCode)
				{
					var bytes = response.Content.ReadAsByteArrayAsync().Result;

					return bytes;
				}
				else
				{
					throw new Exception("FooCDN GET not successful");
				}
			}
		}


		public IDictionary<string, dynamic> GetInfo(string blobID)
		{
			using (var fooCDN = new HttpClient())
			{
				fooCDN.BaseAddress = new Uri("http://foocdn.azurewebsites.net");
				fooCDN.DefaultRequestHeaders.Accept.Clear();
				fooCDN.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage response = fooCDN.GetAsync("/api/content/" + blobID + "/info").Result;

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadAsAsync<Object>().Result;
					
					return _serializer.Deserialize<Dictionary<string, dynamic>>(result.ToString());					
				}
				else
				{
					throw new Exception("FooCDN GET info not successful");
				}			
			}
		}

		public HttpResponseMessage Post(string blobID, HttpContent content)
		{
			using (var fooCDN = new HttpClient())
			{
				fooCDN.BaseAddress = new Uri("http://foocdn.azurewebsites.net");
				fooCDN.DefaultRequestHeaders.Accept.Clear();

				//What if this doesn't match the mime type that the blob has?
				//not always going to be JSON, silly!
					//set this based on some attribute of content?
				//fooCDN.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/plain"));
				

				HttpResponseMessage response = fooCDN.PostAsync("/api/content/" + blobID, content).Result;
				

				if (response.IsSuccessStatusCode)
				{
					var j = response.Content.ReadAsAsync<Object>().Result;
					
					return response;
				}
				else
				{
					throw new Exception("FooCDN POST not successful");
				}
			}
		}

		public HttpResponseMessage Put(string blobID, StorageType type)
		{
			using(var fooCDN = new HttpClient())
			{
				fooCDN.BaseAddress = new Uri("http://foocdn.azurewebsites.net");
				fooCDN.DefaultRequestHeaders.Accept.Clear();

				HttpContent empty = new StringContent("");
				empty.Headers.Add("Content-Length", "0");


				HttpResponseMessage response = fooCDN.PutAsync("/api/content/" + blobID + "?type=" + type.ToString(), empty).Result;

				if (response.IsSuccessStatusCode)
				{
					return response;
				}
				else
				{
					throw new Exception("FooCDN PUT not successful");
				}
			}
		}

	}
}