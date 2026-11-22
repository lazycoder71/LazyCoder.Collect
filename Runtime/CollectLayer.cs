using System.Collections.Generic;
using LazyCoder.Core;
using UnityEngine;

namespace LazyCoder.Collect
{
    /// <summary>
    /// Optional parent for every spawned collect group, typically a nested canvas sorted above all pages.
    /// Without one, a group goes under its destination's parent and is drawn, and destroyed, with it.
    /// Items fly in world space, so the layer must share the destination's root canvas (same camera).
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public class CollectLayer : MonoBase
    {
        // The most recently enabled layer wins, the same rule as destinations
        private static readonly List<CollectLayer> s_layers = new();

        /// <summary>
        /// Null while no layer is enabled.
        /// </summary>
        public static Transform Current => s_layers.Count > 0 ? s_layers[^1].TransformCached : null;

        protected override void OnEnable()
        {
            base.OnEnable();

            s_layers.Add(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            s_layers.Remove(this);
        }
    }
}
