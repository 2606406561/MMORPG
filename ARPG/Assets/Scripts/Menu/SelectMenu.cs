using Google.Protobuf.Collections;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

using UnityEngine.UI;

public class SelectMenu : BaseMenu<SelectMenu>
{
    public int sex;
    public string player_name;
    private List<object> list;
    public TMP_Dropdown dropdown;
    public Button btn, btn2;
    public TextMeshProUGUI text;
    public override void Init()
    {
        btn2.onClick.AddListener(() =>
        {
            Hide();
            Begin.Instance.Show();
        });
        btn.onClick.AddListener(() =>
        {
            List<object> list1 = new List<object>();
            foreach (object o in list) list1.Add(o);
            sex = dropdown.value == 0 ? 0 : 1;
            player_name = text.text;
            char[] name = new char[text.text.Length + (56 - Encoding.UTF8.GetBytes(text.text).Length)];
            for (int i = 0; i < text.text.Length; i++)
                name[i] = text.text[i];
            list.Add(name);
            list.Add(sex);
            TCPClick.Instance.SendMessage(5, list);
            StartCoroutine(Login(list1));
        });
        Hide();
    }
    IEnumerator Login(List<object> l)
    {
        float time = 0;
        while (time < 2f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        LoginMenu.Instance.ok = false;
        TCPClick.Instance.SendMessage(6, l);
    }
    public void Show(List<object> list)
    {
        Show();
        this.list = list;
    }
    public void OnChangeValue()
    {
        if (dropdown.value == 0)
        {
            BackPackManager.Instance.playerData.sex = 0;
            BeginDataManager.Instance.GO = Resources.Load<GameObject>("Player2");
        }
        else
        {
            BackPackManager.Instance.playerData.sex = 1;
            BeginDataManager.Instance.GO = Resources.Load<GameObject>("Player1");
        }
    }
}
