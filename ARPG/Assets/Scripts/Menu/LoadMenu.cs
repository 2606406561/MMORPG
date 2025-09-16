using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMenu : BaseMenu<LoadMenu>
{
    public Button btn1, btn2, btn3, btn4, btn5, btn6,btn7;
    public TextMeshProUGUI t1, t2, t3, t4, t5, t6;
    public override void Init()
    {
        btn7.onClick.AddListener(() =>
        {
            Hide();
            Begin.Instance.Show();
        });
        Button btn = btn1;
        TextMeshProUGUI tmp = t1;
        for(int i = 0;i < 6;i++)
        {
            int j = i;
            switch(i)
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
            {
                tmp.text += "\n" + BeginDataManager.Instance.bd.name[i] + "\n" + BeginDataManager.Instance.bd.time[i];
                btn.onClick.AddListener(() =>
                {
                    BackPackManager.Instance.text = BeginDataManager.Instance.bd.name[j];
                    //‘ÿ»Î”Œœ∑≥°æ∞
                    SceneManager.LoadSceneAsync(1);
                });
            }
            else tmp.text += "\nø’";
        }
        Hide();
    }
}
