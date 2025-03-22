using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using Random = UnityEngine.Random;

public class Spray : MonoBehaviour
{
    [Header("___References___")]
    [SerializeField] private Transform sprayPoint;
    [SerializeField] private GameObject sprayParticlePrefab;
    [SerializeField] private XRGrabInteractable grabObject;
    
    [Header("___Spray Config___")]
    [SerializeField] private float spreadAngle = 10f;
    [SerializeField] private float sprayForce = 10f;
    
    private SprayState _state = SprayState.Idle;

    public void Start()
    {
        grabObject.activated.AddListener(OnActivate);
        grabObject.deactivated.AddListener(OnDeactivate);
        grabObject.selectExited.AddListener(OnDeactivate);
    }
    
    private void Update()
    {
        if (_state == SprayState.Spraying)
        {
            SpawnDroplet();
        }
    }

    private void SpawnDroplet()
    {

        // Calculate a random direction within a cone (± spreadAngle degrees in pitch & yaw)
        // Start with the forward direction
        Vector3 randomDirection = transform.forward;
        
        // Random rotation within the spreadAngle around X and Y
        float randomPitch = Random.Range(-spreadAngle, spreadAngle);
        float randomYaw   = Random.Range(-spreadAngle, spreadAngle);

        // Apply the random rotation to get a final spread direction
        randomDirection = Quaternion.Euler(randomPitch, randomYaw, 0f) * randomDirection;

        // Instantiate the droplet
        GameObject droplet = ObjectPoolManager.SpawnObject(sprayParticlePrefab, sprayPoint.position, Quaternion.LookRotation(randomDirection));
        
        // Optionally, if dropletPrefab has a Rigidbody, apply force so it flies outward
        Rigidbody rb = droplet.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(randomDirection * sprayForce, ForceMode.Impulse);
        }
    }
    
    private void OnDeactivate(SelectExitEventArgs arg0)
    {
        _state = SprayState.Idle;
    }

    private void OnDeactivate(DeactivateEventArgs arg0)
    {
        _state = SprayState.Idle;
    }

    private void OnActivate(ActivateEventArgs arg0)
    {
        _state = SprayState.Spraying;
    }


    private enum SprayState
    {
        Idle,
        Shaking,
        Spraying
    }
}
