using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//保存状态，具体的状态继承自该类
public abstract class StateData : ScriptableObject
{
    public abstract void OnEnter(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo);
    public abstract void UpdateAbility(CharacterStateBase characterStateBase, Animator animator,AnimatorStateInfo stateInfo);
    public abstract void OnExit(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo);

}
