using DG.Tweening;
using UnityEngine;

namespace LazyCoder.Collect
{
    public class CollectStepInterval : CollectStep
    {
        [SerializeField] private float _duration;

        public override void Apply(CollectGroupItem item)
        {
            item.Sequence.AppendInterval(_duration);
        }
    }
}
