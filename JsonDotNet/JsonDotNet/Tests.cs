using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;

namespace JsonDotNet
{
    public class Tests
    {
        [Test]
        public void SimpleSerialize()
        {
            Product product = new Product
            {
                Name = "Product1",
                ExpiryDate = new DateTime(2008, 12, 28)
            };


            ColorfulProduct cp = new ColorfulProduct();
            cp.Product = product;
            cp.Color = "Red";

            //JsonSerializer serializer = new JsonSerializer();
            //serializer.Converters.Add(new JavaScriptDateTimeConverter());
            //serializer.NullValueHandling = NullValueHandling.Ignore;

            string productJson = JsonStringUtil.ToString(cp);

            var deserializedProduct = JsonStringUtil.FromString<ColorfulProduct>(productJson);

        }
    }

    public class JsonStringUtil
    {
        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            Converters = new List<JsonConverter> { new StringEnumConverter() { CamelCaseText = false } },
            NullValueHandling = NullValueHandling.Ignore
        };

        public static string ToString<T>(T o)
        {
            return JsonConvert.SerializeObject(o, Settings);
        }

        public static T FromString<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }
    }
}
