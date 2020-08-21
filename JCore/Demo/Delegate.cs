using System;

namespace JCore.Demo
{
    public static class DelegateDemo
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
            _FactoryAction(name, provider);
            _FactoryAction = (value1, value2) =>
            {
                Console.WriteLine(value1 + "sfsafdaf" + value2);
            };


            _FactoryAction(name, provider);

        }

    }
}
