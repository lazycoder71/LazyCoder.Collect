using DG.Tweening;
using LazyCoder.Core;
using LazyCoder.Pool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LazyCoder.Collect
{
    public class CollectGroup : MonoBase
    {
        public CollectConfig Config { get; private set; }

        public CollectDestination Destination { get; private set; }

        public CollectContext Context { get; private set; }

        private Vector3 _destinationPosition;

        /// <summary>
        /// Where items return to. Keeps the last known spot once the destination is gone (page closed, scene unloading).
        /// </summary>
        public Vector3 DestinationPosition
        {
            get
            {
                if (Destination != null)
                    _destinationPosition = Destination.Position;

                return _destinationPosition;
            }
        }

        private Tween _tween;

        private Action _onComplete;

        private int _remainCount;

        private void OnDestroy()
        {
            _tween?.Kill();
        }

        public void Construct(CollectConfig config, CollectContext context, CollectDestination destination,
            Action onComplete)
        {
            Config = config;
            Context = context;
            Destination = destination;

            _onComplete = onComplete;

            _destinationPosition = destination.Position;

            int spawnCount = context.SpawnCount;

            _remainCount = spawnCount;

            // Notify destination about the return begin
            destination.CollectBegin(context);

            if (spawnCount <= 0)
            {
                Destruct();

                return;
            }

            float delayBetween = spawnCount > 1 ? config.SpawnDuration / (spawnCount - 1) : 0.0f;

            // Construct spawn sequence
            Sequence sequence = DOTween.Sequence();

            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 spawnPosition = GetSpawnPosition(config.SpawnPositions, i);

                sequence.AppendCallback(() => { Spawn(spawnPosition); });

                if (i < spawnCount - 1)
                    sequence.AppendInterval(delayBetween);
            }

            sequence.Play();

            _tween?.Kill();
            _tween = sequence;
        }

        /// <summary>
        /// Called by an item once it is back in the pool.
        /// </summary>
        public void ItemCollected()
        {
            // The destination can be gone before the last item lands (page closed, scene unloading)
            if (Destination != null)
                Destination.Collect(Context);

            _remainCount--;

            if (_remainCount <= 0)
                Destruct();
        }

        private static Vector3 GetSpawnPosition(List<Vector3> positions, int index)
        {
            // A config with no sample generated yet spawns everything at the center
            if (positions == null || positions.Count == 0)
                return Vector3.zero;

            return positions.GetLoop(index);
        }

        private void Destruct()
        {
            if (Destination != null)
                Destination.CollectEnd(Context);

            _onComplete?.Invoke();

            Destroy(GameObjectCached);
        }

        private void Spawn(Vector3 spawnPosition)
        {
            CollectItem collectObj = PoolPrefabShared.Get(Config.SpawnPrefab, TransformCached)
                .GetComponent<CollectItem>();

            collectObj.TransformCached.localPosition = spawnPosition;
            collectObj.TransformCached.localScale = Vector3.one;

            collectObj.Construct(this);
        }
    }
}
