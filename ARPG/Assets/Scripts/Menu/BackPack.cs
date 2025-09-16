using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BackPack : BaseMenu<BackPack>
{
    [SerializeField, Header("全部视图")] public GameObject go1;
    [SerializeField, Header("武器视图")] public GameObject go2;
    [SerializeField, Header("消耗视图")] public GameObject go3;
    [SerializeField, Header("其它视图")] public GameObject go4;
    [SerializeField, Header("全部按钮")] public Button btn1;
    [SerializeField, Header("武器按钮")] public Button btn2;
    [SerializeField, Header("消耗按钮")] public Button btn3;
    [SerializeField, Header("其它按钮")] public Button btn4;
    [SerializeField, Header("退出按钮")] public Button btn5;
    [SerializeField, Header("全部视图父节点")] public Transform ts1;
    [SerializeField, Header("武器视图父节点")] public Transform ts2;
    [SerializeField, Header("消耗视图父节点")] public Transform ts3;
    [SerializeField, Header("其它视图父节点")] public Transform ts4;
    [SerializeField, Header("介绍页面")] public GameObject go5;
    [SerializeField, Header("物品姓名")] public TextMeshProUGUI itemName;
    [SerializeField, Header("物品介绍")] public TextMeshProUGUI itemText;
    [SerializeField, Header("物品图片")] public Image itemImage;
    [SerializeField, Header("物品丢弃按钮")] public Button itemBtn1;
    [SerializeField, Header("物品使用按钮")] public Button itemBtn2;
    [SerializeField, Header("物品介绍关闭按钮")] public Button itemBtn3;
    [SerializeField, Header("任务面板")] public GameObject go6;

    public Dictionary<int, int> type;//根据物品id返回种类id 1为武器 2为消耗品 3为其它

    private GameObject now;
    public Dictionary<BackPackManager.Id, GameObject> dic;
    public Dictionary<BackPackManager.Id, Sprite> dicImage;
    private bool msg1, msg2;
    private PlayerMessage1 playerMessage1;
    private BackPackMessage2 backPackMessage2;
    public override void Awake()
    {
        base.Awake();

        type = new Dictionary<int, int>();
        type.Add(1, 1);
        type.Add(2, 1);
        type.Add(3, 2);
        type.Add(4, 2);
        type.Add(5, 2);
        type.Add(6, 3);

        dic = new Dictionary<BackPackManager.Id, GameObject>();
        dic.Add(new BackPackManager.Id(1), Resources.Load<GameObject>("Prefabs/rld"));
        dic.Add(new BackPackManager.Id(2), Resources.Load<GameObject>("Prefabs/phj"));
        dic.Add(new BackPackManager.Id(3), Resources.Load<GameObject>("Prefabs/hfys"));
        dic.Add(new BackPackManager.Id(4), Resources.Load<GameObject>("Prefabs/cjhfys"));
        dic.Add(new BackPackManager.Id(5), Resources.Load<GameObject>("Prefabs/cjnlys"));
        dic.Add(new BackPackManager.Id(6), Resources.Load<GameObject>("Prefabs/tzjz"));

        dicImage = new Dictionary<BackPackManager.Id, Sprite>();
        dicImage.Add(new BackPackManager.Id(1), Resources.Load<Sprite>("Image/rld"));
        dicImage.Add(new BackPackManager.Id(2), Resources.Load<Sprite>("Image/phj"));
        dicImage.Add(new BackPackManager.Id(3), Resources.Load<Sprite>("Image/hfys"));
        dicImage.Add(new BackPackManager.Id(4), Resources.Load<Sprite>("Image/cjhfys"));
        dicImage.Add(new BackPackManager.Id(5), Resources.Load<Sprite>("Image/cjnlys"));
        dicImage.Add(new BackPackManager.Id(6), Resources.Load<Sprite>("Image/tzjz"));
    }


    public override void Show()
    {
        base.Show();
        UpdateItem();
        go6.SetActive(false);
        Time.timeScale = 0f;
        PlayerStateShow.Instance.Hide();
    }

    public override void Hide()
    {
        base.Hide();
        go6.SetActive(true);
        PlayerController.Instance.isB = false;
        EventSystemManager.Instance.Publish('c');
        Time.timeScale = 1f;
    }

    //更新每个视图的实例
    private void UpdateItem()
    {
        if (now == go1) All.Instance.UpdataItem();
        else if (now == go2) WeaponMenu.Instance.UpdataItem();
        else if (now == go3) ConsumeMenu.Instance.UpdataItem();
        else ObjectMenu.Instance.UpdataItem();
    }

    public override void Init()
    {
        UnityAction<object> unityAction1 = HandleMsg1, unityAction2 = HandleMsg2;
        EventSystemManager.Instance.Register(1, unityAction1);
        EventSystemManager.Instance.Register(2, unityAction2);
        itemBtn3.onClick.AddListener(() =>
        {
            ItemHide();
        });
        ItemHide();
        btn1.onClick.AddListener(() =>
        {
            if (now != go1)
            {
                now.SetActive(false);
                now = go1;
                UpdateItem();
                now.SetActive(true);
            }
        });
        btn2.onClick.AddListener(() =>
        {
            if (now != go2)
            {
                now.SetActive(false);
                now = go2;
                UpdateItem();
                now.SetActive(true);
            }
        });
        btn3.onClick.AddListener(() =>
        {
            if (now != go3)
            {
                now.SetActive(false);
                now = go3;
                UpdateItem();
                now.SetActive(true);
            }
        });
        btn4.onClick.AddListener(() =>
        {
            if (now != go4)
            {
                now.SetActive(false);
                now = go4;
                UpdateItem();
                now.SetActive(true);
            }
        });
        btn5.onClick.AddListener(() =>
        {
            ItemHide();
            PlayerStateShow.Instance.Show();
            Hide();
        });
        now = go1;
        Hide();
    }


    private void HandleMsg1(object obj)
    {
        if (!msg1) return;
        msg1 = false;
        playerMessage1 = obj as PlayerMessage1;
        BackPackManager.Instance.playerData.hp = playerMessage1.hp;
        BackPackManager.Instance.playerData.damage = playerMessage1.atk;
        BackPackManager.Instance.playerData.piercing = playerMessage1.piercing;
    }

    private void HandleMsg2(object obj)
    {
        if (!msg2) return;
        msg2 = false;
        backPackMessage2 = obj as BackPackMessage2;
    }
    public void AddItem(BackPackManager.Id Tp)
    {
        Transform GO = GameObject.Instantiate(dic[Tp]).transform;
        if (now == go1) GO.SetParent(ts1);
        else if (now == go2) GO.SetParent(ts2);
        else if (now == go3) GO.SetParent(ts3);
        else GO.SetParent(ts4);
    }

    public void ItemShow()
    {
        go5.SetActive(true);
    }

    public void ItemHide()
    {
        go5.SetActive(false);
    }

    //物品被点击后的逻辑
    public void OnClick(BackPackManager.Id td)
    {
        ItemShow();
        BackPackManager.Context a = BackPackManager.Instance.dic[td];
        itemName.text = a.name;
        itemText.text = a.text;
        itemImage.sprite = dicImage[td];
        AskBackPackMsg(td);
    }

    private void AskBackPackMsg(BackPackManager.Id td)
    {
        switch (type[td.id])
        {
            case 1:
                itemBtn2.onClick.RemoveAllListeners();
                itemBtn2.onClick.AddListener(() =>
                {
                    //发送装备武器消息
                    StartCoroutine(UseItem(td.id, 1));
                });
                break;
            case 2:
                itemBtn2.onClick.RemoveAllListeners();
                itemBtn2.onClick.AddListener(() =>
                {
                    //发送使用消耗品消息
                    StartCoroutine(UseItem(td.id, 2));
                });
                break;
                //其它
        }
    }
    private IEnumerator UseItem(int id, int tp)
    {
        //List<object> list = new List<object>();
        //list.Add(2);
        //list.Add(TCPClick.Instance.user_ID);
        //msg2 = true;
        //TCPClick.Instance.SendMessage(7, list);
        //while (backPackMessage2 == null)
        //{
        //    yield return null;
        //}

        //list = new List<object>();
        //list.Add(1);
        //list.Add(TCPClick.Instance.user_ID);
        //msg1 = true;
        //TCPClick.Instance.SendMessage(7, list);
        //while (playerMessage1 == null)
        //{
        //    yield return null;
        //}
        //BackPackManager.Instance.playerData.hp = playerMessage1.hp;
        //BackPackManager.Instance.playerData.damage = playerMessage1.atk;
        //BackPackManager.Instance.playerData.piercing = playerMessage1.piercing;
        //for (int i = 0; i < backPackMessage2.len; i++)
        //{
        //    if (backPackMessage2.item_id[i] == id)
        //    {
                if (tp == 1)//武器
                {
                    //int dmg = backPackMessage2.atk[i], pie = backPackMessage2.piercing[i];
                    int dmg = GameData.Instance.objectData[id][0],pie = GameData.Instance.objectData[id][1];
                    //TCPClick.Instance.SendMessage(10, ChangePlayerMsg10.Send(TCPClick.Instance.user_ID, 0, -playerMessage1.atk + dmg, -playerMessage1.piercing + pie));
                    TCPClick.Instance.SendMessage(10, ChangePlayerMsg10.Send(TCPClick.Instance.user_ID, 0, -BackPackManager.Instance.playerData.damage + dmg, -BackPackManager.Instance.playerData.piercing + pie));
                    PlayerController.Instance.SetWeapon(id, dmg, pie);
                }
                else if (tp == 2)//消耗品
                {
                    //int num = backPackMessage2.item_num[i], hp = backPackMessage2.heal_hp[i];
                    int hp = GameData.Instance.objectData[id][2];
                    TCPClick.Instance.SendMessage(10, ChangePlayerMsg10.Send(TCPClick.Instance.user_ID, 100 - BackPackManager.Instance.playerData.hp >= hp ? hp : 100 - BackPackManager.Instance.playerData.hp, 0, 0));
                    List<object> l = new List<object>();
                    l.Add(1);
                    l.Add(TCPClick.Instance.user_ID);
                    int[] item_id = new int[50], item_num = new int[50];
                    item_id[0] = id;
                    item_num[0] = -1;
                    l.Add(item_id);
                    l.Add(item_num);
                    TCPClick.Instance.SendMessage(11, l);//修改物品数量消息
                    BackPackManager.Instance.playerData.hp = Mathf.Min(BackPackManager.Instance.playerData.hp + hp, 100);
                    BackPackManager.Instance.SetItem(new BackPackManager.Id(id), -1);
                    UpdateItem();
                }
        //        break;
        //    }
        //}
        playerMessage1 = null;
        backPackMessage2 = null;
        yield return null;
    }
}
