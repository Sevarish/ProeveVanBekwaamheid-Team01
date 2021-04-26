using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour, Damageable
{
    private int health = 100;
    [SerializeField]
    private int healthLossPerHit = 10;

    public void TakeDamage()
    {
        health -= healthLossPerHit;
        Debug.Log(health);
    }
}
