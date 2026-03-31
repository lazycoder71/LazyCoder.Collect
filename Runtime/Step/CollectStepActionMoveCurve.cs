using DG.Tweening;
using LazyCoder.Core;
using UnityEngine;

namespace LazyCoder.Collect
{
    [System.Serializable]
    public class CollectStepActionMoveCurve : CollectStepActionMove
    {
        [SerializeField] private AnimationCurve _x;
        [SerializeField] private AnimationCurve _y;

        protected override Tween GetTween(CollectItem item)
        {
            Vector3 startPos = GetStartPosition(item);
            Vector3 endPos = GetEndPosition(item);

            return DOVirtual.Float(0f, 1f, _duration, (time) =>
            {
                float x = Mathf.Lerp(startPos.x, endPos.x, _x.Evaluate(time));
                float y = Mathf.Lerp(startPos.y, endPos.y, _y.Evaluate(time));

                item.TransformCached.SetXY(x, y);
            }).SetEase(_ease);
        }
    }
}