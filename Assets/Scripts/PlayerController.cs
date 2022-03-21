using System;
using System.Collections;
//using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;
using Weapons;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    
    public static Action AttackInput;
    
    // Movement
    [Header("Movement")]
    public float speed = 5;
    public float rotationSpeed = 70f;
    public float staggerTime = 0.5f; // amount of time the player is in the 'staggered' state when they are hit.
    
    private Vector2 movementInput;
    [HideInInspector] public Vector3 spawnLocation = Vector3.zero; // this is handled by the player input manager
    
    // Health
    [Header("Health")]
    public int MaxLives = 3;
    public float maxHealth = 100;
    public HealthBar HealthBar;
    
    private int CurrentLives;
    private bool isDead;

    // Combat
    [Header("Combat")]
    public GameObject WeaponManager;
    
    private int WeaponIndex;
    private bool attacking;
    private string WeaponName;
    private WeaponData currentWeaponData;

    // Components
    //[Header("References")] // Headers only needed for public variables.
    [SerializeField] private Transform[] weapons;
    private Animator animator;
    private GameObject GameManager;
    private GameManager GMscript;

    [Header("UI Elements")] 
    public GameObject HealthBarUI;
    public GameObject SelectorUI;

    private bool Ready = false;
    private bool GameStarted = false;
    [HideInInspector] public int playerID; // set by input manager

    public delegate void DiedAction();
    public static event DiedAction PlayerDied;

    private void Start()
    {
        // Get components
        GameManager = GameObject.FindWithTag("GameManager");
        animator = GetComponent<Animator>();
        GMscript = GameManager.GetComponent<GameManager>();

        PlayerDied += GMscript.PlayerDied;
        
        // Set variables
        transform.position = spawnLocation;
        CurrentLives = MaxLives;
        if (Camera.main != null) transform.LookAt(Camera.main.transform);

        // Call functions
        PlayerInputManager.MoveToArena += StartGame;
        //GMscript.StartGame += StartGame;
        Melee.DoneAttacking += DoneAttacking;
        InitializeWeapons();
    }

    void OnDisable()
    {
        PlayerDied -= GMscript.PlayerDied;
    }

    private void Update()
    {
        if (isDead) return; // temporary code for testing death

        if(movementInput != Vector2.zero && GameStarted)
        {
            if (!attacking)
            {
                animator.SetBool("Moving", true); // tell animator player is moving
                transform.Translate(Vector3.forward * speed * Time.deltaTime);

                //Smoothly Rotate player in direction of the input
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(movementInput.x, 0f, movementInput.y)), Time.deltaTime * rotationSpeed);
            }

            if (GMscript.StopLightState == EnumStopLight.RedLightState)
            {
                TakeDamage(0.1f);
            }
            
        }
        else if (movementInput == Vector2.zero)
        {
            animator.SetBool("Moving", false);
        }
    }

    private void StartGame()
    {
        GameStarted = true;
        transform.position = spawnLocation;
    }

    // Movement code
    public void OnMove(InputAction.CallbackContext context) => movementInput = context.ReadValue<Vector2>();

    //Input information for the lmb
    public void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        if(!attacking && !isDead && GameStarted)
        {
            attacking = true;
            animator.SetBool("Attacking", true);
            AttackInput?.Invoke();
        }
    }

    public void OnReady(InputAction.CallbackContext context)
    {
        Debug.Log("Ready");
        if (Ready) return;
        Ready = true;
        GMscript.Ready();
    }
    
    private void DoneAttacking()
    {
        animator.SetBool("Attacking", false);
        attacking = false;
    }

    // Deal with taking damage
    private void TakeDamage(float damageAmount)
    {
        HealthBar.TakeDamage(damageAmount);
       
        if (!(damageAmount > 1)) return;
        animator.SetBool("Impact", true);
        StartCoroutine(nameof(StaggerTime));
    }
    
    private IEnumerator StaggerTime()
    {
        yield return new WaitForSeconds(staggerTime);
        animator.SetBool("Impact", false);
    }
    
    private void Died()
    {
        isDead = true;
        animator.SetBool("Dead", true);
        animator.Play("Default Idle");
        PlayerDied.Invoke();
    }

    private void InitializeWeapons()
    {
        int weaponCount = WeaponManager.transform.childCount;
        weapons = new Transform[weaponCount];

        for (int i = 0; i < weaponCount; i++)
        {
            weapons[i] = WeaponManager.transform.GetChild(i);
        }
        
        CycleWeapons(1);
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
            if (i == WeaponIndex)
            {
                if (WeaponName != null) animator.ResetTrigger(WeaponName);
                weapons[i].gameObject.SetActive(true);
                WeaponName = weapons[i].gameObject.name;
                animator.Play("Default Idle");
                animator.SetTrigger(WeaponName);
            }
            else
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }
    
}