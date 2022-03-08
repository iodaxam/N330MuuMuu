using System.Collections;
using UnityEngine;

namespace Weapons
{
    public class Weapon : MonoBehaviour
    {
        public Vector3 Position; // prefab tested attachment position
        public Quaternion Rotation; // prefab tested attachment rotation
        public Vector3 Scale; // prefab tested attachment scale
        public GameObject attachmentPoint; // where the weapon is going to attach to on the player prefab.
        public GameObject weaponPrefab; // prefab of the weapon 
        public float damage;
        public float attackTime; // For melee: how long the swing animation is; for ranged: time between shots.
        public string name; // for the combat script to set animations etc.
        
        protected bool Attacking;
        protected GameObject weapon;
        // Start is called before the first frame update
        void Start()
        {
            weapon = Instantiate(weaponPrefab, Vector3.zero, Quaternion.identity); // instantiate a new version of the prefab

            weapon.transform.parent = attachmentPoint.transform;
            weapon.transform.localPosition = Position;
            weapon.transform.localRotation = Rotation;
            weapon.transform.localScale = Scale;
        }
    
        public void Attack() // allow the parent class to tell the weapon it's attacking
        {
            Attacking = true;
        }
    }
}
