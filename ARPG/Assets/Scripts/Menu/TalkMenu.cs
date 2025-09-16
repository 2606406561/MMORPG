using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TalkMenu : BaseMenu<TalkMenu>
{
    public Button btn;
    public TextMeshProUGUI name, context;
    private int sum_now, id;
    private bool ok;
    public override void Init()
    {
        btn.onClick.AddListener(OnClick);
        Hide();
    }

    public void Show1(int id)
    {
        Show();
        this.id = id;
        ok = false;
        name.text = NPCTalkData.Instance.dic[id].name;
        if (BackPackManager.Instance.playerData.task.overTask.ContainsKey(this.id))
        {
            ok = true;
            context.text = "你已经接取了任务";
        }
        else
        {
            sum_now = 0;
            context.text = NPCTalkData.Instance.dic[id].context[sum_now];
        }
    }
    public void OnClick()
    {
        if (ok)
        {
            Hide();
            PlayerController.Instance.isTalk = false;
            return;
        }
        sum_now++;
        if (sum_now == NPCTalkData.Instance.dic[id].context.Length)
        {
            if (NPCTalkData.Instance.dic[id].ta != null)
            {
                TaskManager.Instance.AddTask(NPCTalkData.Instance.dic[id].ta);

                
                List<object> list1 = new List<object>();
                InsertTaskMessage insertTaskMessage = new InsertTaskMessage(TCPClick.Instance.user_ID, NPCTalkData.Instance.dic[id].ta.id);
                list1.Add(insertTaskMessage.userID);
                list1.Add(insertTaskMessage.taskID);
                TCPClick.Instance.SendMessage(9, list1);
            }
            Hide();
            if (NPCTalkData.Instance.dic[id].dict != null)
            {
                InsertBPMessage insertBPMessage = new InsertBPMessage();
                foreach (var i in NPCTalkData.Instance.dic[id].dict)
                {
                    insertBPMessage.Item_id[insertBPMessage.len] = i.Key.id;
                    insertBPMessage.Item_num[insertBPMessage.len] = i.Value;
                    insertBPMessage.exist[insertBPMessage.len] = BackPackManager.Instance.CheckItem(i.Key.id);
                    BackPackManager.Instance.AddItem(i.Key, i.Value);
                    insertBPMessage.len++;
                }

                List<object> list = new List<object>();
                list.Add(insertBPMessage.len);
                list.Add(TCPClick.Instance.user_ID);
                list.Add(insertBPMessage.Item_id);
                list.Add(insertBPMessage.Item_num);
                list.Add(insertBPMessage.exist);
                TCPClick.Instance.SendMessage(8, list);
            }
            PlayerController.Instance.isTalk = false;
            return;
        }
        context.text = NPCTalkData.Instance.dic[id].context[sum_now];

    }
    
    
}
