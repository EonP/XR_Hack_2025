using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Locomotion;
using Random = UnityEngine.Random;

public class Spray : MonoBehaviour
{
    [Header("___References___")]
    [SerializeField] private Transform sprayPoint;
    [SerializeField] private GameObject sprayParticlePrefab;
    [SerializeField] private XRGrabInteractable grabObject;
    [SerializeField] private GameObject visual;
    [SerializeField] private GameObject nozzle;
    [SerializeField] private ColorSO colorSo;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private AudioSource spraySound;
    [SerializeField] private AudioSource shakingSound;
    
    [Header("___Spray Config___")]
    [SerializeField] private float spreadAngle = 10f;
    [SerializeField] private float sprayForce = 10f;
    [SerializeField] private float canVelocity = 10f;
    [SerializeField] private float sprayBuffer = 0.3f;
    
    private SprayState _state = SprayState.Idle;
    Material newMaterial;
    Vector3 lastPosition;
    private Timer timer;

    public void Awake()
    {
        timer = new Timer(sprayBuffer);
    }

    public void Start()
    {
        grabObject.activated.AddListener(OnActivate);
        grabObject.deactivated.AddListener(OnDeactivate);
        grabObject.selectExited.AddListener(OnDeactivate);
        
        newMaterial = new Material(defaultMaterial);
        newMaterial.color = colorSo.color;
        
        visual.GetComponent<MeshRenderer>().material = newMaterial;
        nozzle.GetComponent<MeshRenderer>().material = newMaterial;
        
        lastPosition = transform.position;

        timer.OnTimerEnd += StopSFX;
    }

    private void StopSFX()
    {
        if (shakingSound.isPlaying) 
            shakingSound.Stop();
    }
    
    private void Update()
    {
        if (_state == SprayState.Spraying)
        {
            SpawnDroplet();
            timer.ForceEnd();
        }
        else
        {
            float delta = (transform.position - lastPosition).magnitude;
            Vector3 velocity = (transform.position - lastPosition) / Time.deltaTime;
            float vel =  velocity.magnitude;
            if (vel > 0)
            {
                Debug.Log(vel);
            }
            if (!shakingSound.isPlaying && vel > canVelocity)
            {
                if (delta < 1)
                    shakingSound.Play();
                timer.Restart(sprayBuffer);
            }
            
            lastPosition = transform.position;
            timer.Tick(Time.deltaTime);
        }
    }

    private void SpawnDroplet()
    {

        float xRot = Random.Range(-spreadAngle, spreadAngle);
        float yRot = Random.Range(-spreadAngle, spreadAngle);
        float zRot = Random.Range(-spreadAngle, spreadAngle);

        Vector3 randomDirection = Quaternion.Euler(xRot, yRot, zRot) * transform.forward;;
        // Instantiate the droplet
        GameObject droplet = ObjectPoolManager.SpawnObject(sprayParticlePrefab, sprayPoint.position, Quaternion.LookRotation(randomDirection));
        Material dropletMaterial = new Material(newMaterial);
        droplet.GetComponent<MeshRenderer>().material = dropletMaterial;
        droplet.GetComponent<SprayParticle>().color = colorSo.color;
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
        if (spraySound != null && spraySound.isPlaying)
        {
            spraySound.Stop();
        }
    }

    private void OnDeactivate(DeactivateEventArgs arg0)
    {
        _state = SprayState.Idle;
        if (spraySound != null && spraySound.isPlaying)
        {
            spraySound.Stop();
        }
    }

    private void OnActivate(ActivateEventArgs arg0)
    {
        _state = SprayState.Spraying;
        if (spraySound != null && !spraySound.isPlaying)
        {
            spraySound.Play();
        }
    }


    private enum SprayState
    {
        Idle,
        Shaking,
        Spraying
    }
}
