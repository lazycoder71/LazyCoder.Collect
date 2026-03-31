using DG.Tweening;
using LazyCoder.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LazyCoder.Collect
{
    [System.Serializable]
    public class CollectStepActionMovePath : CollectStepActionMove
    {
        [SerializeField] private PathType _pathType;

        [ValidateInput("CheckPoints", "Path Type: Cubic Bezier - Control points must be 2")]
        [SerializeField] private Vector3[] _points;

        protected override Tween GetTween(CollectItem item)
        {
            Vector3 startPos = GetStartPosition(item);
            Vector3 endPos = GetEndPosition(item);

            if (_pathType == PathType.CubicBezier)
            {
                Vector3[] points = new Vector3[3];

                points[0] = endPos;
                points[1] = startPos + (endPos - startPos).MultipliedBy(_points[0]);
                points[2] = startPos + (endPos - startPos).MultipliedBy(_points[1]);

                return item.TransformCached.DOPath(points, _duration, _pathType, PathMode.Sidescroller2D, 10, Color.red);
            }
            else
            {
                Vector3[] points = new Vector3[_points.Length + 2];

                points[0] = startPos;
                points[^1] = endPos;

                for (int i = 0; i < _points.Length; i++)
                {
                    points[i + 1] = startPos + (endPos - startPos).MultipliedBy(_points[i]);
                }

                return item.TransformCached.DOPath(points, _duration, _pathType, PathMode.Full3D, 10, Color.red);
            }
        }

        private bool CheckPoints()
        {
            if (_pathType == PathType.CubicBezier)
                return _points.Length == 2;
            
            return true;
        }
    }
}
