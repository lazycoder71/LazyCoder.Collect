using DG.Tweening;
using UnityEngine;

namespace LazyCoder.Collect
{
    [System.Serializable]
    public class CollectStepActionTransformRotate : CollectStepActionTransform
    {
        [SerializeField] private RotateMode _rotateMode;

        protected override Tween GetTween(CollectItem item)
        {
            var tween = item.TransformCached.DORotate(Value, _duration, _rotateMode);

            if (ChangeStartValue)
                tween.ChangeStartValue(ValueStart);

            return tween;
        }
    }
}