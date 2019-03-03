using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;
using SpecFlow.Assist.Dynamic;

namespace AssurityTest
{
    [Binding]
    public sealed class AssurityStepDefinition
    {
        [Given(@"A request is made to the service")]
        public void SoapRequestToService(Table service)
        {
            string Url = String.Empty;
            var tblService = service.CreateDynamicSet();
            foreach (var data in tblService)
            {
                Url = data.Url;
            }
            SoapMethods.InitializeSoapRequestTest(Url);
        }

        [When("The response has been saved")]
        public void SaveTheResponse()
        {
            SoapMethods.SaveFormattedXMLResponse();   
        }

        [Then(@"The service status code should be (.*)")]
        public void ThenTheServiceStatusCodeShouldBe(int expectedStatusCode)
        {
            int responseStatusCode = Convert.ToInt32(ScenarioContext.Current["ResponseStatusCode"]);
            Assert.AreEqual(expectedStatusCode, responseStatusCode);
        }

        [Then(@"Name is (.*), CanRelist is (.*) and Promotion element (.*) has a description containing text (.*)")]
        public void ThenNameIsCarbonCreditsCanRelistIsTrueAndPromotionElementGalleryHasADescriptionContainingTextXLargerImage(string Name, string RFlag, string PromoName, string Text)
        {
            string json = (string)ScenarioContext.Current["SoapResult"];

            var data = (JObject)JsonConvert.DeserializeObject(json);
            
            string strName = data["Name"].Value<string>();
            Assert.AreEqual(Name, strName, Name + "doesn't match with " + strName + " in the response");

            string strRelist = data["CanRelist"].Value<string>();
            Assert.AreEqual(RFlag, strRelist, RFlag + "doesn't match with " + strRelist + " in the response");

            bool returnFlag = EvaluatePromotionAndDescription(data, PromoName, Text);

            Assert.AreEqual(true, returnFlag, "Promotion name " + PromoName + " not found with description " + Text);
        }

        //Function checks if Promotion Name with description are present in the response
        bool EvaluatePromotionAndDescription(JObject data, string PromoName, string desc)
        {
            int i = 0;
            bool flag = false;
            string promoDesc = "";
            var promoNames =
                from p in data["Promotions"]
                select (string)p["Name"];

            foreach (var item in promoNames)
            {
                if (item.Equals(PromoName))
                {
                    promoDesc = (string)data["Promotions"][i]["Description"];

                    flag = (promoDesc.Contains(desc)) ? true : false;
                    break;
                }
                i = i + 1;
            }
            return flag;
        }
    }
}
