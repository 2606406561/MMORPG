using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeItemMsg
{
    public int len;
    public int user_ID;
    public int[] Item_id;
    public int[] Item_num;

    public ChangeItemMsg()
    {
        Item_id = new int[50];
        Item_num = new int[50];
    }
}
