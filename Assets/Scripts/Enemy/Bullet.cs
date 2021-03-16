using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    private void Awake()
    {
        StartCoroutine(DestroyDelay());
    }

    private void OnTriggerEnter(Collider Other)
    {
        Destroy(this.gameObject);
    }

    IEnumerator DestroyDelay()
    {
        yield return new WaitForSeconds(5f);
        Destroy(this.gameObject);
    }
}
