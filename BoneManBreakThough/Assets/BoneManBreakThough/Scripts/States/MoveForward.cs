using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New State", menuName = "Roundbeargames/AbilityData/MoveForward")]
public class MoveForward : StateData
{
    public float Speed;

    public override void UpdateAbility(CharacterStateBase characterStateBase, Animator animator)
    {
        CharacterControl characterControl = characterStateBase.GetCharacterControl(animator);

        if (characterControl.MoveRight && characterControl.MoveLeft)
        {
            animator.SetBool(TransitionParameter.Move.ToString(), false);
            return;
        }

        if (!characterControl.MoveRight && !characterControl.MoveLeft)
        {
            animator.SetBool(TransitionParameter.Move.ToString(), false);
            return;
        }

        if ( characterControl.MoveLeft)
        {
            characterControl.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            characterControl.transform.rotation = Quaternion.Euler(0, 180, 0);
            animator.SetBool(TransitionParameter.Move.ToString(), true);
        }

        if (characterControl.MoveRight)
        {
            characterControl.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            characterControl.transform.rotation = Quaternion.Euler(0, 0, 0);
            animator.SetBool(TransitionParameter.Move.ToString(), true);
        }
    }
}
