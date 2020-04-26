using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace WeakReferenceMessageCenterTest.ConditionalWeakTable
{
    public static class MessageCenter
    {
        private static readonly
            Dictionary<string, ConditionalWeakTable<object, List<Action>>> _dictionary =
                new Dictionary<string, ConditionalWeakTable<object, List<Action>>>();

        public static void Sub<T>(T subObj, string message, Action action)
        {
            lock (_dictionary)
            {
                if (_dictionary.ContainsKey(message))
                {
                    ConditionalWeakTable<object, List<Action>> conditionalWeakTable =
                        _dictionary[message];
                    if (conditionalWeakTable.TryGetValue(subObj, out List<Action> list))
                    {
                        list.Add(action);
                    }
                    else
                    {
                        conditionalWeakTable.Add(subObj, new List<Action>() {action});
                    }
                }
                else
                {
                    var conditionalWeakTable =
                        new ConditionalWeakTable<object, List<Action>>()
                        {
                            {action, new List<Action>() {action}}
                        };
                    _dictionary.Add(message,
                        conditionalWeakTable);
                }
            }
        }

        public static void Send(string message)
        {
            lock (_dictionary)
            {
                if (_dictionary.ContainsKey(message))
                {
                    ConditionalWeakTable<object, List<Action>> conditionalWeakTable =
                        _dictionary[message];
                    int count = conditionalWeakTable.Count();
                    Console.Out.WriteLine("count = {0}", count);
                    foreach (KeyValuePair<object, List<Action>> keyValuePair in
                        conditionalWeakTable)
                    {
                        foreach (Action action in keyValuePair.Value)
                        {
                            action.Invoke();
                        }
                    }
                }
            }
        }
    }
}