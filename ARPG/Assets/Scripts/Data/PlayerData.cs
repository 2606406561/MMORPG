using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    [Serializable]
    public class ItemInfo
    {
        public int id;
        public int sum;
        public ItemInfo(int id,int sum)
        {
            this.id = id;
            this.sum = sum;
        }
        public ItemInfo()
        {
        }
    }

    [Serializable]
    public class Task
    {
        public Dictionary<int, bool> overTask;
        public List<Ta> list;

        [Serializable]
        public enum TP
        {
            Kill,
            Task
        }
        [Serializable]
        public class Ta
        {
            public int id;
            public string name;
            public string description;
            public TP type;
            public int needSum;
            public Ta()
            {

            }
            public Ta(int id, string name, string de, TP tp, int needSum)
            {
                this.id = id;
                this.name = name;
                this.description = de;
                this.type = tp;
                this.needSum = needSum;
            }
        }

        public Task()
        {
            overTask = new Dictionary<int, bool>();
            list = new List<Ta>();
        }
    }

    public int piercing;
    public int hp;
    public int damage;
    public List<ItemInfo> list;
    public Task task;
    public Vector3 nowPos;
    public int scene_id;
    public int sex;//0Ϊ�У�1ΪŮ
    public string playerName;
    public void Init()
    {
        list = new List<PlayerData.ItemInfo>();
        task = new Task();
        sex = BackPackManager.Instance.playerData.sex;
        scene_id = BackPackManager.Instance.playerData.scene_id;
    }
}
