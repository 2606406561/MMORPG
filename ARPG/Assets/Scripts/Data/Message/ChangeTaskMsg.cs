using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTaskMsg
{
    public int user_ID;
    public int task_id;
    public int gain;//目前完成数量

    public ChangeTaskMsg(int user_ID, int task_id, int gain)
    {
        this.user_ID = user_ID;
        this.task_id = task_id;
        this.gain = gain;
    }
}
