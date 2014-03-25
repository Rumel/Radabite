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
		
		public byte[] Get(string blobID, string mediaType)
		{
			using (var fooCDN = new HttpClient())
			{
				fooCDN.BaseAddress = _baseUri;
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
				fooCDN.BaseAddress = _baseUri;
				fooCDN.DefaultRequestHeaders.Accept.Clear();
				fooCDN.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				HttpResponseMessage response = fooCDN.GetAsync("/api/content/" + blobID + "/info").Result;

				if (response.IsSuccessStatusCode)
				{
					var result = response.Content.ReadAsAsync<Object>().Result;
					
					//Into a dictionary for now
					return _serializer.Deserialize<Dictionary<string, dynamic>>(result.ToString());					
				}
				else
				{
					throw new Exception("FooCDN GET info not successful");
				}			
			}
		}

		public byte[] Post(string blobID, string filename)
		{
			WebClient fooCDN = new WebClient();

			byte[] response = fooCDN.UploadFile("http://foocdn.azurewebsites.net/api/content/" + blobID, filename);

			return response;
		}

		public HttpResponseMessage Put(string blobID, StorageType type)
		{
			using(var fooCDN = new HttpClient())
			{
				fooCDN.BaseAddress = _baseUri;
				fooCDN.DefaultRequestHeaders.Accept.Clear();

				HttpContent empty = new StringContent("");
				empty.Headers.Add("Content-Length", "0");

				HttpResponseMessage response = fooCDN.PutAsync("/api/content/" + blobID + "?type=" + type.ToString(), empty).Result;

				return response;
			}
		}

		public string CreateBlob(string mimeType)
		{
			var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://foocdn.azurewebsites.net/api/content/add/");
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "POST";

			using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
			{
				string json = "{ \"AccountKey\": \"E22EB65F52875\", \"MimeType\":\"" + mimeType + "\"}";

				streamWriter.Write(json);
			}
			var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

			if (httpResponse.StatusCode == HttpStatusCode.OK)
			{
				using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
				{
					var createdBlob = streamReader.ReadToEnd();

					//get rid of the quotes surrounding it
					createdBlob = createdBlob.Substring(1, createdBlob.Length - 2);

					return createdBlob;
				}
			}
			else
			{
				throw new Exception("POST: create new blob not successful");
			}
		}

		public HttpResponseMessage Delete(string blobID)
		{
			using (var fooCDN = new HttpClient())
			{
				fooCDN.BaseAddress = _baseUri;
				fooCDN.DefaultRequestHeaders.Accept.Clear();

				HttpResponseMessage response = fooCDN.DeleteAsync("/api/content/" + blobID).Result;

				return response;
			}
		}
	}
}