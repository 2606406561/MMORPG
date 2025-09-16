using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventSystemManager
{
    private static EventSystemManager instance = new EventSystemManager();
    public static EventSystemManager Instance => instance;

    private Dictionary<int, List<UnityAction<object>>> dic = new Dictionary<int, List<UnityAction<object>>>();
    private Dictionary<char, List<UnityAction>> dic1 = new Dictionary<char, List<UnityAction>>();

    public void Register(int a, UnityAction<object> ua)
    {
        if (!dic.ContainsKey(a)) dic[a] = new List<UnityAction<object>>();
        dic[a].Add(ua);
    }

    public void Register(char a, UnityAction ua)
    {
        if (!dic1.ContainsKey(a)) dic1[a] = new List<UnityAction>();
        dic1[a].Add(ua);
    }

    public void Publish(int a, object obj)
    {
        if (!dic.ContainsKey(a)) return;
        foreach (UnityAction<object> ua in dic[a])
        {
            ua.Invoke(obj);
        }
    }
    public void Publish(char a)
    {
        if (!dic1.ContainsKey(a)) return;
        foreach (UnityAction ua in dic1[a])
        {
            ua.Invoke();
        }
    }
    public void Unregister(int a, UnityAction<object> ua)
    {
        if (!dic.ContainsKey(a)) return;
        dic[a].Remove(ua);
    }
    public void Unregister(char a, UnityAction ua)
    {
        if (!dic1.ContainsKey(a)) return;
        dic1[a].Remove(ua);
    }
}
