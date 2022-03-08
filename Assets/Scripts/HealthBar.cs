using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public float MaxHealth = 100;
    private float CurrentHealth;
    public Slider healthBar;
    
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        healthBar.value = CurrentHealth / MaxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        CurrentHealth -= Mathf.Clamp(damageAmount, 0, damageAmount);
        if (CurrentHealth == 0)
        {
            SendMessageUpwards("Died");
        }
        else
        {
            
            GetComponentInParent<Animator>().SetBool("GotHit", true);
            StartCoroutine(nameof(StaggerTime));
        }
    }
    
    private IEnumerator StaggerTime()
    {
        yield return new WaitForSeconds(0.5f);
        GetComponentInParent<Animator>().SetBool("GotHit", false);
    }
}
