using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, Damageable
{
    public Slider healthUI;
    public int maxHealth = 30;
    [HideInInspector]
    public int health;
    public int damageTaken = 10;
    public float speed = 8.5f,
                 vision,
                 flashBatteryLife;
    public AssaultRifle HK416;
    public Taser X26;
    public bool Died;
    private GameObject visionCone;
    public Action GameOver;
    public AudioClip[] hurtSfx;
    public AudioClip[] deathSfx;
    public int currentWeapon; //0 is HK416, 1 is Taser

    private void Start()
    {
        health = maxHealth;
        Hostage[] hostages = GameObject.FindObjectsOfType<Hostage>();
        foreach(Hostage hostage in hostages)
        {
            hostage.SavedHostage += HealPlayer;
        }
    }

    private void SetVision()
    {

    }

    public void SwitchWeapon()
    {
        if (currentWeapon == 0) { currentWeapon = 1; Debug.Log(currentWeapon); return; }
        if (currentWeapon == 1) { currentWeapon = 0; Debug.Log(currentWeapon); return; }
    }

    private void DestroyPlayer()
    {
        //Destroy(this.gameObject);
        this.gameObject.SetActive(false);
    }
    public void TakeDamage(int _amount)
    {
        health -= _amount;

        UpdateHealthUI(health);

        if (health <= 0)
        {
            SoundManager.Instance.RandomSoundEffect(deathSfx);
            Invoke(nameof(DestroyPlayer), 0.5f);
            GameOver?.Invoke();
        } else
        {
            SoundManager.Instance.RandomSoundEffect(hurtSfx);
        }
    }

    public void TakeDamage()
    {
        health -= damageTaken;

        UpdateHealthUI(health);

        if (health <= 0)
        {
            SoundManager.Instance.RandomSoundEffect(deathSfx);
            Invoke(nameof(DestroyPlayer), 0.5f);
            GameOver?.Invoke();
        } else
        {
            SoundManager.Instance.RandomSoundEffect(hurtSfx);
        }
    }

    public void HealPlayer(int heal)
    {
        health += heal;
        UpdateHealthUI(health);
    }

    public void HealPlayer()
    {
        health += maxHealth;
        UpdateHealthUI(health);
    }

    private void UpdateHealthUI(int newHealth)
    {
        healthUI.value = newHealth;
    }

    private void ClampHealth()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
    }

    private void Update()
    {
        ClampHealth();
    }
}
