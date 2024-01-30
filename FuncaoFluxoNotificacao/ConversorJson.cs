using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuncaoFluxoNotificacao
{
    public static class ConversorJson
    {
        public static T ConvertaJson<T>(this string json)
        {
            var jsonSettings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var objeto = JsonConvert.DeserializeObject<T>(json, jsonSettings);
            return objeto;
        }
    }
}
