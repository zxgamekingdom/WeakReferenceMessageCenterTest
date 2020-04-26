using System;

namespace WeakReferenceMessageCenterTest.弱引用委托
{
    public static class ConsoleExtensions
    {
        public static void WriteLine(this object o)
        {
            Console.WriteLine(o);
        }
    }
}