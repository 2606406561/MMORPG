using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class PlayerMessage1
{
    public int sex;
    public string name;
    public int hp;
    public int atk;
    public int piercing;//�Ƽ�
    public float x, y, z;
    public int scene_id;

    public static PlayerMessage1 Get(byte[] by)
    {
        PlayerMessage1 a = new PlayerMessage1();
        int nowPos = 0;
        a.sex = BitConverter.ToInt32(by, nowPos);
        nowPos += 4;
        a.name = Encoding.UTF8.GetString(by, nowPos, 56);
        nowPos += 56;
        a.hp = BitConverter.ToInt32(by, nowPos);
        nowPos += 4;
        a.atk = BitConverter.ToInt32(by, nowPos);
        nowPos += 4;
        a.piercing = BitConverter.ToInt32(by, nowPos);
        nowPos += 4;
        a.x = BitConverter.ToSingle(by, nowPos);
        nowPos += 4;
        a.y = BitConverter.ToSingle(by, nowPos);
        nowPos += 4;
        a.z = BitConverter.ToSingle(by, nowPos);
        nowPos += 4;
        a.scene_id = BitConverter.ToInt32(by, nowPos);
        nowPos += 4;
        return a;
    }
}
