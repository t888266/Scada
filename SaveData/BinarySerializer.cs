using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
/// <summary>
/// BinarySerializer is tool to save your game data in files as binary
/// so that no body can read it or modify it, 
/// this tool is generic.
/// 
/// Allowed types : int, float , strings, ...(all .Net types)  
/// But for unity types it accepts only 5 types: Vector2, Vector3, Vector4, Color, Quaternion.
/// 
/// Developed by Hamza Herbou
/// GITHUB : https://github.com/herbou
/// </summary>

namespace BinaryConfig
{
    public class BinarySerializer : Serializer
    {
        protected SurrogateSelector surrogateSelector;
        public BinarySerializer() : base()
        {
            surrogateSelector = GetSurrogateSelector();
        }

        public override void Save<T>(T data, string filesPath, string filename)
        {
            if (IsSerializable<T>())
            {
                if (!Directory.Exists(filesPath))
                {
                    Directory.CreateDirectory(filesPath);
                }
                using (Stream file = File.Create(SaveLoadData.GetFilePath(filename)))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.SurrogateSelector = surrogateSelector;
                    formatter.Serialize(file, data);
                }
            }
        }

        public override T Load<T>(string filename)
        {
            T data = default(T);
            if (IsSerializable<T>())
            {
                if (SaveLoadData.HasSaved(filename))
                {
                    using (Stream file = File.OpenRead(SaveLoadData.GetFilePath(filename)))
                    {
                        BinaryFormatter formatter = new BinaryFormatter();
                        formatter.SurrogateSelector = surrogateSelector;
                        data = (T)formatter.Deserialize(file);
                    }
                }
            }
            return data;
        }
        protected bool IsSerializable<T>()
        {
            bool isSerializable = typeof(T).IsSerializable;
            if (!isSerializable)
            {
                string type = typeof(T).ToString();
                Debug.LogError(
                "Class <b><color=white>" + type + "</color></b> is not marked as Serializable,"
                + "make sure to add <b><color=white>[System.Serializable]</color></b> at the 
                top of your " + type + " class."
                );
            }

            return isSerializable;
        }
        protected SurrogateSelector GetSurrogateSelector()
        {
            SurrogateSelector surrogateSelector = new SurrogateSelector();

            Vector2_SS v2_ss = new Vector2_SS();
            Vector3_SS v3_ss = new Vector3_SS();
            Vector4_SS v4_ss = new Vector4_SS();
            Color_SS co_ss = new Color_SS();
            Quaternion_SS qu_ss = new Quaternion_SS();

            surrogateSelector.AddSurrogate(typeof(Vector2), new StreamingContext
            (StreamingContextStates.All), v2_ss);
            surrogateSelector.AddSurrogate(typeof(Vector3), new StreamingContext
            (StreamingContextStates.All), v3_ss);
            surrogateSelector.AddSurrogate(typeof(Vector4), new StreamingContext
            (StreamingContextStates.All), v4_ss);
            surrogateSelector.AddSurrogate(typeof(Color), new StreamingContext
            (StreamingContextStates.All), co_ss);
            surrogateSelector.AddSurrogate(typeof(Quaternion), new StreamingContext
            (StreamingContextStates.All), qu_ss);

            return surrogateSelector;
        }
    }
}

