using DG.Tweening;
using LazyCoder.Core;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace LazyCoder.Collect
{
    [System.Serializable]
    public abstract class CollectStepActionMove : CollectStepAction
    {
        [Serializable]
        public enum Journey
        {
            Spawn = 0,
            Return = 1,
        }

        [SerializeField] protected Journey _journey;

        [ShowIf("@_journey == Journey.Spawn")]
        [SerializeField] protected bool _spawnAtCenter;

        [ShowIf("@_journey == Journey.Spawn")]
        [SerializeField] protected Vector3 _spawnOffset;

        public override string DisplayName => $"{base.DisplayName} ({_journey})";

        protected Vector3 GetStartPosition(CollectItem item)
        {
            switch (_journey)
            {
                case Journey.Spawn:
                    if (_spawnAtCenter)
                        return item.TransformCached.parent.position +
                               _spawnOffset * item.RectTransform.GetUnitPerPixel();

                    return item.SpawnPosition + _spawnOffset * item.RectTransform.GetUnitPerPixel();
                
                case Journey.Return:
                    return item.SpawnPosition;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected Vector3 GetEndPosition(CollectItem item)
        {
            switch (_journey)
            {
                case Journey.Spawn:
                    return item.SpawnPosition;
                
                case Journey.Return:
                    return item.DestinationPosition;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}