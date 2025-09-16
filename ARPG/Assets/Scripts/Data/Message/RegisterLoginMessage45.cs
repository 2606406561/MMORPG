using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegisterLoginMessage56
{
    public int user_ID;
    public int flag;

    public static RegisterLoginMessage56 Get(byte[] bytes)
    {
        RegisterLoginMessage56 registerLoginMessage45 = new RegisterLoginMessage56();
        int nowPos = 0;
        registerLoginMessage45.user_ID = BitConverter.ToInt32(bytes, nowPos);
        nowPos += 4;
        registerLoginMessage45.flag = BitConverter.ToInt32(bytes, nowPos);
        nowPos += 4;
        return registerLoginMessage45;
    }

}
