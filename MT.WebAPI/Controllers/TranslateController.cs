using System.Net.Http;
using System.Web.Http;
using System.Xml;
using AttributeRouting.Web.Http;
using Newtonsoft.Json;

namespace MT.WebAPI.Controllers
{
    [RoutePrefix("api/translate/json")]
    public class TranslateController : ApiController
    {
        [POST("")]
        public string PostXml(HttpRequestMessage xml)
        {
            var xmlContent = xml.Content.ReadAsStringAsync().Result;
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xmlContent);
            return JsonConvert.SerializeXmlNode(xmlDocument);
        }
    }
}
