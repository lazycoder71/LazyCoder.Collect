using DG.Tweening;
using LazyCoder.Core;
using LazyCoder.Pool;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace LazyCoder.Collect
{
    public class CollectGroupItem : MonoBase
    {
        [Title("Event")]
        [SerializeField] private UnityEvent _onSpawn;

        [SerializeField] private UnityEvent _onCollected;

        private RectTransform _rectTransform;

        private CollectGroup _group;

        public Sequence Sequence { get; private set; }

        public RectTransform RectTransform
        {
            get
            {
                _rectTransform ??= GetComponent<RectTransform>();

                return _rectTransform;
            }
        }
        
        public CollectDestination Destination => _group.Destination;

        private void OnDestroy()
        {
            Sequence?.Kill();
        }

        public virtual void Construct(CollectGroup group)
        {
            _group = group;

            Sequence?.Kill();
            Sequence = DOTween.Sequence();

            for (int i = 0; i < _group.Config.Steps.Length; i++)
            {
                _group.Config.Steps[i].Apply(this);
            }

            Sequence.OnComplete(Sequence_OnComplete);

            _onSpawn?.Invoke();
        }

        private void Sequence_OnComplete()
        {
            _group.Destination.Collect();

            TransformCached.SetParent(null);
            
            PoolPrefabShared.Release(GameObjectCached);

            _onCollected?.Invoke();
        }
    }
}