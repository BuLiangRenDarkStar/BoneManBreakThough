﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu( fileName = "New State", menuName = "Roundbeargames/AbilityData/Idle") ]
public class Idle : StateData
{
    public override void UpdateAbility(CharacterStateBase characterStateBase, Animator animator)
    {
        if (VirtualInputManager.Instance.MoveLeft)
        {
            animator.SetBool(TransitionParameter.Move.ToString(), true);
        }

        if (VirtualInputManager.Instance.MoveRight)
        {
            animator.SetBool(TransitionParameter.Move.ToString(), true);
        }
    }
}
