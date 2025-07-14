using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Helpers
{
    public class GlobalFormat
    {
        public FormatData LogFormat(HttpContext _HttpContext, object obj)
        {
            FormatData data = new FormatData();
            var principal = _HttpContext?.User;           
            if (principal != null && principal.HasClaim(c => c.Type.ToLower() == "UserId".ToLower()))
            {
                var activeUserId = principal.FindFirst(c => c.Type.ToLower() == "UserId".ToLower()).Value;
                data.UserId = activeUserId;
            }

            var settings = new JsonSerializerSettings();
            settings.Converters.Add(new IPAddressConverter());
            settings.Converters.Add(new IPEndPointConverter());
            settings.Formatting = Formatting.Indented;

            if (_HttpContext != null && _HttpContext.Connection != null)
            {
                data.IP = " RemoteIpAddress= " + JsonConvert.SerializeObject(_HttpContext.Connection.RemoteIpAddress, settings) + "  LocalIpAddress=" + JsonConvert.SerializeObject(_HttpContext.Connection.LocalIpAddress, settings);
            }
            if (obj != null)
            {
                data.Data = JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
                {
                    PreserveReferencesHandling = PreserveReferencesHandling.Objects,
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    Formatting=Formatting.Indented
                });
            }
            return data;
        }
    }
    public class FormatData
    {
        public string UserId { get; set; }
        public string IP { get; set; }
        public string Data { get; set; }

    }

    ///https://stackoverflow.com/questions/18668617/json-net-error-getting-value-from-scopeid-on-system-net-ipaddress
    class IPAddressConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IPAddress));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            return IPAddress.Parse((string)reader.Value);
        }
    }

    class IPEndPointConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IPEndPoint));
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            IPEndPoint ep = (IPEndPoint)value;
            JObject jo = new JObject();
            jo.Add("Address", JToken.FromObject(ep.Address, serializer));
            jo.Add("Port", ep.Port);
            jo.WriteTo(writer);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            IPAddress address = jo["Address"].ToObject<IPAddress>(serializer);
            int port = (int)jo["Port"];
            return new IPEndPoint(address, port);
        }
    }
}

