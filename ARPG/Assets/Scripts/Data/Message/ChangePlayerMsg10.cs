using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayerMsg10
{
    public int user_ID;
    public int hp;
    public int atk;

    public static List<object> Send(int user_ID, int hp, int atk, int piercing)
    {
        List<object> list = new List<object>();
        list.Add(user_ID);
        list.Add(hp);
        list.Add(atk);
        list.Add(piercing);
        return list;
    }

    public ChangePlayerMsg10(int user_ID, int hp, int atk, int piercing, float x, float y, float z, int scene_id)
    {
        this.user_ID = user_ID;
        this.hp = hp;
        this.atk = atk;
    }
}
