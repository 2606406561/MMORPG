using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;


public class AttackAnimator : StateMachineBehaviour
{
    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        base.OnStateMachineEnter(animator, stateMachinePathHash);
        PlayerController.Instance.isAtk = true;
    }
}
