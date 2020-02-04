using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDetector : MonoBehaviour
{
    CharacterControl characterControl;

    private void Awake()
    {
        characterControl = GetComponent<CharacterControl>();
    }


    private void Update()
    {
        if (AttackManager.Instance.CurrentAttacks.Count > 0)
        {
            CheckAttack();
        }        
    }

    void CheckAttack()
    {
        foreach( AttackInfo info in AttackManager.Instance.CurrentAttacks)
        {
            if( info == null || !info.isRegisterd || info.Attacker==characterControl ||
                info.isFinished || info.CurrentHits >= info.MaxHits)
            {
                continue;
            }

            if( info.MustCollide)
            {
                if( IsCollided(info))
                {
                    TakeDamage(info);
                }
            }

        }
    }

    private bool IsCollided( AttackInfo info)
    {
        foreach( Collider collider in characterControl.CollidingParts)
        {
            foreach( string name in info.ColliderNames)
            {
                Debug.Log(name + "  " + collider.gameObject.name);
                if (name == collider.gameObject.name)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private void TakeDamage( AttackInfo info)
    {
        Debug.Log(info.Attacker.gameObject.name + " hits: " + this.gameObject.name);
        characterControl.SkinnedMeshAnimator.runtimeAnimatorController = info.AttackAbility.GetDeathAnimator();
        info.CurrentHits++;
    }

}
