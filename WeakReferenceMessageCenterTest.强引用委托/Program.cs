using System;

namespace WeakReferenceMessageCenterTest.强引用委托
{
    class Program
    {
        static NewClass _newClass = new NewClass("全局变量");

        static void Main(string[] args)
        {
            var newClass = new NewClass("非全局,但生命周期长于Action的变量");
            NewMethod(newClass);
            GC.Collect();
            new string('-', 88).WriteLine();
            MessageCenter.Send("a");
            Console.ReadKey();
        }

        private static void NewMethod(NewClass newClass)
        {
            var a = new NewClass("临时变量");
            a.Sub("a", () => a.WriteLine());
            _newClass.Sub("a", () => _newClass.WriteLine());
            newClass.Sub("a", () => newClass.WriteLine());
            MessageCenter.Send("a");
        }
    }
}