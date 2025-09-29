using LazyCoder.Core;
using System;
using UnityEngine;

namespace LazyCoder.Collect
{
    public static class CollectHelper
    {
        public static void Spawn(CollectConfig config, CollectContext context, Vector3 spawnPosition,
            Action onComplete = null)
        {
            // Find destination
            CollectDestination destination = CollectDestinationManager.Get(config);

            // Check destination exists
            if (destination == null)
            {
                LDebug.Log(typeof(CollectHelper), $"Can't find destination for {config.name}");

                onComplete?.Invoke();

                return;
            }

            // Spawn the Collect object
            GameObject objCollect = new GameObject(config.name, typeof(RectTransform));

            // Set up the Collect object
            CollectGroup collectGroup = objCollect.AddComponent<CollectGroup>();

            collectGroup.TransformCached.SetParent(destination.TransformCached.parent, false);
            collectGroup.TransformCached.SetAsLastSibling();
            collectGroup.TransformCached.position = spawnPosition;

            collectGroup.Construct(config, context, destination, onComplete);
        }
    }
}