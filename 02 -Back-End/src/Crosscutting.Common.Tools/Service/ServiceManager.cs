using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Crosscutting.Common.Tools.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using RestSharp;
using System.Web;

namespace Crosscutting.Common.Tools.Service
{
    public class ServiceManager
    {
        public string TokenAccess { get; set; }
        public ServiceManager(string path)
        {
            _path = path;
        }

        private readonly string _path;

        public async Task<HttpResponseMessage> CallServiceAsync(string action, string content, string tokenAccess = "",
            RestMethod method = RestMethod.Post, Dictionary<string, string> headers = null)
        {
            using (var client = new HttpClient())
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                if (!string.IsNullOrEmpty(tokenAccess))
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenAccess);
                }

                if (headers != null && headers.Any())
                {
                    foreach (var header in headers)
                    {
                        client.DefaultRequestHeaders.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                HttpResponseMessage result;

                switch (method)
                {
                    case RestMethod.Get:

                        var parameters = string.Empty;
                        if (content != "")
                        {
                            var json = JsonConvert.DeserializeObject<JObject>(content);
                            var properties = json.Properties().ToList();

                            var counter = 0;
                            bool last;
                            foreach (var item in properties)
                            {
                                counter++;
                                last = counter == properties.Count();
                                if (!last)
                                {
                                    parameters += item.Name + "=" + item.Value + "&";
                                }
                                else
                                {
                                    parameters += item.Name + "=" + item.Value;
                                }
                            }
                        }


                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));
                        result = await client.GetAsync(_path + "/" + action + (!string.IsNullOrEmpty(parameters) ? "?" : string.Empty) + parameters).ConfigureAwait(false);

                        break;

                    case RestMethod.Post:
                        result = await client.PostAsync(_path + "/" + action,
                           new StringContent(content, Encoding.UTF8, "application/json"))
                            .ConfigureAwait(false);
                        break;

                    case RestMethod.Patch:
                        var patchMethod = new HttpMethod("PATCH");
                        HttpContent httpContent = new StringContent(content, Encoding.UTF8, "application/json");
                        var request = new HttpRequestMessage(patchMethod, new Uri(_path + "/" + action))
                        {
                            //Content = new StringContent(content, Encoding.UTF8, "application/json")
                            Content = httpContent
                        };
                        result = await client.SendAsync(request).ConfigureAwait(false);
                        break;
                    case RestMethod.Put:
                        result =
                            await
                                client.PutAsync(_path + "/" + action,
                                    new StringContent(content, Encoding.UTF8, "application/json")).ConfigureAwait(false);
                        break;
                    case RestMethod.Delete:
                        var deleteMethod = new HttpMethod("DELETE");
                        HttpContent contdelete = new StringContent(content, Encoding.UTF8, "application/json");
                        var requestdelete = new HttpRequestMessage(deleteMethod, new Uri(_path + "/" + action))
                        {
                            Content = contdelete
                        };
                        result = await client.SendAsync(requestdelete).ConfigureAwait(false);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(method), method, null);
                }


                return result;
            }
        }

        public String UrlEncode(string actionUrl, Dictionary<string, object> parameters, Dictionary<string, string> headers = null)
        {
            try
            {
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                var client = new RestClient($"{_path}/{actionUrl}");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                if (headers != null && headers.Any())
                {
                    foreach (var header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }

                foreach (var item in parameters)
                {
                    request.AddParameter(item.Key, item.Value);
                }

                IRestResponse response = client.Execute(request);
                return response.Content;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string FormData(string actionUrl, Dictionary<string, byte[]> parameters, Dictionary<string, string> headers = null)
        {
            try
            {
                var client = new RestClient($"{_path}/{actionUrl}");
                var request = new RestRequest(Method.POST);
                request.AddHeader("Cache-Control", "no-cache");
                request.AddHeader("Content-Type", "multipart/form-data");
                if (headers != null && headers.Any())
                {
                    foreach (var header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }
                foreach (var item in parameters)
                {
                    request.AddFileBytes(item.Key.Split('.')[0], item.Value, "a", MimeMapping.GetMimeMapping(item.Key));
                }

                IRestResponse response = client.Execute(request);
                return response.Content;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }

}