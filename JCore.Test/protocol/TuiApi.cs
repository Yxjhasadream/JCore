using System.Collections.Generic;
using NUnit.Framework;

namespace JCore.Tests.protocol
{
    [TestFixture]
    public class TuiApi
    {
        [Test]
        public static void GetCommand()
        {
            var str = @"field1 param=field2  subfield1 """"""""   ""@no t-a-file"" ";
            var arguments = GetParam(str);

            Assert.AreEqual(arguments[0], "field1");
            Assert.AreEqual(arguments[1], "field2");
            Assert.AreEqual(arguments[2], "subfield1");
            Assert.AreEqual(arguments[3], "\"");
            Assert.AreEqual(arguments[4], " ");
            Assert.AreEqual(arguments[5], "@no t-a-file");
        }

        private class Param
        {
            public void Reset()
            {
                value = "";
            }
            public string value;
        }
        private static List<string> GetParam(string str)
        {
            char s;
            var token = Token.None;// 初始化。
            var param = new List<string>();
            var p = new Param();
            for (int i = 0; i < str.Length; i++)
            {
                s = str[i];
                switch (s)
                {
                    case '"':
                        if (token == Token.Quotes) // 之前已经有一个引号,因为tui协议采用的是索引序号，而不是key，value的方式来获取，即其没有key。直接是value1 value2的形式。
                        {
                            if (str[i + 1] == '"')// 看看后面一个字符是不是也是" 是的话，则表示两个合并展示为一个"
                            {
                                p.value += s;
                                i++;// 处理完了要把标记往后移一下。
                                break;
                            }

                            param.Add(p.value);
                            p.Reset();
                            token = Token.None;
                        }
                        else
                        {
                            token = Token.Quotes;
                        }
                        break;
                    case '=': // 遇到等号清空之前的value。
                        p.Reset();
                        break;
                    default:
                        if (char.IsWhiteSpace(s)) // 多个空白字符应当视为一个处理。
                        {
                            // 空白字符如果在引号内，视为正常值，而不是分隔符。
                            if (token == Token.Quotes)
                            {
                                p.value += s;
                                break;
                            }

                            var nextWhiteSpace = i + 1 < str.Length && char.IsWhiteSpace(str[i + 1]);
                            if (token == Token.WhiteSpace)
                            {
                                if (nextWhiteSpace) // 后面的也是空白字符，直接跳过，不是就加一个到param里面，然后reset一下。
                                {
                                    break;
                                }

                                p.value += s;
                                param.Add(p.value);
                                p.Reset();
                            }
                            else // 不在引号内，则说明是一个分隔符了。直接处理一下p。但是多个空白字符视为一个来处理。
                            {
                                if (p.value.Length > 0)
                                {
                                    param.Add(p.value);
                                    p.Reset();
                                    break;
                                }

                                if (nextWhiteSpace)
                                {
                                    token = Token.WhiteSpace;
                                    // 连续的3个以上空白字符出现，第一个视为分隔符，第二个视为普通字符。只有两个则忽略。
                                }
                            }
                        }
                        else
                        {
                            p.value += s;
                        }
                        break;
                }
            }

            return param;
        }

        private enum Token
        {
            None,
            WhiteSpace,
            Quotes
        }
    }
}
