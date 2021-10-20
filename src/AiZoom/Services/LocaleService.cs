using AiZoom.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.IO;
using AiZoom.Services.Interfaces;

namespace AiZoom.Services
{
    public class LocaleService:ILocaleService
    {
        public List<LocaleModel> GetLocale()
        {
            var json = File.ReadAllText(@"Data\Locale.json");
            var locales = JsonSerializer.Deserialize<List<LocaleModel>>(json);
            

            return locales;
        }
    }
}
