using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class HeavyWeapon : MonoBehaviour
{
    public Vector3 Position; // prefab tested attachment position
    public Quaternion Rotation; // prefab tested attachment rotation
    public Vector3 Scale; // prefab tested attachment scale
    public GameObject attachmentPoint; // where the weapon is going to attach to on the player prefab.
    public GameObject heavyWeaponPrefab; // prefab of the weapon 
    
    private GameObject heavyWeapon;
    private bool Attacking;
    // Start is called before the first frame update
    void Start()
    {
        Attacking = false;
        
        heavyWeapon = Instantiate(heavyWeaponPrefab, Vector3.zero, Quaternion.identity); // instantiate a new version of the prefab

        heavyWeapon.transform.parent = attachmentPoint.transform;
        heavyWeapon.transform.localPosition = Position;
        heavyWeapon.transform.localRotation = Rotation;
        heavyWeapon.transform.localScale = Scale;
        
        Physics.IgnoreCollision(heavyWeapon.GetComponent<CapsuleCollider>(), heavyWeapon.GetComponentInParent<CapsuleCollider>()); // prevent weapon from attacking parent
    }

    private void OnTriggerEnter(Collider other) // if it collides with a player it will send the take damage message
    {
        if(other.CompareTag("Player")) {
            if(Attacking)
            {
                other.SendMessage("TakeDamage", 25);
                Debug.Log("DamageInflicted");
            }
        }
    }

    
    public void SetAttacking() // allow the parent class to tell the weapon it's attacking
    {
        Attacking = !Attacking;
    }
}
