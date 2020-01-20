using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName="New State",menuName = "Roundbeargames/AbilityData/Jump")]
public class Jump : StateData
{
    public float JumpForce;

    public override void OnEnter(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        characterStateBase.GetCharacterControl(animator).RIGID_BODY.AddForce(Vector3.up * JumpForce);
    }

    public override void UpdateAbility(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        CharacterControl characterControl = characterStateBase.GetCharacterControl(animator);

        
    }

    public override void OnExit(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {

    }
}
