﻿using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
            }
            return instance;
        }
    }
}
