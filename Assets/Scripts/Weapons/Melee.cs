using UnityEngine;

namespace Weapons
{
    public class Melee : Weapon
    {
    
        // Start is called before the first frame update
        void Start()
        {
            Attacking = false;
            Physics.IgnoreCollision(weapon.GetComponent<CapsuleCollider>(), weapon.GetComponentInParent<CapsuleCollider>()); // prevent weapon from attacking parent
        }

        private void OnTriggerEnter(Collider other) // if it collides with a player it will send the take damage message
        {
            if(other.CompareTag("Player")) {
                if(Attacking)
                {
                    other.SendMessage("TakeDamage", damage);
                    Debug.Log("DamageInflicted");
                }
            }
        }
    
    }
}
