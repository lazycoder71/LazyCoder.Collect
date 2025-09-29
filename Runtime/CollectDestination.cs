using LazyCoder.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LazyCoder.Collect
{
    public class CollectDestination : MonoBase
    {
        [Title("Reference")]
        [SerializeField] private Transform _target;

        [Title("Config")]
        [SerializeField] private CollectConfig _config;

        protected int ReturnExpect;

        protected int ReturnCount;

        protected float ReturnProgress => (float)ReturnCount / ReturnExpect;

        public CollectConfig Config => _config;

        public Vector3 Position => _target == null ? TransformCached.position : _target.position;

        protected override void OnEnable()
        {
            base.OnEnable();

            if (_config != null)
                CollectDestinationManager.Push(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            if (_config != null)
                CollectDestinationManager.Pop(this);
        }

        public virtual void CollectBegin(CollectContext context)
        {
            if (ReturnCount >= ReturnExpect)
            {
                ReturnExpect = 0;
                ReturnCount = 0;
            }

            ReturnExpect += context.SpawnCount;
        }

        public virtual void Collect()
        {
            ReturnCount++;
        }

        public virtual void CollectEnd()
        {
        }
    }
}