using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class All : BaseMenu<All>
{
    private PlayerData playerData;
    public Dictionary<BackPackManager.Id, TextMeshProUGUI> dic;
    public Dictionary<BackPackManager.Id, Transform> dictr;
    private BackPackManager.Id tp;
    public override void Awake()
    {
        base.Awake();
        dic = new Dictionary<BackPackManager.Id, TextMeshProUGUI>();
        dictr = new Dictionary<BackPackManager.Id, Transform>();
    }
    public void UpdataItem()
    {
        PlayerData.ItemInfo itemInfo = null;
        playerData = BackPackManager.Instance.playerData;
        foreach (PlayerData.ItemInfo item in playerData.list)
        {
            tp = new BackPackManager.Id(item.id);
            if (dic.ContainsKey(tp))
            {
                dic[tp].text = item.sum.ToString();
                if (item.sum == 0)
                {
                    itemInfo = item;
                    Destroy(dictr[tp].gameObject);
                    dictr[tp] = null;
                    dic[tp] = null;
                    BackPack.Instance.ItemHide();
                }
            }
            else
            {
                Transform GO = GameObject.Instantiate(BackPack.Instance.dic[tp], BackPack.Instance.ts1).transform;
                GO.Find("Item").GetComponent<Button>().onClick.AddListener(() =>
                {
                    BackPack.Instance.OnClick(new BackPackManager.Id(item.id));
                });
                dictr.Add(tp, GO);
                TextMeshProUGUI tmp = GO.Find("Item/text").GetComponent<TextMeshProUGUI>();
                tmp.text = item.sum.ToString();
                dic.Add(tp, tmp);
            }
        }
        if (itemInfo != null) playerData.list.Remove(itemInfo);
    }

    public override void Init()
    {
        
    }
}
