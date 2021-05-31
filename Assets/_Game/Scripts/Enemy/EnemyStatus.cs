using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatus : MonoBehaviour, Damageable
{
    private int health = 100;
    [SerializeField]

    public void TakeDamage(int _amount)
    {
        health -= _amount;
        Debug.Log(health);
    }
}
