using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    public static Action AttackInput;
    
    // Movement
    public float speed = 5;
    private Vector2 movementInput;
    public float rotationSpeed = 70f; 
    
    // PlayerLives
    public int MaxLives = 3;
    private int CurrentLives;

    // Combat
    private int WeaponIndex;
    public GameObject WeaponManager;
    private bool attacking;

    // Health
    public float maxHealth = 100;
    private bool isDead;
    
    // Scripts
    private HealthBar HealthBar;

    private void Start()
    {
        CurrentLives = MaxLives;
        animator = GetComponent<Animator>();
        HealthBar = GetComponent<HealthBar>();
        
        animator.SetTrigger("HeavyMelee"); // for testing purposes only. Should be set based on weapon carried
    }

    private void Update()
    {
        //Debug.Log(rb.velocity.normalized);

        if (isDead) return; // temporary code for testing death
        
        
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
        Debug.Log("OnPrimaryAttack");
        if(!attacking)
        {
            Debug.Log("Attacking");
            attacking = true;
            animator.SetBool("Attacking", true);
            AttackInput?.Invoke();
            StartCoroutine(nameof(AttackTimer));
        }
    }

    private IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(1);
        animator.SetBool("Attacking", false);
        attacking = false;
    }

    // Deal with taking damage
    private void TakeDamage(float damageAmount)
    {
        HealthBar.TakeDamage(damageAmount);
        animator.SetBool("GotHit", true);
        StartCoroutine(nameof(StaggerTime));
    }
    
    private IEnumerator StaggerTime()
    {
        yield return new WaitForSeconds(0.5f);
        animator.SetBool("GotHit", false);
    }
    
    private void Died()
    {
        isDead = true;
        animator.SetBool("Dead", true);
        animator.Play("Default Idle");
    }

    // private void CycleWeapons()
    // {
    //     WeaponIndex += 1;
    //     if (WeaponIndex > WeaponList.Capacity)
    //     {
    //         WeaponIndex = 0;
    //     }
    //     
    //     if (weapon != null)
    //     {
    //         weapon.SetActive(false);
    //     }
    //
    //     weapon = WeaponList[WeaponIndex];
    //     weapon.SetActive(true);
    // }
    //
    
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
