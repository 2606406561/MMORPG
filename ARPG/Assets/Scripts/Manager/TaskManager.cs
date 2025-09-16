using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class TaskManager : MonoBehaviour
{
    private static TaskManager instance;
    public static TaskManager Instance => instance;
    [SerializeField, Header("任务面板位置")] public Transform pos;
    private PlayerData.Task ta;
    private GameObject task;
    private bool isMsg3, isMsg12;
    private Dictionary<int, Transform> dic;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        dic = new Dictionary<int, Transform>();
        task = Resources.Load<GameObject>("Prefabs/task");
        ta = BackPackManager.Instance.playerData.task;
        UnityAction<object> unityAction = HandleMsg3;
        EventSystemManager.Instance.Register(3, unityAction);
        List<object> list = new List<object>();
        list.Add(3);
        list.Add(TCPClick.Instance.user_ID);
        isMsg3 = true;
        TCPClick.Instance.SendMessage(7, list);
        if (ta == null)
        {
            ta = new PlayerData.Task();
            ta.list = new List<PlayerData.Task.Ta>();
            ta.overTask = new Dictionary<int, bool>();
        }
        else
        {
            if (ta.list == null) ta.list = new List<PlayerData.Task.Ta>();
            if (ta.overTask == null) ta.overTask = new Dictionary<int, bool>();
        }
    }
    private void HandleMsg3(object obj)
    {
        if (!isMsg3) return;
        isMsg3 = false;
        TaskMessage3 taskMessage3 = (TaskMessage3)obj;
        for (int i = 0; i < taskMessage3.len; i++)
        {
            int id = taskMessage3.task_id[i];
            if (!NPCTalkData.Instance.descript.ContainsKey(id)) continue;
            if (taskMessage3.gain[i] < taskMessage3.require_num[i] && !CheckTask(id))
            {
                PlayerData.Task.TP tp = taskMessage3.task_type[i] == 1 ? PlayerData.Task.TP.Kill : PlayerData.Task.TP.Task;
                PlayerData.Task.Ta ta = new PlayerData.Task.Ta(id, NPCTalkData.Instance.descript[id].Item1, NPCTalkData.Instance.descript[id].Item2, tp, taskMessage3.require_num[i]);
                AddTask(ta);
                this.ta.overTask[id] = true;
                NPCTalkData.Instance.nowSum[id] = taskMessage3.gain[i];
            }
            else this.ta.overTask[id] = true;
        }
    }

    //改变当前面板上的任务进度
    public void ChangeSum(int id, int nowSum)
    {
        List<object> lst = new List<object>();
        lst.Add(TCPClick.Instance.user_ID);
        lst.Add(id);
        lst.Add(nowSum);
        TCPClick.Instance.SendMessage(12, lst);
        for (int i = 0; i < ta.list.Count; i++)
        {
            int Id = ta.list[i].id;
            if (id == Id)
            {
                if (dic.ContainsKey(id))
                {
                    string str = dic[id].Find("Image/sum").GetComponent<TextMeshProUGUI>().text;
                    string needSum1 = "", needSum = "";
                    for (int j = str.Length - 1; str[j] != ' '; j--)
                        needSum1 += str[j];
                    for (int j = needSum1.Length - 1; j >= 0; j--)
                        needSum += needSum1[j];
                    if(nowSum.ToString() == needSum)
                    {
                        Destroy(dic[id].gameObject);
                        dic.Remove(id);
                        BackPackManager.Instance.AddItem(new BackPackManager.Id(2));
                        InsertBPMessage insertBPMessage = new InsertBPMessage();
                        insertBPMessage.len = 1;
                        insertBPMessage.userID = TCPClick.Instance.user_ID;
                        insertBPMessage.Item_num[0] = 1;
                        insertBPMessage.Item_id[0] = 2;
                        insertBPMessage.exist[0] = 0;
                        List<object> list = new List<object>();
                        list.Add(insertBPMessage.len);
                        list.Add(TCPClick.Instance.user_ID);
                        list.Add(insertBPMessage.Item_id);
                        list.Add(insertBPMessage.Item_num);
                        list.Add(insertBPMessage.exist);
                        TCPClick.Instance.SendMessage(8, list);
                        return;
                    }
                    dic[id].Find("Image/sum").GetComponent<TextMeshProUGUI>().text = nowSum.ToString() + " / " + new string(needSum.Reverse().ToArray());
                }
                else print("未找到实例化的任务id");
                return;
            }
        }
        print("未找到任务id");
    }
    public void Show()
    {
        pos.gameObject.SetActive(true);
    }
    public void Hide()
    {
        pos.gameObject.SetActive(false);
    }
    //任务增加
    public int AddTask(PlayerData.Task.Ta taa)
    {
        int id = taa.id, nowSum = NPCTalkData.Instance.nowSum[id], needSum = taa.needSum;
        if (nowSum == needSum) return 1;
        string name = taa.name;
        string text = taa.description;
        PlayerData.Task.TP tp = taa.type;
        if (this.ta.overTask.ContainsKey(id)) return 1;
        PlayerData.Task.Ta ta = new PlayerData.Task.Ta(id, name, text, tp, needSum);
        this.ta.list.Add(ta);
        this.ta.overTask[id] = true;
        Transform t = GameObject.Instantiate(this.task,pos).transform;
        t.Find("Image/Name").GetComponent<TextMeshProUGUI>().text = name;
        t.Find("Image/text").GetComponent<TextMeshProUGUI>().text = text;
        if (tp == PlayerData.Task.TP.Kill) t.Find("Image/sum").GetComponent<TextMeshProUGUI>().text = nowSum.ToString() + " / " + needSum.ToString();
        dic[id] = t;
        return 0;
    }

    //任务完成后的移除
    public void RemoveTask(int id)
    {
        for (int i = 0; i < ta.list.Count; i++)
        {
            if (ta.list[i].id == id)
            {
                ta.list.RemoveAt(i);
                break;
            }
        }
        //BackPackManager.Instance.Save();
    }
    //检查任务列表是否存在某个任务
    public bool CheckTask(int id)
    {
        for (int i = 0; i < ta.list.Count; i++)
        {
            if (ta.list[i].id == id)
            {
                return true;
            }
        }
        return false;
    }


}
