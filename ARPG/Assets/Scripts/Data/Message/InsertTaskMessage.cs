using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertTaskMessage
{
    public int userID;
    public int taskID;
    public InsertTaskMessage(int userID, int taskID)
    {
        this.userID = userID;
        this.taskID = taskID;
    }
}
