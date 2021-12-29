using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class PersistentStorage : MonoBehaviour
{
    string savePath;

    private void Awake()
    {
        savePath = Path.Combine(Application.persistentDataPath, "saveFile");
    }

    public void Save(PersistableObject o, int version)
    {
        using (var write = new BinaryWriter(File.Open(savePath, FileMode.Create)))
        {
            write.Write(-version);
            o.Save(new GameDataWriter(write));
        }
    }

    public void Load(PersistableObject o)
    {
        using (var reader = new BinaryReader(File.Open(savePath, FileMode.Open)))
        {
            o.Load(new GameDataReader(reader, -reader.ReadInt32()));
        }
    }
}
