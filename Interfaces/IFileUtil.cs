using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RadialMaui.Interfaces
{
    public interface IFileUtil
    {
        public Task<string> HandleDownload(HttpResponseMessage response);
    }
}
