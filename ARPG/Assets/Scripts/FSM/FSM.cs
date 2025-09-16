using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM
{
    public IState nowState;
    public StateType nowType;
    public Dictionary<StateType, IState> dic;
    private BlackBoard board;
    [Serializable]
    public class BlackBoard
    {

    }
    public FSM()
    {
        this.dic = new Dictionary<StateType, IState>();
    }

    public void AddState(StateType st,IState Is)
    {
        if (dic.ContainsKey(st)) return;
        dic.Add(st, Is);
    }

    public void SwitchState(StateType st)
    {
        if (!dic.ContainsKey(st))
        {
            Debug.Log("²»´æÔÚ×´Ì¬");
            return;
        }
        if (nowState != null) nowState.OnExit();
        nowState = dic[st];
        nowState.OnEnter();
        nowType = st;
    }
}
