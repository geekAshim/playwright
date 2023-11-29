using EPAM.PlaywrightFW.Core;
using Microsoft.Net.Http.Headers;
using Microsoft.Playwright;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPAM.Playwright.Test
{
    [TestFixture]
    public class IHttpClientFactoryAPITest
    {
        IHttpClientFactory _httpClientFactory;
        HttpClient _httpClient;

        //public IHttpClientFactoryAPITest(IHttpClientFactory httpClientFactory)
        //{
        //    _httpClientFactory = httpClientFactory;                            
        //}

        [OneTimeSetUp]        
        public void Setup()
        {
            TestSession.Current.Start();
            _httpClientFactory = Task.Run(() => TestSession.Current.Resolve<IHttpClientFactory>()).Result;
            _httpClient = _httpClientFactory.CreateClient();
        }

        [Test]
        public async Task SampleAPITestAsync()
        {
            var httpRequestMessage = new HttpRequestMessage(
            HttpMethod.Get,
            "https://dummy.restapiexample.com/api/v1/employees")
            {
                Headers =
                {
                    { HeaderNames.Accept, "application/vnd.github.v3+json" },
                    { HeaderNames.UserAgent, "HttpRequestsSample" }
                }
            };

            var httpResponseMessage = await _httpClient.SendAsync(httpRequestMessage);            

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream =
                    await httpResponseMessage.Content.ReadAsStreamAsync();
            }

            Assert.IsTrue(httpResponseMessage.IsSuccessStatusCode);
        }

        [OneTimeTearDown]
        public void CleanUp()
        {
            if (_httpClient != null)
            {
                _httpClient.Dispose();
            }
        }

        public void Dispose() 
        {
            if(_httpClient != null)
            {
                _httpClient.Dispose();                
            }            
        }
    }
}
