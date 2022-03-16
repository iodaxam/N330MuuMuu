using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class Melee : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private WeaponData weaponData;

        [HideInInspector]
        public float attackTime;
        // Start is called before the first frame update
        void Start()
        {
            PlayerController.AttackInput += Attack;
            attackTime = weaponData.attackTime;
            weaponData.attacking = false;
            //Physics.IgnoreCollision(transform.GetComponent<CapsuleCollider>(), transform.GetComponentInParent<CapsuleCollider>()); // prevent weapon from attacking parent
        }

        public void Attack()
        {
            weaponData.attacking = true;
            StartCoroutine(nameof(AttackTimer));
        }
        
        private IEnumerator AttackTimer()
        {
            yield return new WaitForSeconds(weaponData.attackTime);
            weaponData.attacking = false;
        }
        
        private void OnTriggerEnter(Collider other) // if it collides with a player it will send the take damage message
        {
            if (!other.CompareTag("Player")) return;
            if (!weaponData.attacking) return;
            Debug.Log("Other: " + other.name);
            other.SendMessage("TakeDamage", weaponData.damage);
            Debug.Log("DamageInflicted");
        }
    
    }
}
