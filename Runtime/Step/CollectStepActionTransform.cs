using Sirenix.OdinInspector;
using UnityEngine;

namespace LazyCoder.Collect
{
    public class CollectStepActionTransform : CollectStepAction
    {
        [SerializeField] protected bool ChangeStartValue;
        
        [ShowIf("@ChangeStartValue")]
        [SerializeField] protected Vector3 ValueStart;

        [SerializeField] protected Vector3 Value;
    }
}
