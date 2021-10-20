using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiZoom.Models
{
    public class SettingModel
    {
        public string Locale { get; set; }
        public string Module { get; set; }
        public string FileNameSuffix { get; set; }
        public string OutputFormat { get; set; }
        public string OutputDirectory { get; set; }
    }
}
