using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackPackMessage2
{
    public int len;//几条数据
    public int[] item_id;
    public int[] item_type;
    public int[] item_num;
    public int[] atk;
    public int[] piercing;//破韧
    public int[] heal_hp;
    public int[] heal_sta;//耐力

    public BackPackMessage2()
    {
        item_id = new int[5];
        item_type = new int[5];
        item_num = new int[5];
        atk = new int[5];
        piercing = new int[5];
        heal_hp = new int[5];
        heal_sta = new int[5];
    }
    public static BackPackMessage2 Get(byte[] bytes)
    {
        BackPackMessage2 backPackMessage2 = new BackPackMessage2();
        int nowPos = 0;
        backPackMessage2.len = BitConverter.ToInt32(bytes, nowPos);
        nowPos += 4;
        for(int i = 0;i < backPackMessage2.len;i++)
        {
            backPackMessage2.item_id[i] = BitConverter.ToInt32(bytes, nowPos);
            nowPos += 4;
            backPackMessage2.item_type[i] = BitConverter.ToInt32(bytes, nowPos);
            nowPos += 4;
            backPackMessage2.item_num[i] = BitConverter.ToInt32(bytes, nowPos);
            nowPos += 4;
            backPackMessage2.atk[i] = BitConverter.ToInt32(bytes, nowPos);
            nowPos += 4;
            backPackMessage2.piercing[i] = BitConverter.ToInt32(bytes, nowPos);
            nowPos += 4;
            backPackMessage2.heal_hp[i] = BitConverter.ToInt32(bytes, nowPos);
            nowPos += 4;
            backPackMessage2.heal_sta[i] = BitConverter.ToInt32(bytes, nowPos);
            nowPos += 4;
        }
        return backPackMessage2;
    }
}
