using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New State", menuName = "Roundbeargames/AbilityData/MoveForward")]
public class MoveForward : StateData
{
    public float Speed;
    public AnimationCurve SpeedGraph;
    public float BlockDistance;

    public override void OnEnter(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        
    }

    public override void UpdateAbility(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        CharacterControl characterControl = characterStateBase.GetCharacterControl(animator);

        if( characterControl.Jump )
        {
            animator.SetBool( TransitionParameter.Jump.ToString(), true );
        }

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

        if (characterControl.MoveLeft)
        {
            characterControl.transform.rotation = Quaternion.Euler(0, 180, 0);
            if (!CheckFront(characterControl))
            {
                characterControl.transform.Translate(Vector3.forward * Speed * Time.deltaTime);
            }
        }

        if (characterControl.MoveRight)
        {
            characterControl.transform.rotation = Quaternion.Euler(0, 0, 0);
            if ( !CheckFront(characterControl))
            {
                characterControl.transform.Translate(Vector3.forward * Speed * Time.deltaTime);                
            }                        
        }
    }

    public override void OnExit(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        
    }

    bool CheckFront( CharacterControl characterControl )
    {
        for(int i=0; i<characterControl.FrontSpheres.Count; ++i)
        {           
            GameObject frontSphere = characterControl.FrontSpheres[i];
            Debug.DrawRay(frontSphere.transform.position, frontSphere.transform.forward, Color.blue);
            RaycastHit hit;
            if( Physics.Raycast( frontSphere.transform.position, frontSphere.transform.forward, out hit, BlockDistance ))
            {
                return true;
            }
        }
        return false;
    }
}
