using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Weapon/Melee")]
public class WeaponData : ScriptableObject
{
    [Header("Info")] 
    public new string name;

    [Header("Attacking")] 
    public float damage;
    
    [Header("Animation")]
    public float attackTime;

    [HideInInspector] 
    public bool attacking;
}