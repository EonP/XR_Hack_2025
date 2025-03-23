using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class DummyParticleInstantiator : MonoBehaviour
{
    
    [SerializeField] private float minScale = 0.2f;
    [SerializeField] private float maxScale = 0.7f;
    
    private void OnEnable()
    {
        float randomScale = Random.Range(minScale, maxScale);
        //transform.localScale *= 2;
    }
    
}