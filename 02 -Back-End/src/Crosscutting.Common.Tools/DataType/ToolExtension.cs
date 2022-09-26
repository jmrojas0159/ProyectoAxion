using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using WebMarkupMin.Core;

namespace Crosscutting.Common.Tools.DataType
{
    public static class ToolExtension
    {
        private static readonly char[] BaseDictionary = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();

        public static IEnumerable<T> ToIEnumerable<T>(this IEnumerator<T> enumerator)
        {
            while (enumerator.MoveNext())
            {
                yield return enumerator.Current;
            }
        }
        public static string GenSemiUniqueId(int lenght = 6)
        {
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                var str = new StringBuilder(lenght);
                for (var i = 0; i < lenght; i++)
                {
                    var bytes = new byte[8];
                    rngCsp.GetBytes(bytes);
                    var seed = BitConverter.ToInt32(bytes, 0);
                    str.Append(BaseDictionary[new Random(seed).Next(0, 35)]);
                }
                return str.ToString();
            }
        }

        public static string Minify(this string htmlInput)
        {
            var htmlMinifier = new HtmlMinifier();

            MarkupMinificationResult result = htmlMinifier.Minify(htmlInput, generateStatistics: true);
            if (result.Errors.Count == 0)
            {
                return result.MinifiedContent;
            }

            IList<MinificationErrorInfo> errors = result.Errors;

            Console.WriteLine("Found {0:N0} error(s):", errors.Count);
            Console.WriteLine();

            foreach (var error in errors)
            {
                Console.WriteLine("Line {0}, Column {1}: {2}",
                    error.LineNumber, error.ColumnNumber, error.Message);
                Console.WriteLine();
            }

            return htmlInput;
        }

    }
}