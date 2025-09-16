using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.Events;

public class BackPackManager
{
    public Weapon weapon;
    private static BackPackManager instance = new BackPackManager();
    public static BackPackManager Instance => instance;
    public PlayerData playerData;
    public Dictionary<Id, Context> dic;
    List<object> playerMsglist = new List<object>();
    private string path = "PlayerData";
    public string text;//读取/保存 存档的名字(还没写:游戏中退回菜单要清空)
    public class Id : IEquatable<Id>
    {
        public int id;
        public Id(int i) 
        {
            id = i;
        }
        public bool Equals(Id other)
        {
            if (other == null) return false;
            return id == other.id;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Id);
        }
        public override int GetHashCode()
        {
            // ʹ�ò��ɱ��ֶ����ɹ�ϣ��
            return HashCode.Combine(id);  // C# 7.0+ �Ƽ�д��
        }
    }
    public class Context
    {
        public string name;
        public string text;
        public Context(string a,string b)
        {
            name = a; 
            text = b;
        }
    }
    public void Init()
    {
        playerData = new PlayerData();
        playerData.Init();
        //playerData = JsonMgr.Instance.LoadData<PlayerData>(path + text);
        dic = new Dictionary<Id, Context>();
        dic.Add(new Id(1), new Context("日轮刀", "某人曾使用过的"));
        dic.Add(new Id(2), new Context("破坏剑", "是一把英雄之剑，它承载着战士的梦想、荣誉、骄傲、意志，流传有序，是一座英雄丰碑。"));
        dic.Add(new Id(3), new Context("恢复药水", "恢复20HP，可惜还是缺了点什么"));
        dic.Add(new Id(4), new Context("超级恢复药水", "恢复100HP，这下什么都不缺了"));
        dic.Add(new Id(5), new Context("超级耐力药水", "恢复100耐力"));
        dic.Add(new Id(6), new Context("挑战卷轴", "貌似可以用来挑战某个人..."));
    }


    public enum TP
    {
        Weapon,
        Consume,
        Object
    }
    public void AddItem(Id tp, int sum = 1)
    {
        if (sum == 0) return;
        for (int i = 0; i < playerData.list.Count; i++)
        {
            if (playerData.list[i].id == tp.id)
            {
                playerData.list[i].sum += sum;
                return;
            }
        }
        playerData.list.Add(new PlayerData.ItemInfo(tp.id, sum));
    }

    public int CheckItem(int id)
    {
        for (int i = 0; i < playerData.list.Count; i++)
        {
            if (playerData.list[i].id == id)
                return 1;
        }
        return 0;
    }

    public void SetItem(Id tp, int sum)
    {
        for (int i = 0; i < playerData.list.Count; i++)
        {
            if (playerData.list[i].id == tp.id)
            {
                playerData.list[i].sum += sum;
                return;
            }
        }
    }

    public Context ClickItem(Id tp)
    {
        return dic[tp];
    }
    public void Save()
    {
        JsonMgr.Instance.SaveData(playerData, path + text);
    }

}
