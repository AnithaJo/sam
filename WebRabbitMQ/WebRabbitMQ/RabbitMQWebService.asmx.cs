using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Services;

namespace WebRabbitMQ
{
    /// <summary>
    /// Summary description for RabbitMQWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class RabbitMQWebService : WebService
    {
        static string filePath = @"C:\eVanceIntegration\Projects\Store\EventData.json";
        static FileStream fs;
        static StreamWriter sw;
        static JsonWriter jw;
        static JsonSerializer serializer;

        static RabbitMQWebService()
        {
            fs = File.Open(filePath, FileMode.Append);
            sw = new StreamWriter(fs);
            jw = new JsonTextWriter(sw);
            jw.Formatting = Formatting.Indented;
            serializer = new JsonSerializer();
        }


        [WebMethod]
        public void ReceiveEvent(string eventData)
        {
            if (string.IsNullOrWhiteSpace(eventData))
                return;

            Event evt = (Event)JsonConvert.DeserializeObject(eventData, typeof(Event));
            //serializer.Serialize(sw, evt);
            WriteToJsonFileFormatted(evt);

            //var eventsList = ReadFromJsonFile();
            //eventsList.Add(evt);
            //WriteToJsonFile(eventsList);
        }

        private void WriteToJsonFileFormatted(Event evt)
        {
            try
            {
                serializer.Serialize(jw, evt);
            }
            catch (Exception err)
            {
            }
        }

        private void WriteToJsonFile(object data)
        {
            try
            {
                FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite);
                StreamWriter sw = new StreamWriter(fs);
                JsonWriter jw = new JsonTextWriter(sw);
                jw.Formatting = Formatting.Indented;

                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(jw, data);
                jw.Close();
                sw.Close();
                fs.Close();
            }
            catch (Exception err)
            {
            }
        }

        private List<Event> ReadFromJsonFile()
        {
            if (!File.Exists(filePath))
            {
                File.Create(filePath);
                return new List<Event>();
            }

            try
            {
                StreamReader r = new StreamReader(filePath);
                string json = r.ReadToEnd();
                r.Close();
                List<Event> items = JsonConvert.DeserializeObject<List<Event>>(json);
                return items ?? new List<Event>();
            }
            catch (Exception err)
            {
                return new List<Event>();
            }
        }

        public class Event
        {
            public string NodeAddress { get; set; }
            public int EventTyp { get; set; }
            public string EventDescription { get; set; }
            public string Timestamp { get; set; }
            public string AcctCode { get; set; }
        }
    }
}
