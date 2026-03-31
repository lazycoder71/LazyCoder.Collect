using DG.Tweening;
using UnityEngine;

namespace LazyCoder.Collect
{
    [System.Serializable]
    public class CollectStepActionMoveStraight : CollectStepActionMove
    {
        protected override Tween GetTween(CollectItem item)
        {
            Vector3 startPos = GetStartPosition(item);
            Vector3 endPos = GetEndPosition(item);

            return item.TransformCached.DOMove(endPos, _duration)
                .ChangeStartValue(startPos)
                .SetEase(_ease);
        }
    }
}