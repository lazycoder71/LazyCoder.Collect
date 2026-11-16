using DG.Tweening;
using LazyCoder.Core;
using LazyCoder.Pool;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace LazyCoder.Collect
{
    public class CollectItem : MonoBase
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

        /// <summary>
        /// Where the item rests once it has spread out, taken before any step is built.
        /// Steps must use this instead of the live position: a tween given a start value
        /// rewinds on creation and moves the item while the sequence is still being built.
        /// </summary>
        public Vector3 SpawnPosition { get; private set; }

        private void OnDestroy()
        {
            Sequence?.Kill();
        }

        public virtual void Construct(CollectGroup group)
        {
            _group = group;

            SpawnPosition = TransformCached.position;

            Sequence?.Kill();
            Sequence = DOTween.Sequence();

            foreach (var step in _group.Config.Steps)
            {
                step.Apply(this);
            }

            Sequence.OnComplete(Sequence_OnComplete);

            _onSpawn?.Invoke();
        }

        private void Sequence_OnComplete()
        {
            // Raised while the item is still active, listeners on it would not run once it is pooled
            _onCollected?.Invoke();

            PoolPrefabShared.Release(GameObjectCached);

            // The pool does not reparent: leave the group, it destroys itself after its last item
            TransformCached.SetParent(null, false);

            _group.ItemCollected();
        }
    }
}