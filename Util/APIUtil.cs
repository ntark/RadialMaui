using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RadialMaui.Util
{
    public class APIUtil
    {
        public static string EncodeQueryParams(Dictionary<string, string?> queryParams)
        {
            var encodedValues = queryParams
                .Where(p => !string.IsNullOrEmpty(p.Key) && !string.IsNullOrEmpty(p.Value))
                .Select(p => string.Format("{0}={1}", p.Key, HttpUtility.UrlEncode(p.Value)))
                .ToList();

            var qeuryParamsString = string.Join("&", encodedValues);

            return qeuryParamsString;
        }

        public static MultipartFormDataContent MultipartFileForm(string fileName, Stream stream)
        {
            var form = new MultipartFormDataContent();

            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/octet-stream");
            form.Add(fileContent, "file", fileName);

            return form;
        }
    }
}
