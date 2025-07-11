using DG.Tweening;
using UnityEngine;

namespace LazyCoder.Collect
{
    public class CollectStepActionMoveStraight : CollectStepActionMove
    {
        protected override Tween GetTween(CollectObject obj)
        {
            switch (_journey)
            {
                case Journey.Spawn:
                    Vector3 endPos = obj.TransformCached.localPosition;
                    Vector3 startPos = _startAtCenter ? Vector3.zero : endPos + _startOffset * obj.RectTransform.GetUnitPerPixel();

                    return obj.TransformCached.DOLocalMove(endPos, _duration)
                                              .ChangeStartValue(startPos)
                                              .SetEase(_ease);
                case Journey.Return:
                    return obj.TransformCached.DOMove(obj.Destination.Position, _duration)
                                              .SetEase(_ease);
                default:
                    return null;
            }
        }
    }
}
