using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameObjectManager : MonoBehaviour
{
    private static GameObjectManager instance;
    public static GameObjectManager Instance => instance;
    private Dictionary<int, GameObjectBeh> dic;
    private Vector3 vector3, vector33;
    private bool isMsg11;
    private Dictionary<int, bool> isMsg1;
    private int idx1, idx2;
    private Dictionary<int, PlayerMessage1> dicP;
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    void Start()
    {
        isMsg1 = new Dictionary<int, bool>();
        dicP = new Dictionary<int, PlayerMessage1>();
        dic = new Dictionary<int, GameObjectBeh>();
        vector3 = Vector3.zero;
        vector33 = Vector3.zero;
        EventSystemManager.Instance.Register(11, HandleMsg11);
        EventSystemManager.Instance.Register(1, HandleMsg1);
    }
    void OnDestroy()
    {
        EventSystemManager.Instance.Unregister(11, HandleMsg11);
        EventSystemManager.Instance.Unregister(1, HandleMsg1);
    }
    private void HandleMsg11(object obj)
    {
        GameObjectMsg11 gameObjectMsg11 = obj as GameObjectMsg11;
        if (gameObjectMsg11.userID == TCPClick.Instance.user_ID) return;
        vector3.x = gameObjectMsg11.x;
        vector3.y = gameObjectMsg11.y;
        vector3.z = gameObjectMsg11.z;
        vector33.x = gameObjectMsg11.rotaX;
        vector33.y = gameObjectMsg11.rotaY;
        vector33.z = gameObjectMsg11.rotaZ;
        if (dic.ContainsKey(gameObjectMsg11.userID) && dic[gameObjectMsg11.userID] != null)
        {
            if(gameObjectMsg11.beh == 14)
            {
                Destroy(dic[gameObjectMsg11.userID].gameObject);
                return;
            }
            dic[gameObjectMsg11.userID].SetPosition(vector3);
            dic[gameObjectMsg11.userID].SetRotation(new Quaternion(gameObjectMsg11.rotaX, gameObjectMsg11.rotaY, gameObjectMsg11.rotaZ, gameObjectMsg11.rotaW));
            dic[gameObjectMsg11.userID].SetAnimator(gameObjectMsg11.beh);
            dic[gameObjectMsg11.userID].SetWeapon(gameObjectMsg11.weaponId);
        }
        else
        {
            StartCoroutine(Instaite(gameObjectMsg11.userID, gameObjectMsg11.name, vector3, new Quaternion(gameObjectMsg11.rotaX, gameObjectMsg11.rotaY, gameObjectMsg11.rotaZ, gameObjectMsg11.rotaW), gameObjectMsg11.beh, gameObjectMsg11.weaponId));
        }

    }
    private void HandleMsg1(object obj)
    {
        if (isMsg1.ContainsKey(idx1) && !isMsg1[idx1]) return;
        PlayerMessage1 playerMessage1 = obj as PlayerMessage1;
        dicP[idx1] = playerMessage1;
        idx1++;
    }
    private IEnumerator Instaite(int id, string name, Vector3 vector3, Quaternion q, int beh, int weaponId)
    {
        List<object> list = new List<object>();
        list.Add(1);
        list.Add(id);
        int nowIdx = idx2;
        isMsg1[nowIdx] = true;
        idx2++;
        TCPClick.Instance.SendMessage(7, list);
        while (!dicP.ContainsKey(nowIdx))
            yield return null;
        if (dic.ContainsKey(id) && dic[id] != null) yield break;
        print("Ö´ÐÐ");
        GameObject GO = GameObject.Instantiate(dicP[nowIdx].sex == 1 ? GameData.Instance.sex_1 : GameData.Instance.sex_0, vector3, Quaternion.identity);
        GO.name = name;
        GameObjectBeh ga = GO.AddComponent<GameObjectBeh>();
        dic[id] = ga;
        ga.SetPosition(vector3);
        ga.SetRotation(q);
        ga.SetAnimator(beh);
        ga.SetWeapon(weaponId);
    }
}
