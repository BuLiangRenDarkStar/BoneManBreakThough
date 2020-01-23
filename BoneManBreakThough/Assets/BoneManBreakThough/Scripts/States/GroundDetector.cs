using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New State",menuName = "Roundbeargames/AbilityData/GroundDetector")]
public class GroundDetector : StateData
{

    [Range(0.01f, 1f)]
    public float CheckTime;
    public float Distance;

    public override void OnEnter(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        
    }

    public override void OnExit(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        
    }

    public override void UpdateAbility(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        CharacterControl characterControl = characterStateBase.GetCharacterControl(animator);
        if( stateInfo.normalizedTime>=CheckTime )//确保已经跳起
        {
            if( IsGrounded( characterControl ))
            {
                animator.SetBool(TransitionParameter.Grounded.ToString(), true);
            }
            else
            {
                animator.SetBool(TransitionParameter.Grounded.ToString(), false);
            }            
        }
    }

    //按照一定步长添加角色底部位置，检测那些位置是否与地面的距离
    bool IsGrounded( CharacterControl characterControl)
    {
        if( characterControl.RIGID_BODY.velocity.y>-0.01f && characterControl.RIGID_BODY.velocity.y<=0f)
        {
            return true;
        }

        if (characterControl.RIGID_BODY.velocity.y <= 0f)
        {            
            for (int i = 0; i < characterControl.BottomSpheres.Count; ++i)
            {
                GameObject bottomSphere = characterControl.BottomSpheres[i];
                Debug.DrawRay( bottomSphere.transform.position, -Vector3.up, Color.red ); 
                RaycastHit hit;
                if ( Physics.Raycast(bottomSphere.transform.position, -Vector3.up, out hit, Distance) )
                {                
                    if( !characterControl.RagdollParts.Contains( hit.collider )  )
                        return true;
                }

            }
        }
        return false;
    }
}
