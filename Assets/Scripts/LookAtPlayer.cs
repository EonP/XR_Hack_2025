using System;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void Update()
    {
        transform.forward = transform.position - player.transform.position;
    }
}
