using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SaveMenu : BaseMenu<SaveMenu>
{
    public GameObject GO;
    public Button btn1, btn2, btn3, btn4, btn5, btn6, btn7;
    public TextMeshProUGUI t1, t2, t3, t4, t5, t6, t7;
    private int id;
    public override void Init()
    {
        Button btn = btn1;
        TextMeshProUGUI tmp = t1;
        GO.SetActive(false);
        for (int i = 0; i < 6; i++)
        {
            int j = i;
            switch (i)
            {
                case 0:
                    btn = btn1;
                    tmp = t1;
                    break;
                case 1:
                    btn = btn2;
                    tmp = t2;
                    break;
                case 2:
                    btn = btn3;
                    tmp = t3;
                    break;
                case 3:
                    btn = btn4;
                    tmp = t4;
                    break;
                case 4:
                    btn = btn5;
                    tmp = t5;
                    break;
                case 5:
                    btn = btn6;
                    tmp = t6;
                    break;
            }
            if (BeginDataManager.Instance.bd.name[i] != "/")
                tmp.text += "\n" + BeginDataManager.Instance.bd.name[i] + "\n" + BeginDataManager.Instance.bd.time[i];
            else tmp.text += "\n乱码";
            btn.onClick.AddListener(() =>
            {
                id = j;
                if (BeginDataManager.Instance.bd.name[id] != "/")
                {
                    BackPackManager.Instance.text = BeginDataManager.Instance.bd.name[id];
                    BackPackManager.Instance.playerData.playerName = BackPackManager.Instance.playerData.playerName;
                    BackPackManager.Instance.playerData.sex = BackPackManager.Instance.playerData.sex;
                    BackPackManager.Instance.Save();
                    switch (id)
                    {
                        case 0:
                            tmp = t1;
                            break;
                        case 1:
                            tmp = t2;
                            break;
                        case 2:
                            tmp = t3;
                            break;
                        case 3:
                            tmp = t4;
                            break;
                        case 4:
                            tmp = t5;
                            break;
                        case 5:
                            tmp = t6;
                            break;
                    }
                    string a = tmp.text.Substring(0, tmp.text.Length - BeginDataManager.Instance.bd.time[id].Length);
                    BeginDataManager.Instance.bd.time[id] = System.DateTime.Now.ToString();
                    a += BeginDataManager.Instance.bd.time[id];
                    tmp.text = a;
                    BeginDataManager.Instance.SaveData();
                }
                else GO.SetActive(true);
            });
        }
        btn7.onClick.AddListener(() =>
        {
            BackPackManager.Instance.text = t7.text;
            BackPackManager.Instance.playerData.playerName = BackPackManager.Instance.playerData.playerName;
            BackPackManager.Instance.playerData.sex = BackPackManager.Instance.playerData.sex;
            BackPackManager.Instance.Save();
            BeginDataManager.Instance.bd.name[id] = t7.text;
            BeginDataManager.Instance.bd.time[id] = System.DateTime.Now.ToString();
            TextMeshProUGUI tmp = t1;
            switch (id)
            {
                case 0:
                    tmp = t1;
                    break;
                case 1:
                    tmp = t2;
                    break;
                case 2:
                    tmp = t3;
                    break;
                case 3:
                    tmp = t4;
                    break;
                case 4:
                    tmp = t5;
                    break;
                case 5:
                    tmp = t6;
                    break;
            }
            string a = tmp.text.Substring(0,tmp.text.Length - 1);
            a +=BeginDataManager.Instance.bd.name[id] + "\n" + BeginDataManager.Instance.bd.time[id];
            tmp.text = a;
            t7.text = "";
            GO.SetActive(false);
            BeginDataManager.Instance.SaveData();
        });
        Hide();
    }
}
