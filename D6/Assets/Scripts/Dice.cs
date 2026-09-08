using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    
    private void FixedUpdate()
    {
        
    }

    public void Roll()
    {
        
    }

    private void OnValidate()
    {
        if (!_rigidbody)
        {
            TryGetComponent(out _rigidbody);
        }
    }
}
