using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

[RequireComponent(typeof(Rigidbody))]
public class Collector : MonoBehaviour
{
    [SerializeField] private Resource _targetResource;
    [SerializeField] private Base Base;
    [SerializeField] private float _speed;
    [SerializeField] private Transform _resourceTransferLocation;

    private bool _busy;
    private Rigidbody _rigidbody;
    private Vector3 _target;

    public bool Busy { get { return _busy; } private set { _busy = value; } }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }


    public void SetTargetResource(Resource targetResource)
    {
        _targetResource = targetResource;
        Busy = true;
    }

    private void FixedUpdate()
    {
        if (_targetResource != null)
        {
            _target = new Vector3(_targetResource.transform.position.x, transform.position.y, _targetResource.transform.position.z);
            transform.LookAt(_target);
            _rigidbody.velocity = transform.forward * _speed * Time.deltaTime;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
