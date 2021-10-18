using AiZoom.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AiZoom.Services
{
    public class ModuleService : IModuleService
    {
        public List<string> GetModules()
        {
            List<string> fileNames = new List<string>();
            var dir = new DirectoryInfo(@"realesrgan\models");
            var files = dir.GetFiles("*.bin");
            foreach (var f in files)
            {
                fileNames.Add(Path.GetFileNameWithoutExtension(f.Name));
            }

            return fileNames;
        }
    }
}
