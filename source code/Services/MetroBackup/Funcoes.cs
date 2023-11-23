using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace MetroBackup
{
    public static class Funcoes
    {
        public static string RemoveWhitespace(this string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }
        public static string RemoveCaracteresEspeciais(this string _texto)
        {
            if (_texto == null)
                return null;

            _texto = Regex.Replace(_texto, @"[^\w\.\/\@\- ]|[_]&*'", "");
            _texto = _texto.Normalize(NormalizationForm.FormD);
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < _texto.Length; i++)
            {
                Char c = _texto[i];
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    stringBuilder.Append(c);
            }
            return stringBuilder.ToString();
        }
    }
}
