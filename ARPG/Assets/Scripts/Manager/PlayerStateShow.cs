using RainbowArt.CleanFlatUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStateShow : MonoBehaviour
{
    private static PlayerStateShow instance;
    public static PlayerStateShow Instance => instance;
    [SerializeField, Header("耐力条")] public ProgressBarSpecial nlt;
    [SerializeField, Header("生命条")] public ProgressBarSpecial hp;
    [SerializeField, Header("耐力消耗速度")] public int speed1;
    [SerializeField, Header("耐力恢复速度")] public int speed2;
    [SerializeField, Header("画布")] public Canvas ui;
    [SerializeField, Header("耐力条位置")] public RectTransform rt;
    [SerializeField, Header("快捷武器栏")] public List<Image> list;
    [SerializeField, Header("对话")] public RectTransform talk;
    private Transform NPC_ts;
    private BackPackManager.Id[] idList;
    public int shift = 100;//体力值
    public bool ok;//判断耐力是否使用到过0
    private float time;
    private Vector3 offset, pos, offset2;
    public int NPC_id;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        GameData.Instance.Init();
        NPCTalkData.Instance.Init();
        offset2 = new Vector3(-1, 2, 0);
        idList = new BackPackManager.Id[10];
        offset = new Vector3(2, 2, 0);//耐力条与人物偏移的位置
        instance = this;
    }

    private void Start()
    {
        talk.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (NPC_ts != null)
        {
            pos = Camera.main.WorldToScreenPoint(NPC_ts.position + offset2);
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(ui.transform as RectTransform, pos, null, out localPos);
            talk.localPosition = localPos;
        }
        nlt.CurrentValue = shift;
        hp.CurrentValue = BackPackManager.Instance.playerData.hp;
        if (shift != 100)
        {
            time += Time.deltaTime;
            ShowNl();
            HandleFollow();
        }
        else
        {
            time = 0;
            HideNl();
        }
        PendingNl();
    }

    //处理耐力消耗,每秒消耗和恢复耐力
    private void PendingNl()
    {
        if (shift == 0) ok = true;
        if (ok)//耐力为0，强制取消疾跑状态并且禁用闪避功能
        {
            PlayerController.Instance.isShift = false;
            if (shift >= 25) ok = false;//恢复到四分之一允许疾跑

        }
        if (CameraController.Instance.flag)//在索敌状态 开始消耗耐力
        {
            //处于闪避状态，禁止恢复耐力
            if (PlayerController.Instance.nowShift) time = 0;
            //处于疾跑状态，开始消耗
            else if (PlayerController.Instance.isShift)
            {
                if (time >= 1f)
                {
                    time -= 1f;
                    if (shift <= speed1)
                    {
                        shift = 0;
                        ok = true;
                    }
                    else shift -= speed1;
                }
            }
            //未处于疾跑和闪避状态，开始恢复
            else
            {
                if (time >= 1f)
                {
                    time -= 1f;
                    if (shift + speed2 >= 100) shift = 100;
                    else shift += speed2;
                }
            }
        }
        //处于闪避状态，禁止恢复耐力
        else if (PlayerController.Instance.nowShift) time = 0;
        else
        {
            if (time >= 1f)
            {
                time -= 1f;
                if (shift + speed2 >= 100) shift = 100;
                else shift += speed2;
            }
        }
    }

    //处理耐力条的位置，使其保持在人物右侧
    private void HandleFollow()
    {
        pos = Camera.main.WorldToScreenPoint(PlayerController.Instance.transform.position + offset);
        Vector2 localPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(ui.transform as RectTransform, pos, null, out localPos);
        rt.localPosition = localPos;
    }
    public void ShowTalk(Transform ts, int id)
    {
        NPC_ts = ts;
        NPC_id = id;
        talk.gameObject.SetActive(true);
    }
    public void HideTalk()
    {
        NPC_ts = null;
        NPC_id = 0;
        talk.gameObject.SetActive(false);
    }
    public void ShowNl()
    {
        nlt.gameObject.SetActive(true);
    }
    public void ShowHp()
    {
        hp.gameObject.SetActive(true);
    }
    public void HideNl()
    {
        nlt.gameObject.SetActive(false);
    }
    public void HideHp()
    {
        hp.gameObject.SetActive(false);
    }
    public void Show()
    {
        ShowNl();
        ShowHp();
    }
    public void Hide()
    {
        HideNl();
        HideHp();
    }
}
