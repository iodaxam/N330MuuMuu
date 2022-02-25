using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5;
    private Vector2 movementInput;
    public float rotationSpeed = 70f;

    //Scripts
    public PlayerCombat combatScript;

    private void Update()
    {
        if(movementInput != Vector2.zero)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);

            //Smoothly Rotate player in direction of the input
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(movementInput.x, 0f, movementInput.y)), Time.deltaTime * rotationSpeed);
        }
    }

    public void OnMove(InputAction.CallbackContext context) => movementInput = context.ReadValue<Vector2>();

    //Input information for the lmb
    public void OnPrimaryAttack(InputAction.CallbackContext context)
    {
        if(context.performed) 
        //^^ So that InitiateAttack() is only called once
        {
            combatScript.InitiateAttack();
        }
    }
}
