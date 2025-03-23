
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class SprayParticle : MonoBehaviour
{
    [SerializeField] private GameObject sprayDecalPrefab; // Assign SprayDecal prefab in Inspector
    [SerializeField] private float decalOffset = 0.002f; // Small offset to prevent clipping
    [SerializeField] private float decalLifetime = 10f; // Decals disappear after some time
    
    public Color color;
    
    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero; // Stop movement
        rb.useGravity = false;

        ContactPoint contact = collision.contacts[0];
        Vector3 position = contact.point + contact.normal * decalOffset; // Offset to avoid z-fighting
        Quaternion rotation = Quaternion.LookRotation(-contact.normal); // Ensure proper alignment

        // Spawn the decal and parent it to the wall
        GameObject decal = Instantiate(sprayDecalPrefab, position, rotation);
        decal.transform.SetParent(collision.transform);
        decal.GetComponent<SpriteRenderer>().color = color;// Attach decal to the wall
        // Destroy(decal, decalLifetime);

        // Recycle the spray particle
        ObjectPoolManager.RecycleObject(gameObject);
    }
}





/*
 using System;
using UnityEngine;
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class SprayParticle : MonoBehaviour
{
    [SerializeField] private int splashCount = 50;
    [SerializeField] private float spreadRadius = 0.05f;
    [SerializeField] private float splashForce = 2f;

    [SerializeField] private GameObject sprayParticlePrefab;
    private void OnCollisionEnter(Collision other)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.zero;
        rb.useGravity = false;

        for (int i = 0; i < splashCount; ++i)
        {
            Vector3 spawnPosition = transform.position + UnityEngine.Random.insideUnitSphere * spreadRadius;
            GameObject droplet = ObjectPoolManager.SpawnObject(sprayParticlePrefab, spawnPosition, Quaternion.identity);

            Rigidbody splashRb = droplet.GetComponent<Rigidbody>();
            if (splashRb != null)
            {
                Vector3 randomDirection = (spawnPosition - transform.position).normalized;
                splashRb.AddForce(randomDirection * splashForce, ForceMode.Impulse);
            }
        }
        ObjectPoolManager.RecycleObject(gameObject);

    }
}
*/
