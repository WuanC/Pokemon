using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Scripts.MyUtils
{
    public class Observer : Singleton<Observer>
    {
        private Dictionary<EventId, Action<object>> listeners = new Dictionary<EventId, Action<object>>();
        #region Register, Unregister, Broadcast
        public void Register(EventId id, Action<object> action)
        {
            if (!listeners.ContainsKey(id))
            {
                listeners[id] = action;
            }
            else
            {
                listeners[id] += action;
            }
        }

        public void Unregister(EventId id, Action<object> action)
        {
            if (listeners.ContainsKey(id))
            {
                listeners[id] -= action;
            }
        }
        public void Broadcast(EventId id, object data)
        {
            if (listeners.ContainsKey(id))
            {
                listeners[id]?.Invoke(data);
            }

        }

        #endregion
    }
}