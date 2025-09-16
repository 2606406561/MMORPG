using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertBPMessage
{
    public int userID;
    public int len;
    public int[] Item_id;
    public int[] Item_num;
    public int[] exist;

    public InsertBPMessage()
    {
        Item_id = new int[50];
        Item_num = new int[50];
        exist = new int[50];
    }

}
