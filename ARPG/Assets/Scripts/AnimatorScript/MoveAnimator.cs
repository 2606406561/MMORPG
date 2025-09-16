using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAnimator : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        PlayerController.Instance.QuitMove();
    }
}
