using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private const float MaxHealth = 100;
    private float m_CurrentHealth = 30;
    
    public Slider healthBar;
    private Camera m_Cam;

    // Start is called before the first frame update
    private void Start()
    {
        //healthBar = GetComponent<Slider>();
        healthBar.GetComponent<Slider>().maxValue = MaxHealth;
        m_CurrentHealth = MaxHealth;
        m_Cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        m_Cam = Camera.main;
        healthBar.GetComponent<Slider>().value = m_CurrentHealth;
        healthBar.transform.LookAt(m_Cam.transform);
    }

    public void TakeDamage(float damageAmount)
    {
        m_CurrentHealth -= Mathf.Clamp(damageAmount, 0, m_CurrentHealth);
        if (damageAmount > 1)
        {
            Debug.Log("Taking Damage: " + m_CurrentHealth);
        }
        if (m_CurrentHealth == 0)
        {
            SendMessageUpwards("Died");
        }
    }
}
