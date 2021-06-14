using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{

    private GameObject Player;
    private PlayerInput pi;
    void Start()
    {
        Player = GameObject.Find("Player");
        pi = Player.GetComponentInChildren<PlayerInput>();
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, Player.transform.position) < 1)
        {
            pi.AddAmmoToPlayer();
            Destroy(this.gameObject);
        }
    }
}
