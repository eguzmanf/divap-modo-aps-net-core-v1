using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotacionWEBCore.Helpers.String.StringCase
{
    public class StringCase
    {
        public string StringCaseFormat(string str)
        {
            var frase = str.Trim();
            System.Globalization.TextInfo textInfo = new
            System.Globalization.CultureInfo("es-CL", false).TextInfo;
            frase = textInfo.ToTitleCase(frase);

            return frase;
        }
    }
}
