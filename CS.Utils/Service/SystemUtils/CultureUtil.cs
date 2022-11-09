using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace ArsuLeo.CS.Utils.Service.SystemUtils
{
    public static class CultureUtil
    {
        public static CultureInfo GetCultureFromTwoLetterCountryCode(string twoLetterISOCountryCode, CultureInfo defaultCulture)
        {
            try
            {
                IEnumerable<CultureInfo> matchingCultures = CultureInfo.GetCultures(CultureTypes.AllCultures & ~CultureTypes.NeutralCultures)
                          .Where(m => m.Name.EndsWith("-" + twoLetterISOCountryCode));
                if(matchingCultures.Count() == 0)
                {
                    return defaultCulture;
                }
                return matchingCultures.First();
            }
            catch
            {
                return defaultCulture;
            }
        }
    }
}
