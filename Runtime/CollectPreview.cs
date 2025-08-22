using LazyCoder.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using Vertx.Debugging;

namespace LazyCoder.Collect
{
    [RequireComponent(typeof(RectTransform))]
    public class CollectPreview : MonoBehaviour
    {
        [Title("Reference")]
        [SerializeField, AssetsOnly] private CollectConfig _config;
        [SerializeField] private Color _colorStart = Color.white;
        [SerializeField] private Color _colorEnd = Color.black;

        private void OnDrawGizmos()
        {
            if (_config == null)
                return;

            float unitPerPixel = GetComponent<RectTransform>().GetUnitPerPixel();

            for (int i = 0; i < _config.spawnPositions.Count; i++)
            {
                D.raw(new Shape.Circle2D(transform.TransformPoint(_config.spawnPositions[i]), 5.0f * unitPerPixel), Color.Lerp(_colorStart, _colorEnd, (float)i / (_config.spawnPositions.Count - 1)));
            }

            D.raw(new Shape.Circle2D(transform.position, _config.spawnSampleRadius * unitPerPixel), Color.yellow);
        }
    }
}
