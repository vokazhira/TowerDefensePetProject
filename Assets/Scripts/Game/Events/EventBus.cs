using System;
using System.Collections;
using System.Collections.Generic;

namespace Game.Events
{
    public static class EventBus
    {
        private static readonly Dictionary<Type, IList> _events = new Dictionary<Type, IList>();
        private static readonly Dictionary<Type, List<Action>> _parametersLessEvents = new Dictionary<Type, List<Action>>();

        public static void Subscribe<T>(Action<T> callback)
        {
            if (callback == null) return;
            
            Type type = typeof(T);

            if (!_events.TryGetValue(type, out var list))
            {
                list = new List<Action<T>>();
                _events[type] = list;
            }

            List<Action<T>> typed = (List<Action<T>>)list;
            
            if (!typed.Contains(callback))
            {
                typed.Add(callback);
            }
        }

        public static void Subscribe<T>(Action callback)
        {
            if (callback == null) return;
            
            Type type = typeof(T);

            if (!_parametersLessEvents.TryGetValue(type, out var list))
            {
                list = new List<Action>();
                _parametersLessEvents[type] = list;
            }

            if (!list.Contains(callback))
            {
                list.Add(callback);
            }
        }

        public static void Unsubscribe<T>(Action<T> callback)
        {
            if (callback == null) return;
            
            var type = typeof(T);

            if (!_events.TryGetValue(type, out IList list)) return;
            
            ((List<Action<T>>)list).Remove(callback);

            if (list.Count == 0)
            {
                _events.Remove(type);
            }
        }
        
        public static void Unsubscribe<T>(Action callback)
        {
            if (callback == null) return;
            
            var type = typeof(T);
            
            if (!_parametersLessEvents.TryGetValue(type, out List<Action> list)) return;
            
            list.Remove(callback);

            if (list.Count == 0)
            {
                _parametersLessEvents.Remove(type);
            }
        }

        public static void Invoke<T>(T eventAction)
        {
            var type = typeof(T);

            if (_events.TryGetValue(type, out var list))
            {
                List<Action<T>> typed = (List<Action<T>>)list;
                Action<T>[] snapshot = typed.ToArray();

                foreach (Action<T> callback in snapshot)
                {
                    callback?.Invoke(eventAction);
                }
            }

            if (_parametersLessEvents.TryGetValue(type, out var parameterlessList))
            {
                Action[] snapshot = parameterlessList.ToArray();

                foreach (Action callback in snapshot)
                {
                    callback?.Invoke();
                }
            }
        }
    }
}