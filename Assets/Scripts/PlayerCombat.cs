using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private bool CanAttack;
    private bool Attacking;

    public float AttackDuration = 2f;

    public float CooldownTimerMax = 3f;
    private float CooldownTimerCurrent;

    public void InitiateAttack()
    {
        if(CanAttack)
        {
            Attacking = true;
            CanAttack = false;

            CooldownTimerCurrent = CooldownTimerMax;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) {
            if(Attacking)
            {
                Debug.Log("DamageInflicted");
            }
        }
    }

    private void Update()
    {
        if(!CanAttack)
        {
            CooldownTimerCurrent -= Time.deltaTime;

            if(CooldownTimerCurrent <= (CooldownTimerMax - AttackDuration)) {Attacking = false;}

            if(CooldownTimerCurrent <= 0f) {CanAttack = true;}
        }
    }
}
