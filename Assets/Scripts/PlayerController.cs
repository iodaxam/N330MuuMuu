using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    
    // Movement
    public float speed = 5;
    private Vector2 movementInput;
    public float rotationSpeed = 70f; 
    
    // PlayerLives
    public int MaxLives = 3;
    private int CurrentLives;

    // Combat
    private bool attacking;
    private Weapon weapon;

    // Health
    public float MaxHealth = 100;
    private float CurrentHealth;
    private bool isDead;
    
    private void Start()
    {
        CurrentLives = MaxLives;
        CurrentHealth = MaxHealth;
        animator = GetComponent<Animator>();
        weapon = GetComponent<Weapon>();
        animator.SetTrigger("HeavyMelee"); // for testing purposes only. Should be auto set based on weapon carried
    }

    private void Update()
    {
        if(movementInput != Vector2.zero && !attacking)
        {
            animator.SetBool("Moving", true); // tell animator player is moving
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            //Smoothly Rotate player in direction of the input
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(movementInput.x, 0f, movementInput.y)), Time.deltaTime * rotationSpeed);
        }
        else if (movementInput == Vector2.zero)
        {
            animator.SetBool("Moving", false);
        }
    }

    // Movement code
    public void OnMove(InputAction.CallbackContext context) => movementInput = context.ReadValue<Vector2>();

    //Input information for the lmb
    public void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        if(!attacking)
        {
            attacking = true;
            animator.SetBool("Attacking", true);
            SendMessage("SetAttacking", weapon);
            StartCoroutine(nameof(AttackTimer));
        }
    }

    private IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(weapon.attackTime);
        animator.SetBool("Attacking", false);
        attacking = false;
        SendMessage("SetAttacking", weapon);
    }

    private void TakeDamage(float damageAmount)
    {
        CurrentHealth -= Mathf.Clamp(damageAmount, 0, damageAmount);
        if (CurrentHealth <= 0)
        {
            isDead = true;
        }
        else
        {
            animator.SetTrigger("GotHit"); // TODO: change this to something other than a trigger in the animator
        }
    }
    
    
    // =================== Weapon Swapping ================== //
    // below is some weapon swapping code that needs revisited.
    // I didn't really get the chance to finish what I had
    // started with it because it was missing something...
    
    // public List<Weapon> weapons;
    
    // public enum EnumWeapons {HeavyMelee, LightMelee, Ranged};
    // private EnumWeapons currentWeapon;

    // public void SelectWeapon(EnumWeapons newWeapon)
    // {
    //     string weaponName;
    //     if (currentWeapon == newWeapon) return;
    //     switch (newWeapon)
    //     {
    //         case EnumWeapons.HeavyMelee:
    //             weaponName = "HeavyMelee";
    //             break;
    //         
    //         case EnumWeapons.LightMelee:
    //             weaponName = "LightMelee";
    //             break;
    //         
    //         case EnumWeapons.Ranged:
    //             weaponName = "Ranged";
    //             break;
    //         
    //         default:
    //             weaponName = "HeavyMelee";
    //             break;
    //     }
    //
    //     foreach (Weapon weapon in weapons)
    //     {
    //         if (weaponName == weapon.name)
    //         {
    //             weapon.gameObject.SetActive(true);
    //         }
    //         else
    //         {
    //             weapon.gameObject.SetActive(false);
    //         }
    //     }
    // }
}
