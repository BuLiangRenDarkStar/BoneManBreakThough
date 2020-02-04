using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New State", menuName = "Roundbeargames/AbilityData/Attack")]
public class Attack : StateData
{
    public float StartAttackTime;
    public float EndAttackTime;
    public List<string> ColliderNames = new List<string>();
    public bool MustCollide;
    public bool MustFaceAttacker;
    public float LethalRange;
    public int MaxHits;
    public List<RuntimeAnimatorController> DeathAnimator = new List<RuntimeAnimatorController>();
    public override void OnEnter(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        //Debug.Log("Attack Enter");
        animator.SetBool(TransitionParameter.Attack.ToString(), false);

        GameObject obj = Instantiate(Resources.Load("AttackInfo", typeof(GameObject))) as GameObject;
        AttackInfo info = obj.GetComponent<AttackInfo>();

        info.ResetInfo(this);

        if( !AttackManager.Instance.CurrentAttacks.Contains( info ))
        {
            AttackManager.Instance.CurrentAttacks.Add(info);
        }
    }

    public void RegisterAttack(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {        
        if ( StartAttackTime<=stateInfo.normalizedTime&&EndAttackTime>stateInfo.normalizedTime)
        {
            Debug.Log( "   " + stateInfo.normalizedTime);
            foreach ( AttackInfo info in AttackManager.Instance.CurrentAttacks)
            {
                if( info!=null && !info.isRegisterd&&info.AttackAbility==this)
                {
                    info.Register(this, characterStateBase.GetCharacterControl(animator));
                }
            }
        }
    }

    public void DeregisterAttack(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        if( stateInfo.normalizedTime>=EndAttackTime)
        {
            foreach (AttackInfo info in AttackManager.Instance.CurrentAttacks)
            {
                if (info != null && info.AttackAbility == this && !info.isFinished )
                {
                    info.isFinished = true;
                    Destroy(info.gameObject);
                }
            }
        }
    }

    public override void OnExit(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        ClearAttack();
    }

    public override void UpdateAbility(CharacterStateBase characterStateBase, Animator animator, AnimatorStateInfo stateInfo)
    {
        RegisterAttack(characterStateBase, animator, stateInfo);
        DeregisterAttack(characterStateBase, animator, stateInfo);
    }

    public void ClearAttack()
    {
        for( int i=0; i<AttackManager.Instance.CurrentAttacks.Count; ++i)
        {
            if( AttackManager.Instance.CurrentAttacks[i] == null || AttackManager.Instance.CurrentAttacks[i].isFinished)
            {
                AttackManager.Instance.CurrentAttacks.RemoveAt(i);
            }
        }
    }

    public RuntimeAnimatorController GetDeathAnimator()
    {
        int index = Random.Range(0, DeathAnimator.Count);
        return DeathAnimator[index];
    }
}
