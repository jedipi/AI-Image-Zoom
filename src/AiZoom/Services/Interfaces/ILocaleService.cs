using AiZoom.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiZoom.Services.Interfaces
{
    public interface ILocaleService
    {
        List<LocaleModel> GetLocale();
    }
}
