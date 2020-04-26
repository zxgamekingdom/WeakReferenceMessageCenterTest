using System;

namespace WeakReferenceMessageCenterTest.弱引用委托
{
    public static class MessageCenterExtensions
    {
        public static void Sub<T>(this T subObj, string message, Action action)
        {
            MessageCenter.Sub(subObj, message, action);
        }
    }
}