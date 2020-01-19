using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStateBase : StateMachineBehaviour
{
    public List<StateData> ListAbilityData = new List<StateData>();
    private void UpdateAll( CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        for( int i=0; i<ListAbilityData.Count; ++i)
        {
            ListAbilityData[i].UpdateAbility(characterStateBase, animator, stateInfo);
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < ListAbilityData.Count; ++i)
        {
            ListAbilityData[i].OnEnter(this, animator, stateInfo);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        UpdateAll(this, animator, stateInfo);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        for (int i = 0; i < ListAbilityData.Count; ++i)
        {
            ListAbilityData[i].OnExit(this, animator, stateInfo);
        }
    }

    private CharacterControl characterControl;
    public CharacterControl GetCharacterControl( Animator animator)
    {
        if( null==characterControl)
        {
            characterControl = animator.GetComponentInParent<CharacterControl>();
        }
        return characterControl;
    }
}
