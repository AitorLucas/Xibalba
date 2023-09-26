using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class LevelEnemiesSO : ScriptableObject, ISerializationCallbackReceiver {

    [SerializeField] List<EnemySO> _keys;
    [SerializeField] List<int> _values;

    public Dictionary<EnemySO, int> enemiesDictionary;

    public void OnBeforeSerialize() {
        _keys.Clear();
        _values.Clear();

        foreach (var kvp in enemiesDictionary) {
            _keys.Add(kvp.Key);
            _values.Add(kvp.Value);
        }
    }

    public void OnAfterDeserialize() {
        enemiesDictionary = new Dictionary<EnemySO, int>();

        for (int i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
            enemiesDictionary.Add(_keys[i], _values[i]);
    }
    
    void OnGUI() {
        foreach (var kvp in enemiesDictionary)
            GUILayout.Label("Key: " + kvp.Key + " value: " + kvp.Value);
    }
}
