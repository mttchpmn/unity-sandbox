using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 15f;

    private float nextTimeToFire = 0f;
    
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public Text text;
    public float impactForce;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            Shoot();
            nextTimeToFire = Time.time + 1f / fireRate;
        }
    }

    private void Shoot()
    {
        muzzleFlash.Play();
        
        var didCollide = Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out var raycastHit, range);
        if (!didCollide) return;
        
        Debug.Log($"Hit: {raycastHit.transform.name}");
            
        var target = raycastHit.transform.GetComponent<Target>();
        if (target == null) return;
            
        target.TakeDamage(damage);
        text.text = "You hit the target!";
        var impactObject = Instantiate(impactEffect, raycastHit.point, Quaternion.LookRotation(raycastHit.normal));
        Destroy(impactObject, 2f);

        if (raycastHit.rigidbody != null)
        {
            raycastHit.rigidbody.AddForce(-raycastHit.normal * impactForce);
        }

    }
}