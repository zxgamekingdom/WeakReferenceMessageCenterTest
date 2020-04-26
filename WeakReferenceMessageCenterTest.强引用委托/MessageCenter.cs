using System;
using System.Collections.Generic;
using System.Linq;

namespace WeakReferenceMessageCenterTest.强引用委托
{
    public static class MessageCenter
    {
        private static readonly
            Dictionary<string, List<(WeakReference subObj, Action action)>>
            _dictionary =
                new Dictionary<string, List<(WeakReference subObj, Action action)>>();

        public static void Sub<T>(T subObj, string message, Action action)
        {
            lock (_dictionary)
            {
                if (_dictionary.ContainsKey(message))
                {
                    _dictionary[message].Add((new WeakReference(subObj), action));
                }
                else
                {
                    _dictionary.Add(message,
                        new List<(WeakReference, Action)>()
                            {(new WeakReference(subObj), action)});
                }
            }
        }

        public static void Send(string message)
        {
            lock (_dictionary)
            {
                if (_dictionary.ContainsKey(message))
                {
                    foreach ((WeakReference weakReference, Action action) in _dictionary
                            [message]
                        .Where(tuple => tuple.subObj.Target != null))
                    {
                        action.Invoke();
                    }
                }
            }
        }
    }
}