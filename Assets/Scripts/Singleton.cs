using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour
where T : MonoBehaviour
{
    private static T m_Instance;

    public static T Instance
    {
        get
        {
            return m_Instance;
        }

    }

    protected virtual void Awake()
    {
        m_Instance = this as T;
    }

}
