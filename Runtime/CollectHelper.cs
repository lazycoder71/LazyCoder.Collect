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
                LzDebug.Log(typeof(CollectHelper), $"Can't find destination for {config.name}");

                onComplete?.Invoke();

                return;
            }

            // Spawn the Collect object
            GameObject objCollect = new GameObject(config.name, typeof(RectTransform));

            // Set up the Collect object
            CollectGroup collectGroup = objCollect.AddComponent<CollectGroup>();

            // A layer keeps the group alive and on top when the destination or the spawn source sits under a page
            Transform parent = CollectLayer.Current;

            if (parent == null)
                parent = destination.TransformCached.parent;

            collectGroup.TransformCached.SetParent(parent, false);
            collectGroup.TransformCached.SetAsLastSibling();
            collectGroup.TransformCached.position = spawnPosition;

            collectGroup.Construct(config, context, destination, onComplete);
        }
    }
}