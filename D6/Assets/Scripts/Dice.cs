using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    [SerializeField] private GameObject _outline;
    [SerializeField] private Rigidbody _rigidbody;
    [Range(1, 100)] [SerializeField] private float _rotationImpulseMultiplier = 10;
    [Range(1, 100)] [SerializeField] private float _launchImpulseMultiplier = 10;

    private bool _hasRolled;
    private bool _lockDie;
    
    public int Score { get; private set; }
    
    private void FixedUpdate()
    {
        if (_rigidbody.IsSleeping())
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;

            if (Score == 0)
            {
                CalculateRoll();
            }
        }
    }

    private void CalculateRoll()
    {
        var allDirections = new[]
        {
            -transform.up,
            transform.forward,
            -transform.right,
            -transform.forward,
            transform.right,
            transform.up,
        };

        var closest = 0;
        var closestValue = -1f;

        for (var i = 0; i < allDirections.Length; i++)
        {
            var dot = Vector3.Dot(allDirections[i], Vector3.up);
            if (dot > closestValue)
            {
                closest = i;
                closestValue = dot;
            }
        }

        Score = closest + 1;
        Debug.Log($"Score calculation: {Score}");
    }

    public void ResetDie()
    {
        if (!_lockDie)
        {
            _hasRolled = false;
            Score = 0;
        }
    }

    public void Roll()
    {
        if (_hasRolled)
        {
            return;
        }

        _hasRolled = true;
        _rigidbody.constraints = RigidbodyConstraints.None;
        
        _rigidbody.AddTorque(Random.insideUnitSphere * _rotationImpulseMultiplier, ForceMode.Impulse);
        _rigidbody.AddForce(Vector3.up * _launchImpulseMultiplier, ForceMode.Impulse);
    }

    private void OnValidate()
    {
        if (!_rigidbody)
        {
            TryGetComponent(out _rigidbody);
        }
    }

    private void OnMouseUp()
    {
        _lockDie = !_lockDie;
        _outline.SetActive(!_lockDie);
    }
}
