using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertItemMessage8
{
    public int len;
    public int user_ID;
    public int[] item_id;
    public int[] item_num;
    public int[] exist;

    public InsertItemMessage8()
    {
        item_id = new int[50];
        item_num = new int[50];
        exist = new int[50];
    }
}
