using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMenu<T> : MonoBehaviour where T : class
{
    private static T instance;
    public static T Instance => instance;
    public abstract void Init();
    public virtual void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this as T;
    }
    private void Start()
    {
        Init();
    }

    protected void ChangeInstance(T a)
    {
        instance = a;
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
}
