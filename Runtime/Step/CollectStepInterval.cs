using DG.Tweening;
using UnityEngine;

namespace LazyCoder.Collect
{
    [System.Serializable]
    public class CollectStepInterval : CollectStep
    {
        [SerializeField] private float _duration;

        public override void Apply(CollectItem item)
        {
            item.Sequence.AppendInterval(_duration);
        }
    }
}
