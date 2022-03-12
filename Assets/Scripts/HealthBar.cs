using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private const float MaxHealth = 100;
    private float m_CurrentHealth = 30;
    private Slider healthBar;
    private GameObject GameManager;
    private GameManager GMscript;

    private Rigidbody rb;
    
    private Camera m_Cam;

    // Start is called before the first frame update
    private void Start()
    {
        healthBar = GameObject.Find("Health Bar").GetComponent<Slider>();
        m_CurrentHealth = MaxHealth;
        m_Cam = Camera.main;

        GameManager = GameObject.FindWithTag("GameManager");

        GMscript = GameManager.GetComponent<GameManager>();

        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if((GMscript.StopLightState == EnumStopLight.RedLightState) && (rb.velocity.normalized != Vector3.zero))
        {
            TakeDamage(10f);
            Debug.Log("Taking Stoplight Damage");
        }
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
