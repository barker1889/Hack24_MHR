using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Titanium.Web.Proxy;
using Titanium.Web.Proxy.EventArguments;
using Titanium.Web.Proxy.Models;

namespace Proxy
{
    public class ProxyTestController
    {
        private readonly ProxyServer _proxyServer;

        public ProxyTestController()
        {
            _proxyServer = new ProxyServer
                           {
                               TrustRootCertificate = true
                           };
        }

        public void StartProxy()
        {
            _proxyServer.BeforeRequest += OnRequest;
            _proxyServer.ServerCertificateValidationCallback += OnCertificateValidation;
            _proxyServer.ClientCertificateSelectionCallback += OnCertificateSelection;

            var explicitEndPoint = new ExplicitProxyEndPoint(IPAddress.Any, 8000, true);

            _proxyServer.AddEndPoint(explicitEndPoint);
            _proxyServer.Start();


            foreach (var endPoint in _proxyServer.ProxyEndPoints)
                Console.WriteLine("Listening on '{0}' endpoint at Ip {1} and port: {2} ",
                    endPoint.GetType().Name, endPoint.IpAddress, endPoint.Port);

            _proxyServer.SetAsSystemHttpProxy(explicitEndPoint);
            _proxyServer.SetAsSystemHttpsProxy(explicitEndPoint);
        }

        public void Stop()
        {
            _proxyServer.BeforeRequest -= OnRequest;
            _proxyServer.ServerCertificateValidationCallback -= OnCertificateValidation;
            _proxyServer.ClientCertificateSelectionCallback -= OnCertificateSelection;

            _proxyServer.Stop();
        }

        public async Task OnRequest(object sender, SessionEventArgs e)
        {
            var method = e.WebSession.Request.Method.ToUpper();
            if (method == "POST" || method == "PUT" || method == "PATCH")
            {
                var bodyBytes = await e.GetRequestBody();
                await e.SetRequestBody(bodyBytes);

                var bodyString = await e.GetRequestBodyAsString();
                await e.SetRequestBodyString(bodyString);
            }

            var uri = e.WebSession.Request.RequestUri;

            if (uri.AbsoluteUri.Contains("google.com/mail") && uri.Query.Contains("act=sm") && method == "POST")
            {
                var coll = HttpUtility.ParseQueryString(await e.GetRequestBodyAsString());

                var plainTextBody = HtmlToText.ConvertHtml(coll["body"]);

                using (var client = new HttpClient())
                {
                    client.PostAsync("http://localhost:56613/api/Data/PostData", new StringContent(plainTextBody));
                }
            }

        }

        public Task OnCertificateValidation(object sender, CertificateValidationEventArgs e)
        {
            if (e.SslPolicyErrors == System.Net.Security.SslPolicyErrors.None)
            {
                e.IsValid = true;
            }

            return Task.FromResult(0);
        }

        public Task OnCertificateSelection(object sender, CertificateSelectionEventArgs e)
        {
            return Task.FromResult(0);
        }
    }
}