using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;



public class SaveLoadSystem : MonoBehaviour
{
    public string saveName;

    public string savePath => $"{Application.persistentDataPath}/"+ saveName + ".txt";


    [ContextMenu("Save")]
    public void Save()
    {
        var state = LoadFile();
        SaveState(state);
        SaveFile(state);
    }

    [ContextMenu("Load")]
    public void Load()
    {
        var state = LoadFile();
        LoadState(state);
    }

    void SaveFile(object state)
    {
        using (var stream = File.Open(savePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, state);
        }
    }
    Dictionary<string, object> LoadFile()
    {
        if(!File.Exists(savePath))
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(savePath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }

    void SaveState(Dictionary<string, object> state)
    {
        foreach(var saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.Id] = saveable.SaveState();
        }
    }

    void LoadState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            if (state.TryGetValue(saveable.Id, out object savedState))
            {
                saveable.LoadState(savedState);
            }
        }
    }
}
