using DG.Tweening;

namespace LazyCoder.Collect
{
    [System.Serializable]
    public class CollectStepActionTransformScale : CollectStepActionTransform
    {
        protected override Tween GetTween(CollectItem item)
        {
            var tween = item.TransformCached.DOScale(Value, _duration);

            if (ChangeStartValue)
                tween.ChangeStartValue(ValueStart);

            return tween;
        }
    }
}