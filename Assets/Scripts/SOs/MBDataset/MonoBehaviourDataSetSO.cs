using System;
using System.Collections.Generic;
using UnityEngine;

namespace SOs.DataSet
{
    /**
     * A SO based dataset that stores MonoBehaviour prefabs with corresponding enum id
     * I: enum id to be used
     * M: the MonoBehaviour to be stored
     */
    public class MonoBehaviourDataSetSO<I, M> : ScriptableObject where I : Enum where M : MonoBehaviour
    {
        [SerializeField] private PrefabDataSetEntry[] _dataEntry;
        private Dictionary<I, M> _data;

        public M this[I id]
        {
            get
            {
                if (_data == null)
                    InitializeDataSet();

                if (!ContainsID(id))
                {
                    Debug.LogError($"{typeof(I)} Id {id} does not exist in dataset {name}");
                    return null;
                }

                return _data[id];
            }
        }

        public bool ContainsID(I id) => _data.ContainsKey(id);

        // Setup the dictionary from entries
        private void InitializeDataSet()
        {
            _data = new Dictionary<I, M>();
            for (int i = 0; i < _dataEntry.Length; i++)
            {
                I id = _dataEntry[i].ID;
                M value = _dataEntry[i].Prefab;

                if (id == null || value == null)
                {
                    Debug.LogWarning($"Null id or value in data entry at index {i}: {id}, {value}");
                    continue;
                }

                if (_data.ContainsKey(id))
                {
                    Debug.LogWarning($"Value with Id {id}, {_data[id]} being overrided with {value}");
                    _data[id] = value;
                    continue;
                }

                _data.Add(id, value);
            }
        }

        [Serializable]
        public struct PrefabDataSetEntry
        {
            public I ID { get => _id; }
            public M Prefab { get => _prefab; }
            [SerializeField] private I _id;
            [SerializeField] private M _prefab;
        }
    }
}