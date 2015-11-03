﻿using UnityEngine;
using System.Collections;

public class Health : MonoBehaviour {

    public float HP;
	// Use this for initialization
	void Start ()
    {
	
	}

    private bool hasExploded = false;
	// Update is called once per frame
	void Update ()
    {
        if (HP <= 0 && !hasExploded)
        {
            hasExploded = true;
            gameObject.GetComponent<Renderer>().enabled = false;
            foreach(Renderer r in gameObject.GetComponentsInChildren<Renderer>())
            {
                r.enabled = false;
            }

            Destroy(gameObject, ProjectileDeathTimer);
            Explode();
        }
	}

    public GameObject explosionPrefab;
    public float ExplosionStrength = 2000.0f;
    public float ExplosionSize = 5.0f;
    public float ExplosionDeathTimer = 1.0f;
    public float ProjectileDeathTimer = 5.0f;
    void Explode()
    {
        GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity) as GameObject;
        Destroy(explosion, ExplosionDeathTimer);

        for (int i = -5; i <= 5; ++i)
        {
            for (int j = -5; j <= 5; ++j)
            {
                for (int k = -5; k <= 5; ++k)
                {
                    RaycastHit hit;
                    Ray ray = new Ray(transform.position, new Vector3(i, j, k));
                    Physics.Raycast(ray, out hit);
                    Rigidbody body = hit.rigidbody;
                    if (body)
                    {
                        Vector3 direction = body.transform.position - transform.position;
                        direction = direction.normalized;
                        body.AddForce(direction * ExplosionStrength);
                        //if(body.gameObject.tag != "Player" && body.gameObject.tag != "Projectile")
                        //    Destroy(body.gameObject, 1.0f);
                    }
                }
            }
        }

        /*
        RaycastHit[] hits = Physics.SphereCastAll(transform.position, ExplosionSize, Vector3.zero, 0.0f);

        foreach(RaycastHit hit in hits)
        {
            hit.rigidbody.AddExplosionForce(ExplosionStrength, transform.position, ExplosionSize);
        }
        */

    }

    void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.CompareTag("Projectile"))
        {
            HP -= 1;
            return;
        }
    }
}
