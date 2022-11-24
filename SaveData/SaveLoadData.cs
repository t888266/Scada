using UnityEngine;
using System.IO;
public enum TypeSaveMothod
{
    Binary,
    Json,
}
public static class SaveLoadData
{
    static BinaryConfig.BinarySerializer binarySerializer = new BinaryConfig.BinarySerializer();
    static JsonSerializer jsonSerializer = new JsonSerializer();
    static string folderName = "AppData";
    static string filesPath = Application.persistentDataPath + "/" + folderName;
    public static void Save<T>(T data, string filename, TypeSaveMothod typeSaveMothod)
    {
        Method(typeSaveMothod).Save<T>(data, filesPath, filename);
    }

    public static T Load<T>(string filename, TypeSaveMothod typeSaveMothod)
    {
        return Method(typeSaveMothod).Load<T>(filename);
    }

    public static bool HasSaved(string filename)
    {
        return File.Exists(GetFilePath(filename));
    }
    public static void DeleteDataFile(string filename)
    {
        if (HasSaved(filename))
        {
            File.Delete(GetFilePath(filename));
        }
    }

    public static void DeleteAllDataFiles()
    {
        if (Directory.Exists(filesPath))
        {
            Directory.Delete(filesPath, true);
        }
    }

    public static string GetDataPath()
    {
        return filesPath + "/";
    }

    public static string GetFilePath(string filename)
    {
        return filesPath + "/" + filename;
    }
    static Serializer Method(TypeSaveMothod typeSaveMothod)
    {
        switch (typeSaveMothod)
        {
            case TypeSaveMothod.Binary:
                return binarySerializer;
            default:
                return jsonSerializer;
        }
    }
}
