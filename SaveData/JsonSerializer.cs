using System.IO;
using UnityEngine;
public class JsonSerializer : Serializer
{
    public override void Save<T>(T data, string filesPath, string filename)
    {
        if (!Directory.Exists(filesPath))
        {
            Directory.CreateDirectory(filesPath);
        }
        string strData = JsonUtility.ToJson(data);
        File.WriteAllText(SaveLoadData.GetFilePath(filename), strData);
    }
    public override T Load<T>(string filename)
    {
        T data = default(T);
        if (SaveLoadData.HasSaved(filename))
        {
          data = JsonUtility.FromJson<T>(File.ReadAllText(SaveLoadData.GetFilePath(filename)));
        }
        return data;
    }
}
