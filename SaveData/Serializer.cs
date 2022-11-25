using UnityEngine;
using System.Runtime.Serialization;
public abstract class Serializer
{

    protected Serializer() { }
    public abstract void Save<T>(T data, string filesPath, string filename);
    public abstract T Load<T>(string filename);
}


namespace BinaryConfig
{
    public class Vector2_SS : ISerializationSurrogate
    {
        //Serialize Vector2
        public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext
         context)
        {
            Vector2 v2 = (Vector2)obj;
            info.AddValue("x", v2.x);
            info.AddValue("y", v2.y);
        }
        //Deserialize Vector2
        public System.Object SetObjectData(System.Object obj, SerializationInfo info, 
        StreamingContext context, ISurrogateSelector selector)
        {
            Vector2 v2 = (Vector2)obj;

            v2.x = (float)info.GetValue("x", typeof(float));
            v2.y = (float)info.GetValue("y", typeof(float));

            obj = v2;
            return obj;
        }
    }

    public class Vector3_SS : ISerializationSurrogate
    {
        //Serialize Vector3
        public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext
         context)
        {
            Vector3 v3 = (Vector3)obj;
            info.AddValue("x", v3.x);
            info.AddValue("y", v3.y);
            info.AddValue("z", v3.z);
        }
        //Deserialize Vector3
        public System.Object SetObjectData(System.Object obj, SerializationInfo info, 
        StreamingContext context, ISurrogateSelector selector)
        {
            Vector3 v3 = (Vector3)obj;

            v3.x = (float)info.GetValue("x", typeof(float));
            v3.y = (float)info.GetValue("y", typeof(float));
            v3.z = (float)info.GetValue("z", typeof(float));

            obj = v3;
            return obj;
        }
    }

    public class Vector4_SS : ISerializationSurrogate
    {
        //Serialize Vector4
        public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext 
        context)
        {
            Vector4 v4 = (Vector4)obj;
            info.AddValue("x", v4.x);
            info.AddValue("y", v4.y);
            info.AddValue("z", v4.z);
            info.AddValue("w", v4.w);
        }
        //Deserialize Vector4
        public System.Object SetObjectData(System.Object obj, SerializationInfo info, 
        StreamingContext context, ISurrogateSelector selector)
        {
            Vector4 v4 = (Vector4)obj;

            v4.x = (float)info.GetValue("x", typeof(float));
            v4.y = (float)info.GetValue("y", typeof(float));
            v4.z = (float)info.GetValue("z", typeof(float));
            v4.w = (float)info.GetValue("w", typeof(float));

            obj = v4;
            return obj;
        }

    }

    public class Color_SS : ISerializationSurrogate
    {
        //Serialize Color
        public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext
         context)
        {
            Color color = (Color)obj;
            info.AddValue("r", color.r);
            info.AddValue("g", color.g);
            info.AddValue("b", color.b);
            info.AddValue("a", color.a);
        }
        //Deserialize Color
        public System.Object SetObjectData(System.Object obj, SerializationInfo info,
         StreamingContext context, ISurrogateSelector selector)
        {
            Color color = (Color)obj;

            color.r = (float)info.GetValue("r", typeof(float));
            color.g = (float)info.GetValue("g", typeof(float));
            color.b = (float)info.GetValue("b", typeof(float));
            color.a = (float)info.GetValue("a", typeof(float));

            obj = color;
            return obj;
        }
    }

    public class Quaternion_SS : ISerializationSurrogate
    {
        //Serialize Quaternion
        public void GetObjectData(System.Object obj, SerializationInfo info, StreamingContext
         context)
        {
            Quaternion qua = (Quaternion)obj;
            info.AddValue("x", qua.x);
            info.AddValue("y", qua.y);
            info.AddValue("z", qua.z);
            info.AddValue("w", qua.w);
        }
        //Deserialize Quaternion
        public System.Object SetObjectData(System.Object obj, SerializationInfo info, 
        StreamingContext context, ISurrogateSelector selector)
        {
            Quaternion qua = (Quaternion)obj;

            qua.x = (float)info.GetValue("x", typeof(float));
            qua.y = (float)info.GetValue("y", typeof(float));
            qua.z = (float)info.GetValue("z", typeof(float));
            qua.w = (float)info.GetValue("w", typeof(float));

            obj = qua;
            return obj;
        }
    }
}