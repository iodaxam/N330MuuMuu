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
    [Header("Movement")]
    public float speed = 5;
    private Vector2 movementInput;
    public float rotationSpeed = 70f; 
    
    // Health
    [Header("Health")]
    public int MaxLives = 3;
    private int CurrentLives;
    public float maxHealth = 100;
    private bool isDead;

    // Combat
    [Header("Combat")]
    private int WeaponIndex;
    public GameObject WeaponManager;
    private bool attacking;

    // Scripts
    [Header("References")]
    [SerializeField] private Transform[] weapons;
    private HealthBar HealthBar;

    private void Start()
    {
        CurrentLives = MaxLives;
        animator = GetComponent<Animator>();
        HealthBar = GetComponent<HealthBar>();
        Debug.Log("Weapons.length: " + weapons.Length);
        
        InitializeWeapons();
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
        yield return new WaitForSeconds(1.267f);
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

    private void InitializeWeapons()
    {
        int weaponCount = WeaponManager.transform.childCount;
        weapons = new Transform[weaponCount];

        for (int i = 0; i < weaponCount; i++)
        {
            weapons[i] = WeaponManager.transform.GetChild(i);
        }
        
        CycleWeapons(0);
    }
    
    private void CycleWeapons(int change)
    {
        WeaponIndex += change;
        if (WeaponIndex < 0)
        {
            WeaponIndex = weapons.Length;
        } 
        else if (WeaponIndex > weapons.Length - 1)
        {
            WeaponIndex = 0;
        }
        
        for (int i = 0; i < weapons.Length; i++)
        {
            Debug.Log(weapons[i].gameObject.name);
            weapons[i].gameObject.SetActive(i == WeaponIndex);
        }
    }
}
