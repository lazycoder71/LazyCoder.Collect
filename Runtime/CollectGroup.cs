using DG.Tweening;
using LazyCoder.Core;
using LazyCoder.Pool;
using System;
using UnityEngine;

namespace LazyCoder.Collect
{
    public class CollectGroup : MonoBase
    {
        public CollectConfig Config { get; private set; }

        public CollectDestination Destination { get; private set; }

        public CollectContext Context { get; private set; }

        private Tween _tween;

        private Action _onComplete;

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

            int spawnCount = context.SpawnCount;

            float delayBetween = spawnCount > 1 ? config.SpawnDuration / (spawnCount - 1) : 0.0f;

            // Construct spawn sequence
            Sequence sequence = DOTween.Sequence();

            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 spawnPosition = config.SpawnPositions.GetLoop(i);

                sequence.AppendCallback(() => { Spawn(spawnPosition); });
                sequence.AppendInterval(delayBetween);
            }

            sequence.Play();
            sequence.OnComplete(StartCheckEmptyLoop);

            _tween?.Kill();
            _tween = sequence;

            // Notify destination about the return begin
            destination.CollectBegin(context);
        }

        private void StartCheckEmptyLoop()
        {
            _tween?.Kill();
            _tween = DOVirtual.DelayedCall(1.0f, null, false)
                .SetLoops(-1, LoopType.Restart)
                .OnUpdate(CheckEmpty);
        }

        private void CheckEmpty()
        {
            if (TransformCached.childCount == 0)
                Destruct();
        }

        private void Destruct()
        {
            Destination.CollectEnd();

            _onComplete?.Invoke();

            Destroy(GameObjectCached);
        }

        private void Spawn(Vector3 spawnPosition)
        {
            CollectGroupItem collectObj = PoolPrefabShared.Get(Config.SpawnPrefab, TransformCached)
                .GetComponent<CollectGroupItem>();

            collectObj.TransformCached.localPosition = spawnPosition;
            collectObj.TransformCached.localScale = Vector3.one;

            collectObj.Construct(this);
        }
    }
}