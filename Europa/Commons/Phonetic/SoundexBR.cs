using System.Text;
using System.Text.RegularExpressions;

namespace Europa.Commons.Phonetic
{
    public static class SoundexBR
    {
        // -> Obtem o soudex de uma string
        private static string GetSoundex(string value)
        {
            value = value.ToUpper(); // -> Normaliza letras em Maiuscula
            var result = new StringBuilder();

            foreach (char item in value)
                if (char.IsLetter(item)) // -> Limpa String
                    AddCharacter(result, item); // -> Adiciona um char Soudex

            // -> Limpa as exceções geradas
            result.Replace(".", string.Empty);
            // -> Arruma o tamanho no padrão soudex 
            //      4 caracteres.
            FixLength(result);

            return result.ToString();
        }

        // -> Preenche com 0 nos casos menor q quatro,
        //      ou pega os 4 primeiros caracteres.
        private static void FixLength(StringBuilder result)
        {
            var length = result.Length;

            if (length < 4)
                result.Append(new string('0', 4 - length));
            else
                result.Length = 4;
        }

        // -> Adiciona o caractere soudex, cuidando para não repetir.
        private static void AddCharacter(StringBuilder result, char item)
        {
            if (result.Length == 0)
            {
                result.Append(item);
            }
            else
            {
                var code = GetSoundexDigit(item);

                if (code != result[result.Length - 1].ToString())
                    result.Append(code);
            }
        }

        // -> Algoritmo de substituição padrão Soudex.
        private static string GetSoundexDigit(char item)
        {
            var charString = item.ToString();

            if ("BFPV".Contains(charString))
                return "1";
            else if ("CGJKQSXZ".Contains(charString))
                return "2";
            else if ("DT".Contains(charString))
                return "3";
            else if ("L".Contains(charString))
                return "4";
            else if ("MN".Contains(charString))
                return "5";
            else if ("R".Contains(charString))
                return "6";
            else
                return ".";
        }

        // -> Recebe várias letras, separadas por virgula, para serem substituídas por uma letra específica.
        private static string ReplaceMultiplo(string value, string from, string to)
        {
            var fromPattern = $"({string.Join(")|(", @from.Split(','))})".Replace(".", @"\.");
            return Regex.Replace(value, fromPattern, to);
        }

        public static string BuildKey(string value)
        {
            value = value.ToUpper();
            value = ReplaceMultiplo(value, "Á,À,Ã,Â,Ä", "A");
            value = ReplaceMultiplo(value, "É,È,Ê,Ë", "E");
            value = ReplaceMultiplo(value, "Í,Ì,Î,Ï", "I");
            value = ReplaceMultiplo(value, "Ó,Ò,Ö,Õ,Ô", "O");
            value = ReplaceMultiplo(value, "Ú,Ù,Û,Ü", "U");
            value = ReplaceMultiplo(value, ".,-,", string.Empty);
            value = ReplaceMultiplo(value, "0,1,2,3,4,5,6,7,8,9,0", string.Empty);
            value = ReplaceMultiplo(value, "Y", "I");
            value = ReplaceMultiplo(value, "PH", "F");
            value = ReplaceMultiplo(value, "GE", "JE");
            value = ReplaceMultiplo(value, "GI", "JI");
            value = ReplaceMultiplo(value, "CA", "KA");
            value = ReplaceMultiplo(value, "CE", "SE");
            value = ReplaceMultiplo(value, "CI", "SI");
            value = ReplaceMultiplo(value, "CO", "KO");
            value = ReplaceMultiplo(value, "CU", "KU");
            value = ReplaceMultiplo(value, "Ç", "S");
            value = ReplaceMultiplo(value, "WAS", "WS");
            value = ReplaceMultiplo(value, "WA", "VA");
            value = ReplaceMultiplo(value, "WO", "VO");
            value = ReplaceMultiplo(value, "WU", "VU");
            value = ReplaceMultiplo(value, "WI", "UI");

            // -> Depois de tratar a lingua portuguesa
            //      Geramos o soudex padrão.
            value = GetSoundex(value);
            return value;
        }
    }
}
