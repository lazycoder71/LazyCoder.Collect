using LazyCoder.Core;
using System;
using UnityEngine;

namespace LazyCoder.Collect
{
    public static class CollectHelper
    {
        public static void Spawn(CollectConfig config, Vector3 spawnPosition, int valueCount, int spawnCount, Action onComplete = null)
        {
            // Find destination
            CollectDestination destination = CollectDestinationManager.Get(config);

            // Check destination exist
            if (destination == null)
            {
                LDebug.Log(typeof(CollectHelper), $"Can't find destination for {config.name}");

                onComplete?.Invoke();

                return;
            }

            // Spawn collect object
            GameObject objCollect = new GameObject(config.name, typeof(RectTransform));

            // Setup collect object
            Collect collect = objCollect.AddComponent<Collect>();

            collect.TransformCached.SetParent(destination.TransformCached.parent, false);
            collect.TransformCached.SetAsLastSibling();
            collect.TransformCached.position = spawnPosition;

            collect.Construct(config, destination, valueCount, spawnCount, onComplete);
        }
    }
}