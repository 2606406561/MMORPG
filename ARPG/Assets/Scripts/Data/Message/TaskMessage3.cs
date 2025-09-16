using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskMessage3
{
    public int len;
    public int[] task_id;
    public int[] task_type;
    public int[] gain;//已完成数量
    public int[] require_num;//需要数量
    

    public TaskMessage3()
    {
        task_id = new int[50];
        require_num = new int[50];
        task_type = new int[50];
        gain = new int[50];
    }

    public static TaskMessage3 Get(byte[] bytes)
    {
        TaskMessage3 msg = new TaskMessage3();
        int nowPos = 0;
        msg.len = BitConverter.ToInt32(bytes, nowPos);
        nowPos += 4;
        for(int i = 0;i < msg.len;i++)
        {
            msg.task_id[i] = BitConverter.ToInt32(bytes, nowPos); 
            nowPos += 4;
            msg.task_type[i] = BitConverter.ToInt32(bytes, nowPos);
            nowPos += 4;
            msg.gain[i] = BitConverter.ToInt32(bytes, nowPos);
            nowPos += 4;
            msg.require_num[i] = BitConverter.ToInt32(bytes, nowPos);
            nowPos += 4;
        }
        return msg;
    }
}
