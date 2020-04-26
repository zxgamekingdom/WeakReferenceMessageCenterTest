using System;

namespace WeakReferenceMessageCenterTest.强引用委托
{
    public static class MessageCenterExtensions
    {
        public static void Sub<T>(this T subObj, string message, Action action)
        {
            MessageCenter.Sub(subObj, message, action);
        }
    }
}