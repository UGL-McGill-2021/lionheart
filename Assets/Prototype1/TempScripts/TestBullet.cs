using System.Collections;
using System.Collections.Generic;
using Photon.Realtime;
using UnityEngine;

/// <summary>
/// Author: Ziqi Li
/// Bullet scripts
/// </summary>
public class TestBullet : MonoBehaviour
{
    public float Speed;
    public Player Owner { get; private set; }

    public void Start()
    {
        Destroy(gameObject, 3.0f);
    }

    public void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }

    public void InitializeBullet(Player owner, Vector3 originalDirection, float lag)
    {
        Owner = owner;

        transform.forward = originalDirection;

        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.velocity = originalDirection * Speed;
        rigidbody.position += rigidbody.velocity * lag;
    }


}
