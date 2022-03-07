using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapons;

public class PlayerCombat : MonoBehaviour
{
    private bool CanAttack;
    private bool Attacking;
    public List<Weapon> weapons;

    public float AttackDuration = 2f;

    public float CooldownTimerMax = 3f;
    private float CooldownTimerCurrent;
    
    public enum EnumWeapons {HeavyMelee, LightMelee, Ranged};
    private EnumWeapons currentWeapon;

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

    public void SelectWeapon(EnumWeapons newWeapon)
    {
        string weaponName;
        if (currentWeapon == newWeapon) return;
        switch (newWeapon)
        {
            case EnumWeapons.HeavyMelee:
                weaponName = "HeavyMelee";
                break;
            
            case EnumWeapons.LightMelee:
                weaponName = "LightMelee";
                break;
            
            case EnumWeapons.Ranged:
                weaponName = "Ranged";
                break;
            
            default:
                weaponName = "HeavyMelee";
                break;
        }

        foreach (Weapon weapon in weapons)
        {
            if (weaponName == weapon.name)
            {
                weapon.gameObject.SetActive(true);
            }
            else
            {
                weapon.gameObject.SetActive(false);
            }
        }
    }
}
