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
using System.Net.Http.Formatting;
using System.IO;
using System.Configuration;

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
		Uri _baseUri = new Uri("http://foocdn.azurewebsites.net");

		public FooResponse Get(string blobID, string mediaType)
		{
			using (var fooCDN = new HttpClient())
			{
				fooCDN.BaseAddress = _baseUri;
				fooCDN.DefaultRequestHeaders.Accept.Clear();
				fooCDN.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

				HttpResponseMessage response = fooCDN.GetAsync("/api/content/" + blobID).Result;

				byte[] bytes = null;
				if(response.IsSuccessStatusCode)
				{
					bytes = response.Content.ReadAsByteArrayAsync().Result;
				}

				FooResponse fooResult = new FooResponse()
				{
					Value = bytes,
					StatusCode = response.StatusCode
				};

				return fooResult;
			}
		}


		public FooResponse GetInfo(string blobID)
		{
			using (var fooCDN = new HttpClient())
			{
				fooCDN.BaseAddress = _baseUri;
				fooCDN.DefaultRequestHeaders.Accept.Clear();
				fooCDN.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage response = fooCDN.GetAsync("/api/content/" + blobID + "/info").Result;

				var result = response.Content.ReadAsAsync<Object>().Result;

				//Into a dictionary for now
				Dictionary<string, dynamic> dictionaryResult = null;

				if(response.IsSuccessStatusCode)
				{
					dictionaryResult = _serializer.Deserialize<Dictionary<string, dynamic>>(result.ToString());
				}

				FooResponse fooResult = new FooResponse()
				{
					Value = dictionaryResult,
					StatusCode = response.StatusCode
				};

				return fooResult;
			}
		}

		public FooResponse Post(string blobID, string filename)
		{
			using (WebClient fooCDN = new WebClient())
			{
				FooResponse fooResult;

				try
				{
					byte[] response = fooCDN.UploadFile("http://foocdn.azurewebsites.net/api/content/" + blobID, filename);

					fooResult = new FooResponse()
					{
						Value = response,

						//With post as it is, there is no HttpResponse, so it is always OK here
						StatusCode = HttpStatusCode.OK
					};
				}
				catch
				{
					fooResult = new FooResponse()
					{
						Value = null,
						StatusCode = HttpStatusCode.InternalServerError
					};
				}

				return fooResult;
			}
		}

		public FooResponse Put(string blobID, StorageType type)
		{
			using (var fooCDN = new HttpClient())
			{
				fooCDN.BaseAddress = _baseUri;
				fooCDN.DefaultRequestHeaders.Accept.Clear();

				HttpContent empty = new StringContent("");
				empty.Headers.Add("Content-Length", "0");

				HttpResponseMessage response = fooCDN.PutAsync("/api/content/" + blobID + "?type=" + type.ToString(), empty).Result;

				FooResponse fooResult = new FooResponse()
				{
					StatusCode = response.StatusCode
				};
				var x = response.IsSuccessStatusCode;
				return fooResult;
			}
		}

		public FooResponse CreateBlob(string mimeType)
		{
			var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://foocdn.azurewebsites.net/api/content/add/");
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "POST";
			var cdnKey = ConfigurationManager.AppSettings["fooCdnKey"];
			using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				string json = String.Format("{{ \"AccountKey\": \"{0}\", \"MimeType\":\"" + mimeType + "\"}}", cdnKey);
				streamWriter.Write(json);
			}
			var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

			string createdBlob = null;

			if (httpResponse.StatusCode == HttpStatusCode.OK)
			{
				using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
				{
					createdBlob = streamReader.ReadToEnd();

					//get rid of the quotes surrounding it
					createdBlob = createdBlob.Substring(1, createdBlob.Length - 2);
				}
			}

			FooResponse fooResult = new FooResponse()
			{
				Value = createdBlob,
				StatusCode = httpResponse.StatusCode
			};

			return fooResult;
		}

		public FooResponse Delete(string blobID)
		{
			using (var fooCDN = new HttpClient())
			{
				fooCDN.BaseAddress = _baseUri;
				fooCDN.DefaultRequestHeaders.Accept.Clear();

				HttpResponseMessage response = fooCDN.DeleteAsync("/api/content/" + blobID).Result;

				FooResponse fooResult = new FooResponse()
				{
					StatusCode = response.StatusCode
				};

				return fooResult;
			}
		}
	}

	public class FooResponse
	{
		public Object Value { get; set; }
		public HttpStatusCode StatusCode { get; set; }
	}
}