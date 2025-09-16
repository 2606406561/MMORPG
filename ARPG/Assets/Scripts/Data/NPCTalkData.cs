using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCTalkData
{
    private static NPCTalkData instance = new NPCTalkData();
    public static NPCTalkData Instance => instance;
    public Dictionary<int, NPC> dic = new Dictionary<int, NPC>();
    public Dictionary<int, int> nowSum = new Dictionary<int, int>();
    public Dictionary<int, (string, string)> descript = new Dictionary<int, (string, string)>();
    public struct NPC
    {
        public string name;
        public string[] context;
        public PlayerData.Task.Ta ta;
        public Dictionary<BackPackManager.Id, int> dict;
    }
    public void Init()
    {
        descript.Add(1, ("消灭领主", "去前面的传送门看看吧"));
        Dictionary<BackPackManager.Id, int> dict = new Dictionary<BackPackManager.Id, int>();
        dict.Add(new BackPackManager.Id(1), 1);
        dict.Add(new BackPackManager.Id(3), 10);
        dict.Add(new BackPackManager.Id(4), 1);
        dic.Add(1, new NPC { name = new string("路人甲"), context = new string[] { BackPackManager.Instance.playerData.playerName + ",欢迎来到异世界。", "我似乎遇到了一些麻烦,你能帮帮我吗?", "前面的传送门里是这附近的领主", "你能帮我去消灭她吗?" }, ta = new PlayerData.Task.Ta(1, descript[1].Item1, descript[1].Item2, PlayerData.Task.TP.Kill, 1), dict = dict });
        nowSum.Add(1, 0);
    }
}
