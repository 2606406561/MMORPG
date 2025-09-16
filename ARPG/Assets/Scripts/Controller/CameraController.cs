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

            // ����Ƿ����������Ұ�ڣ�z > 0ȷ���������ǰ����
            
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
        // ���㣺�����ǰ���ķ�������������Y�ᣬ��������̧ͷӰ��������Ұ��
        Vector3 playerForward = player.forward;
        playerForward.y = 0; // ����Y�ᣬֻ�ж�ˮƽ�������Ұ
        playerForward.Normalize();

        // ���㣺��ҵ�����ķ���������ͬ������Y�ᣩ
        Vector3 playerToEnemy = enemyPosition.position - player.position;
        playerToEnemy.y = 0; // ����Y�ᣬ����ҳ��򱣳�ͬһƽ��
        playerToEnemy.Normalize();

        // �������������ļнǣ������ʽ��
        float angle = Vector3.Angle(playerForward, playerToEnemy);

        // ���н�С�ڡ������Ұ�Ƕȵ�һ�롱��������Ұ��
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
