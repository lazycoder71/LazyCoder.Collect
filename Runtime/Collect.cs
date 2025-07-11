using DG.Tweening;
using LazyCoder.Core;
using LazyCoder.Pool;
using System;
using UnityEngine;

namespace LazyCoder.Collect
{
    public class Collect : MonoBase
    {
        private CollectConfig _config;

        private CollectDestination _destination;

        private Tween _tween;

        private Action _onComplete;

        private void OnDestroy()
        {
            _tween?.Kill();
        }

        public void Construct(CollectConfig config, CollectDestination destination, int valueCount, int spawnCount, Action onComplete)
        {
            _config = config;
            _destination = destination;
            _onComplete = onComplete;

            float delayBetween = spawnCount > 1 ? config.spawnDuration / (spawnCount - 1) : 0.0f;

            // Construct spawn sequence
            Sequence sequence = DOTween.Sequence();

            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 spawnPosition = config.spawnPositions.GetLoop(i);

                sequence.AppendCallback(() => { Spawn(spawnPosition); });
                sequence.AppendInterval(delayBetween);
            }

            sequence.Play();
            sequence.OnComplete(StartCheckEmptyLoop);

            _tween?.Kill();
            _tween = sequence;

            // Notify destination about the return begin
            _destination.CollectBegin(valueCount, spawnCount);
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
            _destination.ReturnEnd();

            _onComplete?.Invoke();

            Destroy(GameObjectCached);
        }

        private void Spawn(Vector3 spawnPosition)
        {
            CollectObject obj = PoolPrefabShared.Get(_config.spawnPrefab, TransformCached).GetComponent<CollectObject>();

            obj.TransformCached.localPosition = spawnPosition;
            obj.TransformCached.localScale = Vector3.one;

            obj.Construct(_config, _destination);
        }
    }
}
