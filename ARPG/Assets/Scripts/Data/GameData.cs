using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    private static GameData instance = new GameData();
    public static GameData Instance => instance;
    [SerializeField, Header("怪物出生点位")] public Vector3 enemyPos;
    [SerializeField, Header("怪物本体")] public GameObject Enemy;
    public AudioClip hit0, hit1, hit2, hit3;
    public AudioClip w0_0,w0_1,w0_2,w0_3,w0_4;
    public AudioClip w1_0, w1_1, w1_2;
    public GameObject sex_0, sex_1;
    public Dictionary<int, int[]> objectData = new Dictionary<int, int[]>();
    public void Init()
    {
        objectData.Add(1, new int[4] { 5, 10, 0, 0 });
        objectData.Add(3,new int[4] { 0, 0, 20, 0 });
        objectData.Add(4, new int[4] { 0, 0, 100, 0 });
        sex_1 = Resources.Load<GameObject>("player1C");
        sex_0 = Resources.Load<GameObject>("player2C");
        hit0 = Resources.Load<AudioClip>("Sound/Hit_0");
        hit1 = Resources.Load<AudioClip>("Sound/Hit_1");
        hit2 = Resources.Load<AudioClip>("Sound/Hit_2");
        hit3 = Resources.Load<AudioClip>("Sound/Hit_3");
        w0_0 = Resources.Load<AudioClip>("Sound/swing-sword");
        w0_1 = Resources.Load<AudioClip>("Sound/swing-sword2");
        w0_2 = Resources.Load<AudioClip>("Sound/SwordWave_2");
        w0_3 = Resources.Load<AudioClip>("Sound/SwordWave_3");
        w0_4 = Resources.Load<AudioClip>("Sound/SwordWave_4");
        w1_0 = Resources.Load<AudioClip>("Sound/GS/GS0");
        w1_1 = Resources.Load<AudioClip>("Sound/GS/GS1");
        w1_2 = Resources.Load<AudioClip>("Sound/GS/GS2");
    }
}
