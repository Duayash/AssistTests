using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TechTalk.SpecFlow;

namespace AssurityTest
{
    class SoapMethods
    {
        public static (string, int) CallWebService(string serviceURL)
        {
            HttpWebRequest webRequest = null;
            webRequest = CreateWebRequest(serviceURL);
         
            IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);
            asyncResult.AsyncWaitHandle.WaitOne();
            string soapResult;
            int statusCode;
            using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
            {
                using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
                {
                    soapResult = rd.ReadToEnd();
                }
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                statusCode = (int)response.StatusCode;
            }
            Assert.IsNotEmpty(soapResult);
            return (soapResult, statusCode);
        }


        public static void InitializeSoapRequestTest(string Url, string responseKey = "SoapResult", string statusCodeKey = "ResponseStatusCode", string testDataFile = null)
        {
            string soapResult;
            int responseStatusCode;
            
            if (!ScenarioContext.Current.ContainsKey(responseKey))
            {
                (soapResult, responseStatusCode) = SoapMethods.CallWebService(Url);
                
                ScenarioContext.Current.Add(statusCodeKey, responseStatusCode);
                ScenarioContext.Current.Add(responseKey, soapResult);
            }
        }

        private static HttpWebRequest CreateWebRequest(string serviceURL)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(serviceURL);
            
            webRequest.ContentType = "application/xml";
            webRequest.Method = "GET";
            return webRequest;
        }
    }
}
