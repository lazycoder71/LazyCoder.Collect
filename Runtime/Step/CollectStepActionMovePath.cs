using DG.Tweening;
using LazyCoder.Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LazyCoder.Collect
{
    public class CollectStepActionMovePath : CollectStepActionMove
    {
        [SerializeField] private PathType _pathType;

        [ValidateInput("CheckPoints", "Path Type: Cubic Bezier - Control points must be 2")]
        [SerializeField] private Vector3[] _points;

        protected override Tween GetTween(CollectObject obj)
        {
            Vector3 posStart = Vector3.zero;
            Vector3 posEnd = Vector3.zero;

            switch (_journey)
            {
                case Journey.Spawn:
                    posEnd = obj.TransformCached.localPosition;
                    posStart = _startAtCenter ? Vector3.zero : posEnd + _startOffset * obj.RectTransform.GetUnitPerPixel();
                    break;
                case Journey.Return:
                    posEnd = obj.Destination.Position;
                    posStart = obj.TransformCached.position;
                    break;
            }

            if (_pathType == PathType.CubicBezier)
            {
                Vector3[] points = new Vector3[3];

                points[0] = posEnd;
                points[1] = posStart + (posEnd - posStart).MultipliedBy(_points[0]);
                points[2] = posStart + (posEnd - posStart).MultipliedBy(_points[1]);

                return obj.TransformCached.DOPath(points, _duration, _pathType, PathMode.Sidescroller2D, 10, Color.red);
            }
            else
            {
                Vector3[] points = new Vector3[_points.Length + 2];

                points[0] = posStart;
                points[points.Length - 1] = posEnd;

                for (int i = 0; i < _points.Length; i++)
                {
                    points[i + 1] = posStart + (posEnd - posStart).MultipliedBy(_points[i]);
                }

                return obj.TransformCached.DOPath(points, _duration, _pathType, PathMode.Full3D, 10, Color.red);
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
