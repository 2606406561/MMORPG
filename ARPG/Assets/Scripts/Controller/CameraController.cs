using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraController : MonoBehaviour
{
    private static CameraController instance;
    public static CameraController Instance => instance;
    public Camera ca;
    public bool flag;
    public Transform player;
    private Vector3 offset;
    private Transform enemyPosition;
    public Vector3 pos = new Vector3(-117.5642f, 25.25187f, -231.42f);
    public static Vector3 defaultPos_Spawn = new Vector3(-117.5642f, 25.25187f, -231.42f);
    public static Vector3 defaultPos_Boss = new Vector3(-150.42f, 27.27f, -231.15f);
    private float playerFieldOfView = 70f;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        if (BackPackManager.Instance.playerData.nowPos != Vector3.zero)
        {
            pos = BackPackManager.Instance.playerData.nowPos;
            BackPackManager.Instance.playerData.nowPos = Vector3.zero;
        }
        else
        {
            if (BackPackManager.Instance.playerData.scene_id == 1) pos = defaultPos_Spawn;
            else pos = defaultPos_Boss;
        }
        if (BeginDataManager.Instance.GO == null) BeginDataManager.Instance.GO = BackPackManager.Instance.playerData.sex == 1 ? Resources.Load<GameObject>("Player1") : Resources.Load<GameObject>("Player2");
        player = GameObject.Instantiate(BeginDataManager.Instance.GO, pos, Quaternion.identity).transform;


    }

    private void Start()
    {
        ca.gameObject.SetActive(false);
    }
    void LateUpdate()
    {
        if(enemyPosition != null) {
            Vector3 viewportPos = Camera.main.WorldToViewportPoint(enemyPosition.position);

            // 检查是否在摄像机视野内（z > 0确保在摄像机前方）
            
            if (viewportPos.x > 0 && viewportPos.x < 1 &&
                   viewportPos.y > 0 && viewportPos.y < 1 &&
                   viewportPos.z > 0 && IsEnemyInPlayerFieldOfView())
        {
            flag = true;
            
        }
        else
        {
            flag = false;
        }
        }
        offset = player.up * 4 - player.forward * 4;
        transform.position = Vector3.Lerp(transform.position, player.position + offset, Time.deltaTime * 5f);
        if (!flag)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, player.rotation * Quaternion.AngleAxis(20, Vector3.right), Time.deltaTime * 3f);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(enemyPosition.position - Camera.main.transform.position), Time.deltaTime * 3f);
        }
    }
    private bool IsEnemyInPlayerFieldOfView()
    {
        // 计算：玩家正前方的方向向量（忽略Y轴，避免上下抬头影响左右视野）
        Vector3 playerForward = player.forward;
        playerForward.y = 0; // 锁定Y轴，只判断水平方向的视野
        playerForward.Normalize();

        // 计算：玩家到怪物的方向向量（同样忽略Y轴）
        Vector3 playerToEnemy = enemyPosition.position - player.position;
        playerToEnemy.y = 0; // 锁定Y轴，与玩家朝向保持同一平面
        playerToEnemy.Normalize();

        // 计算两个向量的夹角（点积公式）
        float angle = Vector3.Angle(playerForward, playerToEnemy);

        // 若夹角小于“玩家视野角度的一半”，则在视野内
        return angle < playerFieldOfView / 2;
    }

    public void Change()
    {
        gameObject.SetActive(false);
       ca.gameObject.SetActive(true);
    }


    public void Enemy(Transform ts)
    {
        enemyPosition = ts;
    }
}
