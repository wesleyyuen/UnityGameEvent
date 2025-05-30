using UnityEngine;
using UnityEngine.Events;

namespace GameEvents
{
    public abstract class GenericGameEventListener<T> : MonoBehaviour
    {
        [SerializeField] private GenericGameEvent<T> _gameEvent;
        [SerializeField] private bool _useFilter;
        [SerializeField] private T[] _filterValues;

        public UnityEvent<T> OnGameEventInvoked;

        private void OnEnable()
        {
            _gameEvent.OnInvoked.AddListener(GameEventInvoked);
        }

        private void OnDisable()
        {
            _gameEvent.OnInvoked.RemoveListener(GameEventInvoked);
        }

        private void GameEventInvoked(T param)
        {
            if (_useFilter && !TestFilter(param)) return;
            OnGameEventInvoked.Invoke(param);
        }

        private bool TestFilter(T param)
        {
            foreach (T filterValue in _filterValues)
            {
                if (param.Equals(filterValue)) return true;
            }

            return false;
        }
    }
}