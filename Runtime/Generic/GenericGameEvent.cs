using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents
{
    public abstract class GenericGameEvent<T> : ScriptableObject
    {
        [SerializeField] private bool _autoCleanupListeners = true;
        
        public UnityEvent<T> OnInvoked;
        
        private List<UnityAction<T>> _listeners = new List<UnityAction<T>>();

        public void Invoke(T param)
        {
            OnInvoked.Invoke(param);
        }

        public void AddListener(UnityAction<T> listener)
        {
            OnInvoked.AddListener(listener);
            _listeners.Add(listener);

            if (_autoCleanupListeners)
            {
                CleanupListeners();
            }
        }

        public void RemoveListener(UnityAction<T> listener)
        {
            OnInvoked.RemoveListener(listener);
            
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
        }

        private void CleanupListeners()
        {
            List<UnityAction<T>> toRemove = new List<UnityAction<T>>();
            foreach (UnityAction<T> listener in _listeners)
            {
                if (listener == null || listener.Target.Equals(null))
                {
                    toRemove.Add(listener);
                }
            }

            foreach (UnityAction<T> listener in toRemove)
            {
                _listeners.Remove(listener);
            }
        }
    }
}
