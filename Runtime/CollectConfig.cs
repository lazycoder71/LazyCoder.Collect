using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace LazyCoder.Collect
{
    [System.Serializable]
    public class CollectConfig : ScriptableObject
    {
        [Title("Spawn")]
        [AssetsOnly]
        [SerializeField] GameObject _spawnPrefab;
        [SerializeField] float _spawnDuration = 0.5f;

        [FoldoutGroup("SpawnSample", GroupName = "Spawn Sample (in pixel unit)", Expanded = false)]
        [SerializeField] List<Vector3> _spawnSamplePositions;

        [FoldoutGroup("SpawnSample")]
        [SerializeField] int _spawnSampleCount = 10;

        [FoldoutGroup("SpawnSample")]
        [SerializeField] float _spawnSampleRadius = 100.0f;

        [Title("Step")]
        [ListDrawerSettings(ShowIndexLabels = false, OnBeginListElementGUI = "BeginDrawListElement", OnEndListElementGUI = "EndDrawListElement")]
        [SerializeReference] CollectStep[] _steps;

        public GameObject spawnPrefab { get { return _spawnPrefab; } }
        public float spawnDuration { get { return _spawnDuration; } }
        public List<Vector3> spawnPositions { get { return _spawnSamplePositions; } }
        public float spawnSampleRadius { get { return _spawnSampleRadius; } }
        public CollectStep[] steps { get { return _steps; } }

#if UNITY_EDITOR

        [Button("Pure Random", Icon = SdfIconType.Dice3Fill), HorizontalGroup("SpawnSample/Random")]
        private void RandomSpawnSamplePosition()
        {
            _spawnSamplePositions = new List<Vector3>();

            for (int i = 0; i < _spawnSampleCount; i++)
            {
                _spawnSamplePositions.Add(Random.insideUnitCircle * _spawnSampleRadius);
            }
        }

        [Button("Halton Random", Icon = SdfIconType.Dice5Fill), HorizontalGroup("SpawnSample/Random")]
        private void RandomSpawnSamplePositionHalton()
        {
            _spawnSamplePositions = new List<Vector3>();

            UtilsHaltonSequence.Reset();

            while (_spawnSamplePositions.Count < _spawnSampleCount)
            {
                UtilsHaltonSequence.Increment(true, true, false);

                Vector3 position = new Vector3(-_spawnSampleRadius, -_spawnSampleRadius) + (UtilsHaltonSequence.currentPosition * _spawnSampleRadius * 2.0f);

                if (Vector3.Distance(Vector3.zero, position) > _spawnSampleRadius)
                    continue;

                _spawnSamplePositions.Add(position);
            }
        }

        private void BeginDrawListElement(int index)
        {
            Sirenix.Utilities.Editor.SirenixEditorGUI.BeginBox(_steps[index].DisplayName);
        }

        private void EndDrawListElement(int index)
        {
            Sirenix.Utilities.Editor.SirenixEditorGUI.EndBox();
        }

#endif
    }
}
