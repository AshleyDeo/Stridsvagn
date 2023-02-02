using System;
using System.Collections.Generic;
using UnityEngine;

public class SaveableEntity : MonoBehaviour {
    [SerializeField] private string id;
    public string ID => id;
    [ContextMenu("Generate ID")]
    private void GenerateID() {
        id = Guid.NewGuid().ToString();
    }
    public object SaveState() {
        var state = new Dictionary<string, object>();
        foreach (var saveable in GetComponents<ISaveable>()) {
            state[saveable.GetType().ToString()] = saveable.SaveState();
        }
        return state;
    }
    public void LoadState(object state) {
        var stateDict = (Dictionary<string, object>)state;
        foreach (var saveable in GetComponents<ISaveable>()) {
            string typeName = saveable.GetType().ToString();
            if (stateDict.TryGetValue(typeName, out object savedState)) {
                saveable.LoadState(savedState);
            }
        }
    }
}