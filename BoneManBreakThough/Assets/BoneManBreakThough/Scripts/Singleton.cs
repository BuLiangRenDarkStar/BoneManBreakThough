using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T>:MonoBehaviour where T:MonoBehaviour
{
    private static T _instace;

    public static T Instance
    {
        get
        {
            _instace = (T)FindObjectOfType(typeof(T));
            if( _instace == null)
            {
                GameObject go = new GameObject();
                _instace = go.AddComponent<T>();
                go.name = typeof(T).ToString();
            }
            return _instace;
        }
    }
}
