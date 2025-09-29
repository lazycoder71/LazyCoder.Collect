using DG.Tweening;
using UnityEngine;

namespace LazyCoder.Collect
{
    public class CollectStepActionTransformRotate : CollectStepActionTransform
    {
        [SerializeField] private RotateMode _rotateMode;

        protected override Tween GetTween(CollectGroupItem item)
        {
            var tween = item.TransformCached.DORotate(Value, _duration, _rotateMode);

            if (ChangeStartValue)
                tween.ChangeStartValue(ValueStart);

            return tween;
        }
    }
}