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

    public static List<Collector> Collectors { get; private set; }

    private Base _base;
    private Resource _targetResource;
    private Vector3 _target;
    private Rigidbody _rigidbody;
    private Condition _status;

    public Condition Status { get { return _status; } private set { _status = value; } }

    static Collector()
    {
        Collectors = new List<Collector>();
    }

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
        Collectors.Add(this);
    }

    private void FixedUpdate()
    {
        if (Status != Condition.Idle)
        {
            Move();

            switch (Status)
            {
                case Condition.GoesForResource:
                    if (Vector3.Distance(transform.position, _target) <= _interactionDistanceResource)
                    {
                        TakeResource();
                    }
                    break;

                case Condition.CarriesResourceToBase:
                    if (Vector3.Distance(transform.position, _target) < _interactionDistanceBase)
                    {
                        GiveResource();
                    }
                    break;
            }
        }
        
    }

    public void SetBase(Base newBase)
    {
        _base = newBase;
    }

    public void SetTargetResource(Resource targetResource)
    {
        targetResource.TrueBusy();
        SetTarget(targetResource.transform);
        Status = Condition.GoesForResource;
        _targetResource = targetResource;
    }

    private void Move()
    {
        transform.LookAt(_target);
        _rigidbody.velocity = transform.forward * _speed * Time.deltaTime;
    }

    private void TakeResource()
    {
        _targetResource.RaiseFromGround();
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
