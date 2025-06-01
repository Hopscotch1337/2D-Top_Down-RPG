using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Singelton<T> : MonoBehaviour where T : Singelton<T>
{
    private static T instance;
    public static T Instance { get { return instance; } }

    protected virtual void Awake()
    {
        if (instance != null && this.gameObject != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = (T)this;
        }
        if(transform.parent ==null)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    
}

