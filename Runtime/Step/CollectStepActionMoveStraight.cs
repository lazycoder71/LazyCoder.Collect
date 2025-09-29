using DG.Tweening;
using LazyCoder.Core;
using UnityEngine;

namespace LazyCoder.Collect
{
    public class CollectStepActionMoveStraight : CollectStepActionMove
    {
        protected override Tween GetTween(CollectGroupItem item)
        {
            switch (_journey)
            {
                case Journey.Spawn:
                    Vector3 endPos = item.TransformCached.localPosition;
                    Vector3 startPos = _startAtCenter ? Vector3.zero : endPos + _startOffset * item.RectTransform.GetUnitPerPixel();

                    return item.TransformCached.DOLocalMove(endPos, _duration)
                                              .ChangeStartValue(startPos)
                                              .SetEase(_ease);
                case Journey.Return:
                    return item.TransformCached.DOMove(item.Destination.Position, _duration)
                                              .SetEase(_ease);
                default:
                    return null;
            }
        }
    }
}
