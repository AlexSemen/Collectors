using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
public class Collector : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _interactionDistanceResource;
    [SerializeField] private float _interactionDistanceBase;
    [SerializeField] private Transform _resourceTransferLocation;

    private Base _base;
    private Resource _targetResource;
    private Vector3 _target;
    private Rigidbody _rigidbody;
    private Condition _status;

    public Condition Status { get { return _status; } private set { _status = value; } }

    public enum Condition
    {
        GoesForResource,
        CarriesResourceToBase,
        Idle
    }

    private void Awake()
    {
        Status = Condition.Idle;
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        switch (Status)
        {
            case Condition.GoesForResource:
                if (_targetResource != null)
                {
                    Move();

                    if (Vector3.Distance(transform.position, _target) <= _interactionDistanceResource)
                    {
                        TakeResource();
                    }
                }
                break; 
            
            case Condition.CarriesResourceToBase:
                Move();

                if (Vector3.Distance(transform.position, _target) < _interactionDistanceBase)
                {
                    GiveResource();
                }
                break;
            
            case Condition.Idle:
                break;
        }
    }

    public void SetBase(Base newBase)
    {
        _base = newBase;
    }

    private void Move()
    {
        transform.LookAt(_target);
        _rigidbody.velocity = transform.forward * _speed * Time.deltaTime;
    }

    public void SetTargetResource(Resource targetResource)
    {
        targetResource.TrueBusy();
        SetTarget(targetResource.transform);
        Status = Condition.GoesForResource;
        _targetResource = targetResource;
    }

    private void TakeResource()
    {
        _targetResource.transform.position = _resourceTransferLocation.position;
        _targetResource.transform.SetParent(_resourceTransferLocation);
        SetTarget(_base.transform);
        Status = Condition.CarriesResourceToBase;
    }

    private void GiveResource()
    {
        Destroy(_targetResource.gameObject);
        Status = Condition.Idle;
        _base.TakeResource(this);
    }

    private void SetTarget(Transform transform)
    {
        _target = new Vector3(transform.transform.position.x, transform.position.y, transform.transform.position.z);
    }
}
