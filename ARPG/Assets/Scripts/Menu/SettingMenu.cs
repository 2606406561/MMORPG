using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : BaseMenu<SettingMenu>
{
    [SerializeField, Header("音乐条")] public Slider slider1;
    [SerializeField, Header("音效条")] public Slider slider2;
    public Button btn;
    public override void Init()
    {
        slider1.value = BeginDataManager.Instance.bd.musicValue * 0.01f;
        slider2.value = BeginDataManager.Instance.bd.soundValue * 0.01f;
        btn.onClick.AddListener(() =>
        {
            Hide();
            Begin.Instance.Show();
        });
        Hide();
    }
    private void Update()
    {
        BeginDataManager.Instance.bd.musicValue = slider1.value * 100;
        BeginDataManager.Instance.bd.soundValue = slider2.value * 100;
    }
}
