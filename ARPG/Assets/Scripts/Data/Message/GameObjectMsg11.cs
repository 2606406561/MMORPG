using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class GameObjectMsg11
{
    public int userID;
    public string name;
    public int hp;
    public float x, y, z, rotaX, rotaY, rotaZ, rotaW;
    public int beh;
    public int weaponId;


    public static GameObjectMsg11 Get(byte[] bytes)
    {
        int nowPos = 0;
        GameObjectMsg11 gameObjectMsg11 = new GameObjectMsg11();
        gameObjectMsg11.userID = BitConverter.ToInt32(bytes, nowPos);
        nowPos += 4;
        gameObjectMsg11.name = Encoding.UTF8.GetString(bytes, nowPos, 56);
        nowPos += 56;
        gameObjectMsg11.hp = BitConverter.ToInt32(bytes, nowPos);
        nowPos += 4;
        gameObjectMsg11.x = BitConverter.ToSingle(bytes, nowPos);
        nowPos += 4;
        gameObjectMsg11.y = BitConverter.ToSingle(bytes, nowPos);
        nowPos += 4;
        gameObjectMsg11.z = BitConverter.ToSingle(bytes, nowPos);
        nowPos += 4;
        gameObjectMsg11.rotaX = BitConverter.ToSingle(bytes, nowPos);
        nowPos += 4;
        gameObjectMsg11.rotaY = BitConverter.ToSingle(bytes, nowPos);
        nowPos += 4;
        gameObjectMsg11.rotaZ = BitConverter.ToSingle(bytes, nowPos);
        nowPos += 4;
        gameObjectMsg11.rotaW = BitConverter.ToSingle(bytes, nowPos);
        nowPos += 4;
        gameObjectMsg11.beh = BitConverter.ToInt32(bytes, nowPos);
        nowPos += 4;
        gameObjectMsg11.weaponId = BitConverter.ToInt32(bytes, nowPos);
        nowPos += 4;

        return gameObjectMsg11;
    }
}
