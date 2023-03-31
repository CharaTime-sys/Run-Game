using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Global_Instance<T> : MonoBehaviour where T:MonoBehaviour
{
    static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = (T)FindObjectOfType<T>();
            }
            return instance;
        }

    }
    private void OnEnable()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void Awake()
    {
        if (Instance != null && instance != this.gameObject.GetComponent<T>())
        {
            Destroy(gameObject);
        }
    }
}
