using System;
using UnityEngine;

public class TakeDamage : MonoBehaviour
{
    [SerializeField] private CapsuleCollider2D collider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
