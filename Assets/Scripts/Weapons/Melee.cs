using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class Melee : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private WeaponData weaponData;

        public float attackTime;
        // Start is called before the first frame update
        void Start()
        {
            PlayerController.AttackInput += Attack;
            attackTime = weaponData.attackTime;
            weaponData.attacking = false;
            Physics.IgnoreCollision(transform.GetComponent<CapsuleCollider>(), transform.GetComponentInParent<CapsuleCollider>()); // prevent weapon from attacking parent
        }

        public void Attack()
        {
            weaponData.attacking = true;
            StartCoroutine(nameof(attackTime));
            Debug.Log("Attacking");
        }
        
        private IEnumerator AttackTimer()
        {
            yield return new WaitForSeconds(weaponData.attackTime);
            weaponData.attacking = false;
        }
        
        private void OnTriggerEnter(Collider other) // if it collides with a player it will send the take damage message
        {
            if(other.CompareTag("Player")) {
                if(weaponData.attacking)
                {
                    other.SendMessage("TakeDamage", weaponData.damage);
                    Debug.Log("DamageInflicted");
                }
            }
        }
    
    }
}
