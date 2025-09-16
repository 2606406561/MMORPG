using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SpawnMenu : BaseMenu<SpawnMenu>
{
    public Button btn;
    public override void Init()
    {
        btn.onClick.AddListener(() =>
        {
            if(BackPackManager.Instance.playerData.scene_id != 1)
            {
                BackPackManager.Instance.playerData.scene_id = 1;
                ChangeMenu.Instance.Show(1,true);
            }
            else
            {
                PlayerController.Instance.NoDie();
            }
                BackPackManager.Instance.playerData.hp = 100;//当前要从服务器获取血量信息
            Hide();
        });
        Hide();
    }
}
