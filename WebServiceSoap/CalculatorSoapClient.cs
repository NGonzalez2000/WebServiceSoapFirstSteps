using System.Text;
using System.Xml;

namespace WebServiceSoap
{
    internal class CalculatorSoapClient
    {
        static readonly string apiUrl = "http://www.dneonline.com/calculator.asmx";
        private static int XmlParser(XmlDocument doc, string action)
        {
            XmlNamespaceManager ns = new(doc.NameTable);
            ns.AddNamespace("soap", "http://schemas.xmlsoap.org/soap/envelope/");
            ns.AddNamespace("tempuri", "http://tempuri.org/");

            XmlNode? node = doc.SelectSingleNode($"//tempuri:{action}Result", ns);
            return node is null ? 0 : Convert.ToInt32(node.InnerText);
        }
        private static int ApiPost(string operation, string action)
        {
            using HttpClient client = new();
            StringContent content = new($@"<?xml version=""1.0"" encoding=""utf-8""?>
            <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
              xmlns:xsd=""http://www.w3.org/2001/XMLSchema""
              xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
              <soap:Body>
                {operation}
              </soap:Body>
            </soap:Envelope>",
            Encoding.UTF8,
            "text/xml");

            content.Headers.Add("SOAPAction", $"\"http://tempuri.org/{action}\"");
            HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;

            string result;
            using(Stream stream = response.Content.ReadAsStream())
            {
                using StreamReader reader = new(stream);
                result = reader.ReadToEnd();
            }

            XmlDocument doc = new();
            doc.LoadXml(result);
            return XmlParser(doc,action);
        }
        internal static int Calculate(int x, int y, string action)
        {
            string operation = 
                @$"<{action} xmlns=""http://tempuri.org/"">
                        <intA>{x}</intA>
                        <intB>{y}</intB>
                   </{action}>";
            return ApiPost(operation, action);
        }

        
    }
}
