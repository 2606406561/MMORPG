using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Begin : BaseMenu<Begin>
{
    public Button btn, btn2, btn3;
    public override void Awake()
    {
        base.Awake();
        BeginDataManager.Instance.Init();
    }
    public override void Init()
    {

        btn.onClick.AddListener(() =>
        {
            LoadMenu.Instance.Show();
            Hide();
        });
       
        btn2.onClick.AddListener(() =>
        {
            LoginMenu.Instance.Show();
            BeginCamera.Instance.SetPosition();
            Hide();
            
        });
        btn3.onClick.AddListener(() =>
        {
            
            SettingMenu.Instance.Show();
            Hide();
        });
    }

}
