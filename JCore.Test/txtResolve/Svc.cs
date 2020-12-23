using System;
using System.Collections.Generic;
using System.Data;
using NUnit.Framework;

namespace JCore.Tests.txtResolve
{
    [TestFixture]
    public class Svc
    {
        private string[] _lines; //文件中的每一行
        private string[][] _results; //文件中每一行的解析结果
        private string _multiLineData; //一整个文件的数据

        /// <summary>
        /// 将svc文本解析为datatable。
        /// </summary>
        [Test]
        public void SvcTest()
        {
            string line1 = "\"1\",2,,";
            string line2 = "1,\"2\r3\",\",\"";
            string line3 = "";
            string line4 = "1";
            string line5 = ",,";

            string[] res1 = { "1", "2", "", "" };
            string[] res2 = { "1", "2\r3" };
            string[] res3 = { "" };
            string[] res4 = { "1" };
            string[] res5 = { "", "", "" };

            _lines = new[] { line1, line2, line3, line4, line5 };
            _results = new[] { res1, res2, res3, res4, res5 };
            _multiLineData = line1 + "\r\n" + line2 + "\r" + line3 + "\r" + line4 + "\n" + line5;

            // read one 元字符有 双引号"" 逗号 ,换行符。
            // 直接把所有的文本弄成textReader ,通过readLine来处理每一行。
            // 读到单行，foreach line character如果是逗号，则append到一个column。要先准备一个list。因为会有多行，可能出现第三行5个元素第一行2个元素的情况。

            var dt = GetDataTableFromSvc(_multiLineData);
            Assert.AreEqual(dt.Rows.Count, 3);
            Assert.AreEqual(dt.Columns.Count, 3);
        }

        public int[] TwoSum(int[] nums, int target)
        {
            var res = new int[2];
            var pre = 0;
            var current = 0;
            for (int i = 0; i < nums.Length; i++)
            {
                var temp = nums[i];
                if (target < temp)
                {
                    continue;
                }

                if (current > 0)
                {
                    pre = current;
                    current = temp;
                }
            }

            throw new NotImplementedException();
        }

        private static DataTable GetDataTableFromSvc(string text)
        {
            var res = GetListFromSvc(text);
            var table = new DataTable();
            var maxCol = 0;
            for (int i = 0; i < res.Count; i++)
            {
                var len = res[i].Count;
                maxCol = maxCol > len ? maxCol : len;
            }

            for (int i = 0; i < maxCol; i++)
            {
                table.Columns.Add("column" + i, typeof(string));
            }

            foreach (var item in res)
            {
                var row = table.NewRow();
                for (int i = 0; i < item.Count; i++)
                {
                    row[i] = item[i];
                }
                table.Rows.Add(row);
            }

            return table;
        }

        private static List<List<string>> GetListFromSvc(string text)
        {
            var token = Token.None;
            var count = text.Length;
            var table = new List<List<string>>();
            var row = new List<string>();
            var value = "";
            for (int i = 0; i < count; i++)
            {
                var character = text[i];
                switch (character)
                {
                    case '"': // 引号引起来的内容都视为一个部分的内容。在引号内，两个引号视为转译为一个引号。
                        if (token == Token.Quotes)
                        {
                            if (text[i + 1] == character)
                            {
                                value += character;
                            }
                            else // 如果不是引号开始，则视为引号结束。
                            {
                                token = Token.None;
                            }
                        }
                        else
                        {
                            token = Token.Quotes;
                        }
                        break;
                    // \r 和 \n单独存在的情况都视为是一个换行。
                    case '\r':// 如果\r后面紧跟着一个\n 就把索引往后移一位，也视为一个换行处理。
                        if (token == Token.Quotes)
                        {
                            value += character;
                            break;
                        }

                        if (text[i + 1] == '\n')
                        {
                            i++;
                        }

                        if (!string.IsNullOrWhiteSpace(value))// 有时换行之前没有把value添加。这里要做一个检查
                        {
                            row.Add(value);
                            value = "";
                        }

                        if (row.Count > 0)
                        {
                            table.Add(row);
                            row = new List<string>();
                        }
                        break;
                    case '\n':
                        if (token == Token.Quotes)
                        {
                            value += character;
                            break;
                        }

                        if (!string.IsNullOrWhiteSpace(value))// 有时换行之前没有把value添加。这里要做一个检查
                        {
                            row.Add(value);
                            value = "";
                        }

                        if (row.Count > 0)
                        {
                            table.Add(row);
                            row = new List<string>();
                        }
                        break;
                    case ',': // 逗号代表了分割，即一个逗号视为一个cell。
                        if (token == Token.Quotes)
                        {
                            value += character;
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(value))
                            {
                                row.Add(value);
                                value = "";
                            }
                        }
                        break;
                    default:
                        value += character;
                        break;
                }
            }

            return table;
        }

        private enum Token
        {
            None,

            Quotes,
        }
    }
}
