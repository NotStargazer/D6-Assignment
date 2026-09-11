using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    [SerializeField] private GameObject _outline;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;
    [Range(1, 100)] [SerializeField] private float _rotationImpulseMultiplier = 10;
    [Range(1, 100)] [SerializeField] private float _launchImpulseMultiplier = 10;

    private bool _hasRolled;
    private bool _lockDie;
    public int Score { get; private set; }
    public bool HasScore => Score > 0;

    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private void Awake()
    {
        originalPosition = transform.position;
        originalRotation = transform.rotation;
    }
    
    private void FixedUpdate()
    {
        if (_rigidbody.IsSleeping())
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            _collider.isTrigger = true;

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
        
        _rigidbody.AddTorque(new Vector3(
            Random.Range(0.5f, 2f) * Mathf.Sign(Random.value - 0.5f),
            Random.Range(0.5f, 2f) * Mathf.Sign(Random.value - 0.5f), 
            Random.Range(0.5f, 2f) * Mathf.Sign(Random.value - 0.5f)) * _rotationImpulseMultiplier * Random.Range(-1.2f, 1.2f), ForceMode.Impulse);
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

    public void DiceReset()
    {
        transform.position = originalPosition;
        transform.rotation = originalRotation;
        _hasRolled = false;
        _collider.isTrigger = false;
    }
}
