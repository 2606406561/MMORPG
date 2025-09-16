using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Text;
public class LoginMenu : BaseMenu<LoginMenu>
{
    public TextMeshProUGUI user_name, user_pwd;
    public Button login_btn, register_btn;
    public bool ok, msg1, msg2;
    public override void Init()
    {
        UnityAction<object> unityAction1 = HandleMsg1, unityAction2 = HandleMsg45, unityAction3 = HandleMsg2;
        EventSystemManager.Instance.Register(1, unityAction1);
        EventSystemManager.Instance.Register(4, unityAction2);
        EventSystemManager.Instance.Register(5, unityAction2);
        EventSystemManager.Instance.Register(2, unityAction3);
        login_btn.onClick.AddListener(() =>
        {
            ok = false;
            List<object> list = new List<object>();
            char[] name = new char[user_name.text.Length + (56 - Encoding.UTF8.GetBytes(user_name.text).Length)], pwd = new char[user_pwd.text.Length + (40 - Encoding.UTF8.GetBytes(user_pwd.text).Length)];
            for (int i = 0; i < user_name.text.Length; i++)
                name[i] = user_name.text[i];
            for (int i = 0; i < user_pwd.text.Length; i++)
                pwd[i] = user_pwd.text[i];
            list.Add(name);
            string pwd1 = new string(pwd);
            byte[] bytes = Encoding.UTF8.GetBytes(pwd1);
            list.Add(bytes);
            TCPClick.Instance.SendMessage(6, list);
            //发送登录消息
            //SelectMenu.Instance.Show();
        });
        register_btn.onClick.AddListener(() =>
        {
            ok = true;
            List<object> list = new List<object>();
            char[] name = new char[user_name.text.Length + (56 - Encoding.UTF8.GetBytes(user_name.text).Length)], pwd = new char[user_pwd.text.Length + (40 - Encoding.UTF8.GetBytes(user_pwd.text).Length)];
            for (int i = 0; i < user_name.text.Length; i++)
                name[i] = user_name.text[i];
            for (int i = 0; i < user_pwd.text.Length; i++)
                pwd[i] = user_pwd.text[i];
            list.Add(name);
            string pwd1 = new string(pwd);
            byte[] bytes = Encoding.UTF8.GetBytes(pwd1);
            list.Add(bytes);
            SelectMenu.Instance.Show(list);
            Hide();
            //发送注册消息
        });
        Hide();
    }


    private void HandleMsg45(object obj)
    {
        RegisterLoginMessage56 registerLoginMessage45 = obj as RegisterLoginMessage56;
        if (registerLoginMessage45.flag == 0)
        {
            if (!ok) Debug.Log("账户或密码错误");
            else Debug.Log("该账户已存在");
            return;
        }
        TCPClick.Instance.user_ID = registerLoginMessage45.user_ID;
        if (ok)//注册时发送初始化修改
        {
            return;
        }
        List<object> list = new List<object>();
        list.Add(1);
        list.Add(registerLoginMessage45.user_ID);
        msg1 = true;
        TCPClick.Instance.SendMessage(7, list);
        list = new List<object>();
        list.Add(2);
        list.Add(registerLoginMessage45.user_ID);
        msg2 = true;
        TCPClick.Instance.SendMessage(7, list);
        //查询玩家信息后初始化
    }

    //查询玩家信息回调
    private void HandleMsg1(object obj)
    {
        if (!msg1) return;
        msg1 = false;
        PlayerMessage1 playerMessage1 = obj as PlayerMessage1;
        BackPackManager.Instance.playerData.hp = playerMessage1.hp;
        BackPackManager.Instance.playerData.damage = playerMessage1.atk;
        BackPackManager.Instance.playerData.piercing = playerMessage1.piercing;
        BackPackManager.Instance.playerData.sex = playerMessage1.sex;
        BackPackManager.Instance.playerData.playerName = playerMessage1.name;
        print("名字:" + playerMessage1.name);
        if (playerMessage1.scene_id != 1)
        {
            BackPackManager.Instance.playerData.scene_id = 1;
            BackPackManager.Instance.playerData.nowPos = CameraController.defaultPos_Spawn;
        }
        else
        {
            BackPackManager.Instance.playerData.scene_id = playerMessage1.scene_id;
            BackPackManager.Instance.playerData.nowPos = new Vector3(-117.5642f, 25.25187f, -231.42f);
            //BackPackManager.Instance.playerData.nowPos = new Vector3(playerMessage1.x, playerMessage1.y, playerMessage1.z);
        }
        ChangeMenu.Instance.Show(1,true);
    }


    /// <summary>
    /// 查询玩家背包回调
    /// </summary>
    /// <param name="obj">回调的信息</param> <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    private void HandleMsg2(object obj)
    {
        if (!msg2) return;
        msg2 = false;
        BackPackMessage2 backPackMessage2 = obj as BackPackMessage2;
        BackPackManager.Id id = new BackPackManager.Id(1);
        for (int i = 0; i < backPackMessage2.len; i++)
        {
            id.id = backPackMessage2.item_id[i];
            BackPackManager.Instance.AddItem(id, backPackMessage2.item_num[i]);
        }

    }

}
