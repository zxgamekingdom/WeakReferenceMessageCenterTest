using System;

namespace WeakReferenceMessageCenterTest.ConditionalWeakTable
{
    public static class MessageCenterExtensions
    {
        public static void Sub<T>(this T subObj, string message, Action action)
        {
            MessageCenter.Sub(subObj, message, action);
        }
    }
}