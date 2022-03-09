using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private const float MaxHealth = 100;
    private float m_CurrentHealth;
    public Slider healthBar;
    
    private Camera m_Cam;

    // Start is called before the first frame update
    private void Start()
    {
        m_CurrentHealth = MaxHealth;
        m_Cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        healthBar.value = m_CurrentHealth / MaxHealth;
        healthBar.transform.LookAt(m_Cam.transform);
    }

    public void TakeDamage(float damageAmount)
    {
        m_CurrentHealth -= Mathf.Clamp(damageAmount, 0, damageAmount);
        if (m_CurrentHealth == 0)
        {
            SendMessageUpwards("Died");
        }
    }
}
