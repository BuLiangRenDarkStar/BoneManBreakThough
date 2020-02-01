using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName ="New State", menuName = "Roundbeargames/AbilityData/MoveForward")]
public class MoveForward : StateData
{
    public bool Constrant;
    public float Speed ;
    public AnimationCurve SpeedGraph;
    public float BlockDistance;

    public override void OnEnter(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        
    }

    public override void UpdateAbility(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        //Debug.Log("MoveForward");
        CharacterControl characterControl = characterStateBase.GetCharacterControl(animator);

        if( characterControl.Jump )
        {
            animator.SetBool( TransitionParameter.Jump.ToString(), true );
        }
        if( Constrant)
        {
            ConstantMove(characterControl, animator, stateInfo);
        }
        else
        {
            ControlledMove(characterControl, animator, stateInfo);
        }
        
    }

    public override void OnExit(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        
    }

    private void ConstantMove(CharacterControl characterControl, Animator animator, AnimatorStateInfo stateInfo)
    {
        if( !CheckFront(characterControl))
        {
            characterControl.transform.Translate(Vector3.forward * Speed * SpeedGraph.Evaluate(stateInfo.normalizedTime)  * Time.deltaTime);
        }
    }

    void ControlledMove( CharacterControl characterControl, Animator animator, AnimatorStateInfo stateInfo)
    {
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
                characterControl.MoveForward(Speed, SpeedGraph.Evaluate(stateInfo.normalizedTime));
            }
        }

        if (characterControl.MoveRight)
        {
            characterControl.transform.rotation = Quaternion.Euler(0, 0, 0);
            if (!CheckFront(characterControl))
            {
                characterControl.MoveForward(Speed, SpeedGraph.Evaluate(stateInfo.normalizedTime));
            }
        }
    }
    //返回true 可继续前进
    bool CheckFront( CharacterControl characterControl )
    {
        for (int i=0; i<characterControl.FrontSpheres.Count; ++i)
        {           
            GameObject frontSphere = characterControl.FrontSpheres[i];
            Debug.DrawRay(frontSphere.transform.position, frontSphere.transform.forward, Color.blue);
            RaycastHit hit;
            if( Physics.Raycast( frontSphere.transform.position, frontSphere.transform.forward, out hit, BlockDistance ))
            {
                if ( !characterControl.RagdollParts.Contains(hit.collider) )//射线投射到自身碰撞框 直接跳过
                {
                    if( !IsBodyPart(hit.collider) )//碰撞位置不是其它游戏npc碰撞框
                    {
                        //Debug.Log("CheckFront true");
                        return true;
                    }
                }               
            }
        }
        return false;
    }

    bool IsBodyPart( Collider col)
    {
        CharacterControl characterControl = col.transform.root.GetComponent<CharacterControl>();
        if( characterControl==null)
        {
            return false;
        }

        if( characterControl.gameObject == col.gameObject)
        {
            return false;
        }

        if( characterControl.RagdollParts.Contains( col ))
        {
            return true;
        }
        return false;
    }

}
