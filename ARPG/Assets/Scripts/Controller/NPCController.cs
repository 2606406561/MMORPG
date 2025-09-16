using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour
{
    public Transform f_ts;
    public int id;
    private string name;
    public GameObject GO_Name;
    private float time;
    private float dis;
    private void Start()
    {
        name = NPCTalkData.Instance.dic[id].name;
    }
    private void Update()
    {
        time += Time.deltaTime;
        if (time > 1)
        {
            time -= 1;
            dis = Vector3.Distance(CameraController.Instance.player.position, f_ts.position);
            if (dis <= 20f)
            {
                Show();
                if (dis <= 4f)
                {
                    PlayerStateShow.Instance.ShowTalk(this.transform, id);
                }
                else PlayerStateShow.Instance.HideTalk();
            }
            else Hide();
        }
    }
    public void Show()
    {
        GO_Name.SetActive(true);
    }
    public void Hide()
    {
        GO_Name.SetActive(false);
    }
}
