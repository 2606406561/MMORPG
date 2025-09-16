using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnload : MonoBehaviour
{
    private static DontDestroyOnload instance;
    public static DontDestroyOnload Instance => instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
