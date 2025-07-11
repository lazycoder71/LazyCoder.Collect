using DG.Tweening;
using UnityEngine;

namespace LazyCoder.Collect
{
    public class CollectStepActionScale : CollectStepAction
    {
        [SerializeField] private Vector3 _value = Vector3.one;

        protected override Tween GetTween(CollectObject obj)
        {
            return obj.TransformCached.DOScale(_value, _duration)
                                      .SetEase(_ease);
        }
    }
}
