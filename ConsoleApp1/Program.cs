using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

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
