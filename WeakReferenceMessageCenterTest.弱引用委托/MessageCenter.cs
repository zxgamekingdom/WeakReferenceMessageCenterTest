using System;
using System.Collections.Generic;
using System.Linq;

namespace WeakReferenceMessageCenterTest.弱引用委托
{
    public static class MessageCenter
    {
        private static readonly
            Dictionary<string,
                List<(WeakReference subObj, WeakReference<Action> action)>>
            _dictionary =
                new Dictionary<string,
                    List<(WeakReference subObj, WeakReference<Action> action)>>();

        public static void Sub<T>(T subObj, string message, Action action)
        {
            lock (_dictionary)
            {
                if (_dictionary.ContainsKey(message))
                {
                    _dictionary[message]
                        .Add((new WeakReference(subObj),
                            new WeakReference<Action>(action)));
                }
                else
                {
                    var valueTuples =
                        new List<(WeakReference subObj, WeakReference<Action> action)
                        >
                        {
                            (new WeakReference(subObj),
                                new WeakReference<Action>(action))
                        };
                    _dictionary.Add(message,
                        valueTuples);
                }
            }
        }

        public static void Send(string message)
        {
            lock (_dictionary)
            {
                if (_dictionary.ContainsKey(message))
                {
                    foreach ((WeakReference weakReference,
                        WeakReference<Action> action) in _dictionary
                            [message]
                        .Where(tuple => tuple.subObj.Target != null))
                    {
                        Action target;
                        action.TryGetTarget(out target);
                        target.Invoke();
                    }
                }
            }
        }
    }
}