using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Helpers
{
    public class RestHelper
    {
        public async Task<T> APICaller<T>(string baseUrl, string resource, Method method, object body, List<Tuple<string, object>> urlSegment = null, List<Tuple<string, object>> parameters = null, List<Tuple<string, string>> headers = null, List<string> files = null, string username=null, string password=null)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

            var client = new RestClient(baseUrl);
            if(!string.IsNullOrWhiteSpace(username) && !string.IsNullOrWhiteSpace(password))
            {
                client.Authenticator = new HttpBasicAuthenticator(username, password); //new SimpleAuthenticator("username", username, "password", password);
            }
            var request = new RestRequest(resource, method);
            if (parameters != null && parameters.Any())
            {
                parameters.ForEach(p => request.AddParameter(p.Item1, p.Item2));
            }

            if (urlSegment != null && urlSegment.Any())
            {
                urlSegment.ForEach(s => request.AddUrlSegment(s.Item1, s.Item2));
            }

            if (headers != null && headers.Any())
            {
                headers.ForEach(h => request.AddHeader(h.Item1, h.Item2));
            }

            if (files != null && files.Any())
            {
                files.ForEach(f => request.AddFile("content", f));
            }

            if (body != null)
            {
                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(body);
            }
            var cancellationTokenSource = new CancellationTokenSource();
            var response = await client.ExecuteAsync(request, cancellationTokenSource.Token);

            if (response.StatusCode != System.Net.HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent && response.StatusCode!=HttpStatusCode.Unauthorized)
            {
                var param = string.Join(", ", request.Parameters.Select(x => x.Name.ToString() + "=" + ((x.Value == null) ? "NULL" : x.Value)).ToArray());
                if (response != null)
                {
                    //Set up the information message with the URL, the status code, and the parameters.
                    var info = "Request to " + baseUrl + request.Resource + " failed with status code " + response.StatusCode + ", parameters: "
                              + param + ", and content: " + response.Content;
                    if (response.ErrorException != null)
                    {
                        if (response.ErrorException.InnerException != null)
                        {
                            info = info + ", and exception: " + response.ErrorException.InnerException.Message;
                        }
                        else
                        {
                            info = info + ", and exception: " + response.ErrorException.Message;
                        }
                    }
                    throw new Exception(response.StatusCode.ToString(), new Exception(JsonConvert.SerializeObject(info)));
                }
                throw new Exception(response.StatusCode.ToString(), new Exception(JsonConvert.SerializeObject(param)));
                //const string message = "Error retrieving response.  Check inner details for more info.";
                //var exception = new Exception(message, response.ErrorException);
                //throw exception;
            }
            else if(response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new Exception("401");
            }

            //if (typeof(T).IsValueType || typeof(T).FullName == "System.String")
            //{
            //    return (T)(object)response.Content;
            //}
            return (T)JsonConvert.DeserializeObject(response.Content, typeof(T));
        }

        public async Task<T> Delete<T>(string baseUrl, string resource, object body = null, List<Tuple<string, object>> urlSegment = null, List<Tuple<string, object>> parameters = null, List<Tuple<string, string>> headers = null, List<string> files = null)
        {
            var res = await APICaller<T>(baseUrl, resource, Method.DELETE, body, urlSegment, parameters, headers, files);
            return res;
        }

        public async Task<T> Get<T>(string baseUrl, string resource, object body = null, List<Tuple<string, object>> urlSegment = null, List<Tuple<string, object>> parameters = null, List<Tuple<string, string>> headers = null, List<string> files = null, string username=null, string password=null)
        {
            var res = await APICaller<T>(baseUrl, resource, Method.GET, body, urlSegment, parameters, headers, files, username, password);
            return res;
        }

        public async Task<T> Post<T>(string baseUrl, string resource, object body, List<Tuple<string, object>> urlSegment = null, List<Tuple<string, object>> parameters = null, List<Tuple<string, string>> headers = null, List<string> files = null, string username=null, string password=null)
        {
            var res = await APICaller<T>(baseUrl, resource, Method.POST, body, urlSegment, parameters, headers, files, username,password);
            return res;
        }

        public async Task<T> Put<T>(string baseUrl, string resource, object body, List<Tuple<string, object>> urlSegment = null, List<Tuple<string, object>> parameters = null, List<Tuple<string, string>> headers = null, List<string> files = null)
        {
            var res = await APICaller<T>(baseUrl, resource, Method.PUT, body, urlSegment, parameters, headers, files);
            return res;
        }

    }
}
