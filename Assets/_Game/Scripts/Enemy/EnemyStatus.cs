using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour, Damageable
{
    private int health = 100;

    public void TakeDamage()
    {
        health -= 10;
        Debug.Log(health);
    }
}
