using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance { get => _instance; }

    protected virtual void Awake()
    {
        if (_instance == null)
        {
            _instance = gameObject.GetComponent<T>();
            if (_instance == null)
            {
                Destroy(gameObject);
                GameObject obj = new GameObject();
                obj.name = typeof(T).ToString();
                _instance = obj.AddComponent<T>();
            }
        }
        DontDestroyOnLoad(this);
    }
    protected virtual void OnDestroy()
    {
        _instance = null;
    }
}
