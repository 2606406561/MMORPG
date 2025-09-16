using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
[Serializable]
public class BossBlackBoard : FSM.BlackBoard
{
    //可在外配置的数据
    [SerializeField, Header("怪物本体")] public GameObject GO;
    [SerializeField, Header("怪物的id")] public int id;
    [SerializeField, Header("怪物的生命值")] public int hp;
    [SerializeField, Header("动画控制器")] public Animator animator;
    [SerializeField, Header("怪物的破韧时间")] public float followTime;
    [SerializeField, Header("怪物的韧性条最大值")] public int value;
    [SerializeField, Header("怪物的武器")] public Transform weapon;
    [SerializeField, Header("怪物要追踪的玩家")] public Transform player;
    [SerializeField, Header("射线发射点")] public Transform ray;
    [SerializeField, Header("怪物的运动")] public CharacterController controller;
}
public class IdleState : IState
{
    private BossBlackBoard board;
    private FSM fsm;
    private float time, dis;
    public IdleState(FSM fsm,FSM.BlackBoard board)
    {
        this.fsm = fsm;
        this.board = board as BossBlackBoard;
    }
    public void OnEnter()
    {
        board.animator.SetFloat("x", 0);
        board.animator.SetFloat("y", 0);
    }

    public void OnExit()
    {
        time = 0;
    }

    public void OnUpdate()
    {
        time += Time.deltaTime;
        dis = Vector3.Distance(board.player.position, board.animator.transform.position);
        
        if (dis <= 2f)
        {
            if (!Physics.Raycast(new Ray(board.ray.position, board.player.position - board.ray.position), dis, 1 << LayerMask.NameToLayer("Ground")))
            {
                board.animator.transform.rotation = Quaternion.LookRotation(board.player.position - board.animator.transform.position);
                fsm.SwitchState(StateType.Attack);
            }
        }
        else if (dis <= 8f)
        {
            fsm.SwitchState(StateType.Move);
        }
        if (time >= 4f)
        {
            fsm.SwitchState(StateType.Patrol);

        }
    }
}

public class MoveState : IState
{
    private BossBlackBoard board;
    private FSM fsm;
    private float time, dis;
    private Quaternion q;
    public MoveState(FSM fsm, FSM.BlackBoard board)
    {
        this.fsm = fsm;
        this.board = board as BossBlackBoard;
    }
    public void OnEnter()
    {

    }
    public void OnExit()
    {
        board.animator.SetFloat("x", 0);
        board.animator.SetFloat("y", 0);
    }

    public void OnUpdate()
    {
        dis = Vector3.Distance(board.player.position, board.animator.transform.position);
        time += Time.deltaTime;
        if (dis <= 2f)
        {
            if (!Physics.Raycast(new Ray(board.ray.position, board.player.position - board.ray.position), dis, 1 << LayerMask.NameToLayer("Ground")))
            {
                board.animator.transform.rotation = Quaternion.LookRotation(board.player.position - board.animator.transform.position);
                fsm.SwitchState(StateType.Attack);
            }
        }
        else if (dis <= 8f)
        {
            if (!Physics.Raycast(new Ray(board.ray.position, board.player.position - board.ray.position), dis, 1 << LayerMask.NameToLayer("Ground")))
            {
                board.animator.SetFloat("y", 0.5f);
                board.controller.SimpleMove(board.controller.transform.forward * 4f);
                board.animator.transform.rotation = Quaternion.Lerp(board.animator.transform.rotation, Quaternion.LookRotation(board.player.position - board.animator.transform.position), Time.deltaTime * 2f);
            }
            else
            {
                board.animator.SetFloat("y", 0f);
            }
        }
        else fsm.SwitchState(StateType.Patrol);
    }

}

public class AttackState : IState
{
    private BossBlackBoard board;
    private FSM fsm;
    private int sum;
    private int nowSum;
    public bool ok;
    private float time;
    public AttackState(FSM fsm, FSM.BlackBoard board)
    {
        this.fsm = fsm;
        this.board = board as BossBlackBoard;
    }
    public void OnEnter()
    {
        sum = Random.Range(1, 4);
        nowSum = 1;
        board.animator.transform.rotation = Quaternion.LookRotation(board.player.position - board.animator.transform.position);
        board.animator.Play("Attack" + nowSum.ToString(), 0, 0f);
    }

    public void OnExit()
    {
        sum = 0;
        nowSum = 0;
        time = 0;
    }

    public void OnUpdate()
    {
        time += Time.deltaTime;
        if (time >= 5f)
        {
            fsm.SwitchState(StateType.Move);
            return;
        }
        if (ok)
        {
            ok = false;
            if (nowSum != sum)
            {
                nowSum++;
                board.animator.transform.rotation = Quaternion.LookRotation(board.player.position - board.animator.transform.position);
                board.animator.Play("Attack" + nowSum.ToString(), 0, 0f);
            }
            else fsm.SwitchState(StateType.Move);
        }
    }
}

public class StopState : IState
{
    private BossBlackBoard board;
    private FSM fsm;
    private float nowTime;//破韧后开始计时
    public StopState(FSM fsm, FSM.BlackBoard board)
    {
        this.fsm = fsm;
        this.board = board as BossBlackBoard;
    }

    public void OnEnter()
    {
        board.animator.Play("GreatSword_Falldown_B_Start", 0, 0f);
    }

    public void OnExit()
    {
        nowTime = 0;
        
    }

    public void OnUpdate()
    {
        nowTime += Time.deltaTime;
        if(nowTime >= board.followTime)
        {
            board.animator.SetTrigger("NoFalldown");
            nowTime = -99;
        }
    }
}

public class HitState : IState
{
    private float time;
    private BossBlackBoard board;
    private FSM fsm;
    public bool ok;
    public HitState(FSM fsm, FSM.BlackBoard board)
    {
        this.fsm = fsm;
        this.board = board as BossBlackBoard;
    }
    public void OnEnter()
    {
        board.animator.Play("Hit_F", 0, 0f);
    }

    public void OnExit()
    {
        time = 0;
    }

    public void OnUpdate()
    {
        time += Time.deltaTime;
        if(time >= 5f)
        {
            fsm.SwitchState(StateType.Move);
            return;
        }
        if(ok)
        {
            fsm.SwitchState(StateType.Move);
            ok = false;
        }
    }
}

public class DieState : IState
{
    private BossBlackBoard board;
    private FSM fsm;
    public DieState(FSM fsm, FSM.BlackBoard board)
    {
        this.fsm = fsm;
        this.board = board as BossBlackBoard;
    }
    public void OnEnter()
    {
        List<object> list = new List<object>();
        list.Add(TCPClick.Instance.user_ID);
        list.Add(1);
        list.Add(1);
        TCPClick.Instance.SendMessage(12, list);
        TaskManager.Instance.ChangeSum(1, 1);
        board.weapon.GetComponent<CapsuleCollider>().enabled = false;
        board.GO.GetComponent<CharacterController>().enabled = false;
        board.animator.Play("Die", 0);
        Rigidbody rb = board.GO.AddComponent<Rigidbody>();
        rb.useGravity = true;
    }

    public void OnExit()
    {

    }

    public void OnUpdate()
    {

    }
}

public class PatrolState : IState
{
    private BossBlackBoard board;
    private FSM fsm;
    private float time, nowTime;
    private Quaternion q, q2;
    public PatrolState(FSM fsm, FSM.BlackBoard board)
    {
        this.fsm = fsm;
        this.board = board as BossBlackBoard;
    }
    public void OnEnter()
    {
        
    }

    public void OnExit()
    {
        
    }

    public void OnUpdate()
    {
        time += Time.deltaTime;
        nowTime += Time.deltaTime;
        if(Vector3.Distance(board.player.position, board.animator.transform.position) <= 8f)
        {
            fsm.SwitchState(StateType.Move);
            return;
        }
        if (time >= 2f)
        {
            board.animator.SetFloat("y", 0f);
            if (q == Quaternion.identity)
            {
                q = Road();
                q2 = board.animator.transform.rotation;
                nowTime = 0;
            }
            board.animator.transform.rotation = Quaternion.Lerp(q2, q, nowTime);
            if (nowTime < 1f) return;
            time = 0;
            q = Quaternion.identity;
        }
        board.animator.SetFloat("y", 0.5f);
        board.controller.SimpleMove(board.controller.transform.forward * 4f);
    }

    private Quaternion Road()
    {
        Vector3 v1 = board.player.position - board.animator.transform.position, v2 = board.animator.transform.forward, v3 = board.animator.transform.right;
        v1.y = 0f;
        v2.y = 0f;
        v3.y = 0f;
        if (Vector3.Dot(v1.normalized, v2.normalized) < 0)
        {
            if (Vector3.Dot(v1.normalized, v3.normalized) > 0)
            {
                return Quaternion.AngleAxis(135, board.animator.transform.up) * board.animator.transform.rotation;
            }
            else return Quaternion.AngleAxis(-135, board.animator.transform.up) * board.animator.transform.rotation;
        }
        if(Vector3.Dot(v1.normalized,v3.normalized) > 0)
        {
            return Quaternion.AngleAxis(45, board.animator.transform.up) * board.animator.transform.rotation;
        }
        return Quaternion.AngleAxis(-45, board.animator.transform.up) * board.animator.transform.rotation;
    }
}
public class AiEnemy : MonoBehaviour
{
    private float DieTime;
    private int hurtSum;//受击次数 主要处理音效播放
    private float hurtTime;
    public AudioSource audioSource;
    private MonsterStateShow GO;
    private FSM fsm;
    public BossBlackBoard board;
    private int value;//韧性值
    private bool ok;//是否处于破韧状态
    private int maxHealth;
    private Vector3 vector3;
    public Transform Ray;
    public bool isB, isDie;
    // Start is called before the first frame update
    void Start()
    {
        UnityAction ua = IsB, ua1 = NoB;
        EventSystemManager.Instance.Register('b', ua);
        EventSystemManager.Instance.Register('c', ua1);
        DieTime = 0;
        maxHealth = board.hp;
        GO = GameObject.Instantiate(Resources.Load<GameObject>("UI/msShow")).GetComponent<MonsterStateShow>();
        GO.ts = transform;
        GO.Initialize(maxHealth, maxHealth);
        value = board.value;
        fsm = new FSM();
        fsm.AddState(StateType.Stop, new StopState(fsm, board));
        fsm.AddState(StateType.Idle, new IdleState(fsm, board));
        fsm.AddState(StateType.Move, new MoveState(fsm, board));
        fsm.AddState(StateType.Attack, new AttackState(fsm, board));
        fsm.AddState(StateType.Hit, new HitState(fsm, board));
        fsm.AddState(StateType.Die, new DieState(fsm, board));
        fsm.AddState(StateType.Patrol, new PatrolState(fsm, board));
        fsm.nowState = new IdleState(fsm, board);
        fsm.nowType = StateType.Idle;
        board.player = CameraController.Instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDie) return;
        if (hurtSum != 0)
        {
            hurtTime += Time.deltaTime;
            if (hurtTime >= 3f)
            {
                hurtSum = 0;
                hurtTime = 0;
            }
        }
        print(fsm.nowState);
        if (fsm.nowType == StateType.Die) return;
        if(board.hp == 0)
        {
            fsm.SwitchState(StateType.Die);
            isDie = true;
            StartCoroutine(Spawn());
            return;
        }
        vector3 = Camera.main.WorldToViewportPoint(this.transform.position);
        if (isB) return;
        if (vector3.x > 0 && vector3.x < 1 && vector3.y > 0 && vector3.y < 1 && vector3.z > 0 && Vector3.Distance(this.transform.position, board.player.position) <= 10f)
            ShowHp();
        else
            HideHp();
        fsm.nowState.OnUpdate();
        if(!ok && value <= 0)
        {
            board.weapon.GetComponent<CapsuleCollider>().enabled = false;
            fsm.SwitchState(StateType.Stop);
            ok = true;
        }
        else
        {
            if (ok) return;
            //已经不处于破韧状态 写AI自动逻辑
        }
    }
    public bool Check()
    {
        return isDie;
    }
    private void OnDestroy()
    {
        UnityAction ua = IsB,ua1 = NoB;
        EventSystemManager.Instance.Unregister('b', ua);
        EventSystemManager.Instance.Unregister('c', ua1);
    }
    IEnumerator Spawn()
    {
        while(DieTime < 5)
        {
            DieTime += Time.deltaTime;
            yield return null;
        }
        List<object> list = new List<object>();
        list.Add(TCPClick.Instance.user_ID);
        list.Add(2);
        list.Add(1);
        TCPClick.Instance.SendMessage(14, list);
        BackPackManager.Instance.playerData.scene_id = 2;
        ChangeMenu.Instance.Show(1, true);
    }

    //结束僵直状态的回调
    public void HitExit()
    {
        (fsm.nowState as HitState).ok = true;
    }

    public void Des()
    {
        Destroy(gameObject);
    }
    //展示和隐藏血量条
    public void ShowHp()
    {
        GO.gameObject.SetActive(true);
    }

    public void HideHp()
    {
        GO.gameObject.SetActive(false);
    }
    public void IsB()
    {
        HideHp();
        isB = true;
    }
    public void NoB()
    {
        isB = false;
    }
    //受伤逻辑
    public void Hurt(Transform ts, int damage, int id ,int value)
    {
        if (id != board.id && fsm.nowType != StateType.Die)
        {
            board.hp -= damage;
            GO.Initialize(maxHealth, board.hp);
            switch (hurtSum)
            {
                case 0:
                    audioSource.clip = GameData.Instance.hit0;
                    break;
                case 1:
                    audioSource.clip = GameData.Instance.hit1;
                    break;
                case 2:
                    audioSource.clip = GameData.Instance.hit2;
                    break;
                case 3:
                    audioSource.clip = GameData.Instance.hit3;
                    hurtSum = -1;
                    break;
            }
            hurtSum++;
            audioSource.Play();
            if (!ok)
            {
                this.value -= value;
                //设置触发受伤僵直动画的概率
                if(Random.value > 0.6f)
                {
                    this.transform.rotation = Quaternion.LookRotation(ts.position - this.transform.position);
                    board.weapon.GetComponent<CapsuleCollider>().enabled = false;
                    fsm.SwitchState(StateType.Hit);
                }
                this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(ts.position - transform.position), Time.deltaTime);
            }
        }
    }

    //破韧动画起身的逻辑
    public void OnAnimatorStand()
    {
        fsm.SwitchState(StateType.Idle);
        ok = false;
        value = board.value;
    }

    //动画开始检测碰撞
    public void OnAnimationAttackEvent()
    {
        board.weapon.GetComponent<CapsuleCollider>().enabled = true;
    }
    //动画结束检测碰撞
    public void OnAnimationAttackEventQuit()
    {
        board.weapon.GetComponent<CapsuleCollider>().enabled = false;
    }
    //攻击动画时间结束回调
    public void OnAnimationExit()
    {
        (fsm.nowState as AttackState).ok = true;
    }

    public void ShowW10()
    {
        audioSource.clip = GameData.Instance.w1_0;
        audioSource.Play();
    }
    public void ShowW11()
    {
        audioSource.clip = GameData.Instance.w1_1;
        audioSource.Play();
    }
    public void ShowW12()
    {
        audioSource.clip = GameData.Instance.w1_2;
        audioSource.Play();
    }
}
