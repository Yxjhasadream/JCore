using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Demo();
            return;
            DemonstrateMergeTable();

            return;
            var beginTime = DateTime.Today.AddHours(8).AddMinutes(33);
            var endTime = DateTime.Today.AddHours(22).AddMinutes(40);
            var param = new
            {
                a = 1,
                b = "2"
            };
            var paramType = param.GetType();
            var m1 = paramType.GetFields(BindingFlags.Default | BindingFlags.Public);
            var m2 = m1.Cast<MemberInfo>();
            var members = m2.ToDictionary(x => x.Name, x => x);
            var props = paramType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach (var prop in props)
            {
                members.Add(prop.Name, prop);
            }

            foreach (var member in members)
            {
                var prp = member.Value as PropertyInfo;

            }

            return;
            var queue = new Queue<int>();
            for (int i = 0; i < 2; i++)
            {
                queue.Enqueue(i);
            }

            for (int i = 0; i < 10; i++)
            {
                var id = queue.Dequeue();
            }


            return;

            Delegate.Run();
        }

        private static void Demo()
        {
            var balance = 10 / 10;
            var a = (decimal)balance / 10;
            Console.WriteLine(a);
            return;
            StrMode("1111");
            Console.WriteLine("输入5.则下面要输出朝东天窗开，外遮阳关，朝西天窗开");
            ProcessOut(5);
            Console.WriteLine("1");
            ProcessOut(1);
            Console.WriteLine("2");
            ProcessOut(2);
            Console.WriteLine("3");
            ProcessOut(3);
            Console.WriteLine("4");
            ProcessOut(4);
            Console.ReadKey();
        }

        public static void StrMode(string Out)
        {
            Out = "1111";

            foreach (var s in Out)
            {
                Console.WriteLine(s);
            }
            if (Out[0]=='1')
            {
                Console.WriteLine("开XX");
            }

        }


        public static void ProcessOut(int Out)
        {
            /*
             * 按照你那张截图给的注释。1朝西天窗开，2朝西天窗关。3。。。。。。
             *
             * 认真看了一下。这个人设计的也是有点问题。明显1和2是控制了同一个东西。结果用两个位来控制。low的一批- -。
             * 理论上应该是这样： （简单举几个，不把所有列出来了）
             *  朝东天窗   外遮阳  朝西天窗  
             *    1/0       1/0     1/0
             *  假设这人给你一个  7 => 111    那么就是这三个东西都开。
             *  假设这人给你一个  3 => 011    那么就是外遮阳和朝西天窗开。
             * 
             */

            if ((Out & 1) > 0)
            {
                朝西天窗开();
            }

            if ((Out & 2) > 0)
            {
                朝西天窗关();
            }





            else
            {
                外遮阳关();
            }

            if ((Out & 4) > 0)
            {
                朝东天窗开();
            }
            else
            {
                朝东天窗关();
            }
        }

        private static void 朝东天窗关()
        {
            Console.WriteLine("朝东天窗关");
        }

        private static void 朝东天窗开()
        {
            Console.WriteLine("朝东天窗开");
        }

        private static void 外遮阳关()
        {
            Console.WriteLine("外遮阳关");
        }

        private static void 外遮阳开()
        {
            Console.WriteLine("外遮阳开");
        }

        private static void 朝西天窗关()
        {
            Console.WriteLine("朝西天窗关");
        }

        private static void 朝西天窗开()
        {
            Console.WriteLine("朝西天窗开");
        }


        private static void DemonstrateMergeTable()
        {
            DataTable table1 = new DataTable("Items");

            DataColumn idColumn = new DataColumn("id", typeof(DateTime));
            DataColumn itemColumn = new DataColumn("item", typeof(string));
            table1.Columns.Add(idColumn);
            table1.Columns.Add(itemColumn);
            DataRow row;
            row = table1.NewRow();
            row["id"] = "2019/11/30 0:00:00";
            row["item"] = "11-30";
            table1.Rows.Add(row);

            row = table1.NewRow();
            row["id"] = "2019/12/1 0:00:00";
            row["item"] = "12-01";
            table1.Rows.Add(row);

            row = table1.NewRow();
            row["id"] = "2019/12/2 0:00:00";
            row["item"] = "12-02";
            table1.Rows.Add(row);

            row = table1.NewRow();
            row["id"] = "2019/12/3 0:00:00";
            row["item"] = "12-03";
            table1.Rows.Add(row);


            // Accept changes.
            table1.AcceptChanges();
            DataTable table2 = new DataTable("Items2");
            DataColumn idColumn2 = new DataColumn("id", typeof(DateTime));
            table2.Columns.Add(idColumn2);
            table2.Columns.Add("newColumn", typeof(String));

            row = table2.NewRow();
            row["id"] = "2019/12/1 0:00:00";
            row["newColumn"] = "new column 1";
            table2.Rows.Add(row);

            row = table2.NewRow();
            row["id"] = "2019/12/2 0:00:00";
            row["newColumn"] = "new column 2";
            table2.Rows.Add(row);

            row = table2.NewRow();
            row["id"] = "2019/12/3 0:00:00";
            row["newColumn"] = "new column 3";
            table2.Rows.Add(row);

            var dataset = new DataSet();
            dataset.Tables.Add(table2);

            var res = new DataTable("res");
            var col = table1.Columns[0];
            var axisName = col.ColumnName;
            var newCol = new DataColumn(axisName, col.DataType);

            res.Columns.Add(newCol);

            for (int i = 0; i < dataset.Tables.Count; i++)
            {
                var table = dataset.Tables[i];
                for (int j = 0; j < table.Columns.Count; j++)
                {
                    col = table.Columns[j];
                    if (res.Columns.Contains(col.ColumnName))
                    {
                        continue;
                    }

                    newCol = new DataColumn(col.ColumnName, col.DataType);
                    res.Columns.Add(newCol);
                }
            }

            res.Merge(table1); // 把旧的表直接merge进去。
                               // 先把横坐标轴转成一个字典。
            var dic = new Dictionary<string, DataRow>();
            for (int i = 0; i < res.Rows.Count; i++)
            {
                var ro = res.Rows[i];
                dic.Add(ro[axisName].ToString(), ro);
            }


            for (int i = 0; i < dataset.Tables.Count; i++)
            {
                var table = dataset.Tables[i];

                //for (int j = 0; j < res.Rows.Count; j++)
                //{
                //    var rRow = res.Rows[j];
                //    if (rRow["id"] == table.Rows[j]["id"])
                //    {
                //    }
                //}

                for (int j = 0; j < table.Rows.Count; j++)
                {
                    var trow = table.Rows[j];
                    if (dic.TryGetValue(trow[axisName].ToString(), out var r))
                    {
                        for (int k = 0; k < r.Table.Columns.Count; k++)
                        {
                            var colName = r.Table.Columns[k].ColumnName;
                        }

                    }
                }
            }



            table1.Merge(table2, false, MissingSchemaAction.Add);
            //table3.Merge(table2, true);
            //table4.Merge(table2, false);

            //table5.Merge(table2, true, MissingSchemaAction.AddWithKey);
            //table6.Merge(table2, true, MissingSchemaAction.Ignore);
            //table7.Merge(table2, false, MissingSchemaAction.Add);
            //table8.Merge(table2, false, MissingSchemaAction.Ignore);
            //table9.Merge(table2);

            PrintValues(table1, "Merged With table1, schema added");

        }

        private static void Row_Changed(object sender,
            DataRowChangeEventArgs e)
        {
            Console.WriteLine("Row changed {0}\t{1}", e.Action,
                e.Row.ItemArray[0]);
        }

        private static void PrintValues(DataTable table, string label)
        {
            // Display the values in the supplied DataTable:
            Console.WriteLine(label);
            foreach (DataRow row in table.Rows)
            {
                foreach (DataColumn col in table.Columns)
                {
                    Console.Write("\t " + row[col].ToString());
                }
                Console.WriteLine();
            }
        }
    }

    public static class Delegate
    {
        private delegate void BuyBook();

        private static void Book()
        {
            Console.WriteLine("我是提供书籍的");
        }

        private static void Book(string name)
        {
            Console.WriteLine(name);
        }

        private static void BookA(string name, string provider)
        {
            Console.WriteLine(name + provider);
        }

        private static string Book(string name, string provider)
        {
            return string.Concat(name, ",", provider);
        }

        private static readonly Func<string, string, string> _factory = Book;
        private static Func<string, string, string> _Bookfactory;
        private static Action<string> _factoryAction = Book;
        private static Action<string, string> _FactoryAction = BookA;



        public static void Run()
        {
            var name = "百年孤独";
            var provider = "新华书店";
            BuyBook buyBook = Book;
            buyBook();

            Action bookAction = Book;
            bookAction();

            Action<string> BookAction = Book;
            BookAction(name);

            Func<string, string, string> bookFunc = Book;
            Console.WriteLine(bookFunc(name, provider));
            Console.WriteLine(_factory.Invoke(name, provider));
            _Bookfactory = (value1, value2) =>
           {
               var a = "1";
               return a + value1 + value2;
           };

            _Bookfactory = _factory;
            Console.WriteLine(_Bookfactory.Invoke("!", "2"));
            _factoryAction(name);
            _FactoryAction = (value1, value2) =>
            {
                Console.WriteLine(value1 + "sfsafdaf" + value2);
            };


            _FactoryAction(name, provider);

        }

    }
}
