using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GameEvents
{
    [CreateAssetMenu(menuName = "Game Events/Void Game Event")]
    public class GameEvent : ScriptableObject
    {
        [SerializeField] private bool _autoCleanupListeners = true;
        
        public UnityEvent OnInvoked;
        
        private List<UnityAction> _listeners = new List<UnityAction>();

        public void Invoke()
        {
            OnInvoked.Invoke();
        }

        public void AddListener(UnityAction listener)
        {
            OnInvoked.AddListener(listener);
            _listeners.Add(listener);

            if (_autoCleanupListeners)
            {
                CleanupListeners();
            }
        }

        public void RemoveListener(UnityAction listener)
        {
            OnInvoked.RemoveListener(listener);
            
            if (_listeners.Contains(listener))
            {
                _listeners.Remove(listener);
            }
        }

        private void CleanupListeners()
        {
            List<UnityAction> toRemove = new List<UnityAction>();
            foreach (UnityAction listener in _listeners)
            {
                if (listener == null || listener.Target.Equals(null))
                {
                    toRemove.Add(listener);
                }
            }

            foreach (UnityAction listener in toRemove)
            {
                _listeners.Remove(listener);
            }
        }
    }
}