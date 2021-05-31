using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : MonoBehaviour
{
    public GameObject muzzleFlashPrefab;
    public GameObject bulletPrefab; //Visual only.
    public GameObject sourceEmitter;
    private ParticleSystem bParticle;
    private ScreenShakeManager screenShake;
    public int clipCapacity = 32, clips = 7;
    [HideInInspector]
    public int fullCapacity = 0;
    public string targetTag;

    private readonly float fireRate = 0.12f;

    public AudioClip shootSfx;

    private void Awake()
    {
        fullCapacity = clipCapacity * clips;
    }

    private void Start()
    {
        screenShake = FindObjectOfType<ScreenShakeManager>();
        if (transform.CompareTag("Player"))
        {
            Quaternion rotParent = sourceEmitter.transform.parent.rotation;
            var bullet = Instantiate(bulletPrefab, sourceEmitter.transform.position, rotParent);
            bullet.transform.SetParent(sourceEmitter.transform.parent);
            bParticle = bullet.GetComponentInChildren<ParticleSystem>();
            bParticle.Stop();
        }
    }

    public void Shoot()
    {
        Quaternion rotParent = sourceEmitter.transform.parent.rotation;
        var MuzzleFlash = Instantiate(muzzleFlashPrefab, sourceEmitter.transform.position, rotParent);
        MuzzleFlash.transform.SetParent(sourceEmitter.transform.parent);
        Destroy(MuzzleFlash, 0.2f);
        screenShake.Shake(0.1f, 0.03f);
        SoundManager.Instance.Play(shootSfx);

        //Debug.DrawRay(sourceEmitter.transform.position, sourceEmitter.transform.forward, Color.cyan, 1);
        //Casts a raycast from the playerEmitter towards the crosshair (targetPoint).
        RaycastHit hit;
        if (Physics.Raycast(sourceEmitter.transform.position, sourceEmitter.transform.forward, out hit, Mathf.Infinity))
        {
            hit.transform.gameObject.GetComponent<Damageable>()?.TakeDamage(25);
        }
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    public void StartPtcl()
    {
        bParticle.Play();
    }

    public void StopPtcl()
    {
        bParticle.Stop();
    }
}
