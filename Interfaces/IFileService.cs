using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadialMaui.Interfaces
{
    public interface IFileService
    {
        public Task<string> HandleDownload(HttpResponseMessage response);
        public Task ConvertFile(string endpoint, HttpClient client);
    }
}
