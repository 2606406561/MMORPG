using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginDataManager
{
    private static BeginDataManager instance = new BeginDataManager();
    public static BeginDataManager Instance => instance;
    public BeginData bd;
    public string loadName;//要保存的存档名字
    public GameObject GO;
    public void Init()
    {
        bd = JsonMgr.Instance.LoadData<BeginData>("BeginData");
        if(bd.name == null)
        {
            bd.Init();
            for (int i = 0; i < 6; i++)
            {
                bd.name[i] = "/";
                bd.time[i] = "";
            }
            SaveData();
        }
        
    }

    public void SaveData()
    {
        JsonMgr.Instance.SaveData(bd, "BeginData");
    }
}
