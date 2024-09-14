using RadialMaui.Interfaces;
using RadialMaui.Platforms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadialMaui.Platforms
{
    public class FileSaveService : IFileSaveService
    {
        public string GetDownloadPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        }
    }
}
