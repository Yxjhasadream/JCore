using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;

namespace ConsoleApp1
{
    class Program
    {
        private static readonly Regex FavoriteItemNamePattern
            = new Regex(@"分类(?<num>\d{1,2})(?<sub>\.\d+)?_(?<name>.+)", RegexOptions.Compiled);

        private static readonly Regex NEWFavoriteItemNamePattern
            = new Regex(@"(?<adChannel>\w+)_分类(?<num>\d{1,2})(?<sub>\.\d+)?_(?<name>.+)", RegexOptions.Compiled);

        private static readonly Regex CountPattern =
            new Regex(@"(?<name>\w+)\s+\w*?(?<score>\d+)分");
        static void Main(string[] args)
        {
            var i = 2;
            var b = 4;
            var r = i / b;
            var rr = Convert.ToDecimal((decimal)i / b);
            var rrr = (decimal)i / b;

            var tt = Convert.ToDecimal(i * 0.01);
            return;


            var path = "C:\\Users\\admin\\Desktop\\新建文件夹 (3)\\V.174版本500分.txt";
            var text = File.ReadAllText(path);

            var match1 = CountPattern.Matches(text);
            var match2 = CountPattern.Match(text);
            var result1 = new List<Tuple<string, int>>();
            var result2 = new List<Tuple<string, int>>();
            while (match2.Success)
            {
                var tuple = new Tuple<string, int>(match2.Groups["name"].ToString(), Convert.ToInt32(match2.Groups["score"].ToString()));
                result1.Add(tuple);
                match2 = match2.NextMatch();
            }

            foreach (Match item in match1)
            {
                var tuple = new Tuple<string, int>(item.Groups["name"].ToString(), Convert.ToInt32(item.Groups["score"].ToString()));
                result2.Add(tuple);
            }

            var sort1 = result1.OrderBy(x => x.Item2);
            var sort2 = result2.OrderBy(x => x.Item2);

            var sb = "";
            foreach (var tuple in sort1)
            {
                sb += tuple.Item1 + " " + tuple.Item2 + "\r\n";
            }

            Console.WriteLine(sb);
        }
    }
}
