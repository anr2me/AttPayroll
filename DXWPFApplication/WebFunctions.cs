using System;
using System.Collections.Generic;
//using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DXWPFApplication
{
    public class WebFunctions
    {
        //public string DefaultConn = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString; // Need System.Configuration reference

        public static string HttpGet(string URI, string ProxyString = null)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        public static string HttpPost(string URI, string Parameters, string ProxyString = null)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            req.Proxy = new System.Net.WebProxy(ProxyString, true);
            //Add these, as we're doing a POST
            req.ContentType = "application/x-www-form-urlencoded";
            req.Method = "POST";
            //We need to count how many bytes we're sending. Post'ed Faked Forms should be name=value&
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(Parameters);
            req.ContentLength = bytes.Length;
            System.IO.Stream os = req.GetRequestStream();
            os.Write(bytes, 0, bytes.Length); //Push it out there
            os.Close();
            System.Net.WebResponse resp = req.GetResponse();
            if (resp == null) return null;
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }

        public class RequestManager
        {
            public string LastResponse { protected set; get; }

            CookieContainer cookies = new CookieContainer();

            internal string GetCookieValue(Uri SiteUri, string name)
            {
                Cookie cookie = cookies.GetCookies(SiteUri)[name];
                return (cookie == null) ? null : cookie.Value;
            }

            public string GetResponseContent(HttpWebResponse response)
            {
                if (response == null)
                {
                    throw new ArgumentNullException("response");
                }
                Stream dataStream = null;
                StreamReader reader = null;
                string responseFromServer = null;

                try
                {
                    // Get the stream containing content returned by the server.
                    dataStream = response.GetResponseStream();
                    // Open the stream using a StreamReader for easy access.
                    reader = new StreamReader(dataStream);
                    // Read the content.
                    responseFromServer = reader.ReadToEnd();
                    // Cleanup the streams and the response.
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                    if (dataStream != null)
                    {
                        dataStream.Close();
                    }
                    response.Close();
                }
                LastResponse = responseFromServer;
                return responseFromServer;
            }

            public HttpWebResponse SendPOSTRequest(string uri, string content, string login, string password, bool allowAutoRedirect, IWebProxy proxy, string contenttype)
            {
                HttpWebRequest request = GeneratePOSTRequest(uri, content, login, password, allowAutoRedirect, proxy, contenttype);
                return GetResponse(request);
            }

            public HttpWebResponse SendGETRequest(string uri, string login, string password, bool allowAutoRedirect, IWebProxy proxy, string contenttype)
            {
                HttpWebRequest request = GenerateGETRequest(uri, login, password, allowAutoRedirect, proxy, contenttype);
                return GetResponse(request);
            }

            public HttpWebResponse SendRequest(string uri, string content, string method, string login, string password, bool allowAutoRedirect, IWebProxy proxy, string contenttype)
            {
                HttpWebRequest request = GenerateRequest(uri, content, method, login, password, allowAutoRedirect, proxy, contenttype);
                return GetResponse(request);
            }

            public HttpWebRequest GenerateGETRequest(string uri, string login, string password, bool allowAutoRedirect, IWebProxy proxy, string contenttype)
            {
                return GenerateRequest(uri, null, "GET", null, null, allowAutoRedirect, proxy, contenttype);
            }

            public HttpWebRequest GeneratePOSTRequest(string uri, string content, string login, string password, bool allowAutoRedirect, IWebProxy proxy, string contenttype)
            {
                return GenerateRequest(uri, content, "POST", null, null, allowAutoRedirect, proxy, contenttype);
            }

            internal HttpWebRequest GenerateRequest(string uri, string content, string method, string login, string password, bool allowAutoRedirect, IWebProxy proxy, string contenttype)
            {
                if (uri == null)
                {
                    throw new ArgumentNullException("uri");
                }
                // Create a request using a URL that can receive a post. 
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
                // Set the Method property of the request to POST.
                request.Method = method;
                // Set cookie container to maintain cookies
                request.CookieContainer = cookies;
                request.AllowAutoRedirect = allowAutoRedirect;
                request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
                // proxy
                if (proxy != null)
                {
                    request.Proxy = proxy;
                }
                // If login is empty use defaul credentials
                if (string.IsNullOrEmpty(login))
                {
                    request.Credentials = CredentialCache.DefaultNetworkCredentials;
                }
                else
                {
                    request.Credentials = new NetworkCredential(login, password);
                }
                if (method == "POST")
                {
                    request.SendChunked = true;
                    // Convert POST data to a byte array.
                    byte[] byteArray = Encoding.UTF8.GetBytes(content ?? "");
                    // Set the ContentType property of the WebRequest.
                    request.ContentType = contenttype ?? "application/x-www-form-urlencoded";
                    // Set the ContentLength property of the WebRequest.
                    request.ContentLength = byteArray.Length;
                    // Get the request stream.
                    Stream dataStream = request.GetRequestStream();
                    // Write the data to the request stream.
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    // Close the Stream object.
                    dataStream.Close();
                }
                // ssl
                var defaultValidator = System.Net.ServicePointManager.ServerCertificateValidationCallback;
                request.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) =>
                    certificate.Subject.Contains("O=DO_NOT_TRUST, OU=Created by http://www.fiddler2.com")
                    || (certificate.Subject == "CN=DRAC5 default certificate, OU=Remote Access Group, O=Dell Inc., L=Round Rock, S=Texas, C=US")
                    || (defaultValidator != null && defaultValidator(request, certificate, chain, sslPolicyErrors));
                return request;
            }

            internal HttpWebResponse GetResponse(HttpWebRequest request)
            {
                if (request == null)
                {
                    throw new ArgumentNullException("request");
                }
                HttpWebResponse response = null;
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                    cookies.Add(response.Cookies);
                    // Print the properties of each cookie.
                    Console.WriteLine("\nCookies: ");
                    foreach (Cookie cook in cookies.GetCookies(request.RequestUri))
                    {
                        Console.WriteLine("Domain: {0}, String: {1}", cook.Domain, cook.ToString());
                    }
                }
                catch (WebException ex)
                {
                    Console.WriteLine("Web exception occurred. Status code: {0}", ex.Status);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                return response;
            }

            private async Task<string> DecodeResponse(HttpWebResponse response)
            {
                foreach (System.Net.Cookie cookie in response.Cookies)
                {
                    cookies.Add(new Uri(response.ResponseUri.GetLeftPart(UriPartial.Authority)), cookie);
                }

                if (response.StatusCode == HttpStatusCode.Redirect)
                {
                    var location = response.Headers[HttpResponseHeader.Location];
                    if (!string.IsNullOrEmpty(location))
                        return await Get(new Uri(location));
                }

                var stream = response.GetResponseStream();
                var buffer = new System.IO.MemoryStream();
                var block = new byte[65536];
                var blockLength = 0;
                do
                {
                    blockLength = stream.Read(block, 0, block.Length);
                    buffer.Write(block, 0, blockLength);
                }
                while (blockLength == block.Length);

                return Encoding.UTF8.GetString(buffer.GetBuffer());
            }

            public async Task<string> Get(Uri uri)
            {
                var request = (HttpWebRequest)HttpWebRequest.Create(uri);
                var response = (HttpWebResponse)await request.GetResponseAsync();
                return await DecodeResponse(response);
            }

        }
    }
}
