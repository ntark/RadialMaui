using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RadialMaui.Util
{
    public class APIUtil
    {
        public static string EncodeQueryParams(Dictionary<string, string> queryParams)
        {
            var encodedValues = queryParams
                .Select(p => string.Format("{0}={1}", p.Key, HttpUtility.UrlEncode(p.Value)))
                .ToList();

            var qeuryParamsString = string.Join("&", encodedValues);

            return qeuryParamsString;
        }
    }
}
