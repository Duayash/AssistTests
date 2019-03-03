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
            //XmlDocument soapEnvelopeXml = CreateSoapEnvelope();
            webRequest = CreateWebRequest(serviceURL);
            //InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);
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

        private static XmlDocument CreateSoapEnvelope()
        {
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\"));
            string testDataFolder = path + "\\TestData";
            XmlDocument soapEnvelopeDocument = new XmlDocument();
            soapEnvelopeDocument.Load(testDataFolder + "\\" + ".xml");
            string envString = soapEnvelopeDocument.OuterXml;
            return soapEnvelopeDocument;
        }

        private static HttpWebRequest CreateWebRequest(string serviceURL)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(serviceURL);
            
            webRequest.ContentType = "application/xml";
            webRequest.Method = "GET";
            return webRequest;
        }

        private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        {
            using (Stream stream = webRequest.GetRequestStream())
            {
                soapEnvelopeXml.Save(stream);
            }
        }

        public static void SaveResponseAsXML(string soapResult)
        {
            string path = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\..\\"));
            string targetPath = path + "\\Response";
            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);
            System.IO.File.WriteAllText(targetPath + "\\" + "Response.xml", soapResult);
        }

        public static void SaveFormattedXMLResponse(string responseVariable = "SoapResult")
        {
            if (ScenarioContext.Current.ContainsKey(responseVariable))
            {
                string json = (string)ScenarioContext.Current[responseVariable];

                var data = (JObject)JsonConvert.DeserializeObject(json);
                //string timeZone = data["Atlantic/Canary"].Value<string>();
                //string formattedXML = SoapMethods.FormatToXML((string)ScenarioContext.Current[responseVariable]);
                //SoapMethods.SaveResponseAsXML(formattedXML);
            }
        }

        public static string FormatToXML(string xml)
        {
            string result = "";
            char chrRemove = Convert.ToChar("\\");

            xml.Remove(xml.IndexOf(chrRemove));
            MemoryStream memoryStream = new MemoryStream();
            XmlTextWriter writer = new XmlTextWriter(memoryStream, Encoding.Unicode);
            XmlDocument document = new XmlDocument();

            try
            {
                document.LoadXml(xml);
                //writer.Formatting = Formatting.Indented;
                document.WriteContentTo(writer);
                writer.Flush();
                memoryStream.Flush();
                memoryStream.Position = 0;
                StreamReader streamReader = new StreamReader(memoryStream);
                string formattedXml = streamReader.ReadToEnd();
                result = formattedXml;
            }
            catch (XmlException ex)
            {
            }
            memoryStream.Close();
            writer.Close();
            return result;
        }
    }
}
