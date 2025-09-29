using DG.Tweening;

namespace LazyCoder.Collect
{
    public class CollectStepActionTransformScale : CollectStepActionTransform
    {
        protected override Tween GetTween(CollectGroupItem item)
        {
            var tween = item.TransformCached.DOScale(Value, _duration);

            if (ChangeStartValue)
                tween.ChangeStartValue(ValueStart);

            return tween;
        }
    }
}