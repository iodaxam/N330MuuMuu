using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Weapons
{
    public class Melee : MonoBehaviour
    {
        public static Action DoneAttacking;

        public delegate void HitAction(bool LightWeapon);
        public static event HitAction PlayerHit;

        private GameManager GameManagerScript;
        
        [Header("References")]
        [SerializeField] private WeaponData weaponData;

        [HideInInspector]
        public float attackTime;

        private List<Collider> hasBeenHit;
        // Start is called before the first frame update
        private void Start()
        {
            GameManagerScript = GameObject.FindWithTag("GameManager").GetComponent<GameManager>();

            PlayerHit += GameManagerScript.PlayerHitFunction;

            hasBeenHit = new List<Collider>(GameObject.FindGameObjectsWithTag("Player").Length);
            
            PlayerController.AttackInput += Attack;
            
            attackTime = weaponData.attackTime;
            weaponData.attacking = false;
            
            Physics.IgnoreCollision(transform.GetComponent<CapsuleCollider>(), transform.GetComponentInParent<CapsuleCollider>()); // prevent weapon from attacking parent
        }

        void OnDisable()
        {
            //PlayerHit -= GameManagerScript.PlayerHitFunction;
        }

        private void Attack()
        {
            weaponData.attacking = true;
            StartCoroutine(nameof(AttackTimer));
        }
        
        private IEnumerator AttackTimer()
        {
            yield return new WaitForSeconds(weaponData.attackTime);
            weaponData.attacking = false;
            hasBeenHit.Clear();
            DoneAttacking?.Invoke();
        }
        
        private void OnTriggerEnter(Collider other) // if it collides with a player it will send the take damage message
        {
            if (!other.CompareTag("Player")) return;
            if (!weaponData.attacking) return;
            
            var otherHasBeenHit = false;
            foreach (var player in hasBeenHit.Where(player => player == other))
            {
                otherHasBeenHit = true;
            }
            
            if (otherHasBeenHit) return;
            
            hasBeenHit.Add(other);
            other.SendMessage("TakeDamage", weaponData.damage);

            if(weaponData.name == "LightMelee")
            {
                PlayerHit.Invoke(true);
            } else if(weaponData.name == "HeavyMelee")
            {
                PlayerHit.Invoke(false);
            }
        }
    
    }
}
