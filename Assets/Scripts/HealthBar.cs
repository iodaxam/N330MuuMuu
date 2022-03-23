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
    //private Camera m_Cam;

    private GameObject MainCamera;

    // Start is called before the first frame update
    private void Start()
    {
        //healthBar = GetComponent<Slider>();
        healthBar.GetComponent<Slider>().maxValue = MaxHealth;
        m_CurrentHealth = MaxHealth;
        //m_Cam = Camera.main;

        MainCamera = GameObject.Find("Main Camera");

        GameObject.FindWithTag("GameManager").GetComponent<GameManager>().StartGame += StartGame;
        healthBar.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        healthBar.GetComponent<Slider>().value = m_CurrentHealth;
        healthBar.transform.LookAt(MainCamera.transform);

        //Debug.Log(m_Cam.transform.position);
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

    private void StartGame()
    {
        //m_Cam = Camera.main;
        healthBar.gameObject.SetActive(true);
    }
}
