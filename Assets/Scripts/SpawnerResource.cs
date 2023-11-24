using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
//using Vector3 = UnityEngine.Vector3;

public class SpawnerResource : MonoBehaviour
{
    [SerializeField] private float _distanceX;
    [SerializeField] private float _distanceZ;
    [SerializeField] private float _spawnHeight;
    [SerializeField] private float _turn;
    [SerializeField] private int _quantity;
    [SerializeField] private float _distanceFromResource;
    [SerializeField] private float _distanceFromBase;
    [SerializeField] private Base _Base;
    [SerializeField] private Resource _resource;
    [SerializeField] private int _spawnAttempts;

    private int _failedAttempts;
    private Vector3 _curretTransformSpam;
    private List<Resource> _resources;

    public bool TrySpamResource()
    {
        _curretTransformSpam = new Vector3(Random.Range(-_distanceX, _distanceX), _spawnHeight, Random.Range(-_distanceZ, _distanceZ));

        if (Vector3.Distance(_Base.transform.position, _curretTransformSpam) < _distanceFromBase)
        {
            return false;
        }

        for (int i = 0; i < _resources.Count; i++)
        {
            if (Vector3.Distance(_resources[i].transform.position, _curretTransformSpam) < _distanceFromResource)
            {
                return false;
            }
        }

        Resource resource = Instantiate(_resource, _curretTransformSpam, Quaternion.identity);
        resource.transform.Rotate(0.0f, Random.Range(-_turn, _turn), 0.0f);
        resource.transform.SetParent(transform);

        _resources.Add(resource);

        return true;
    }

    public void Awake()
    {
        _failedAttempts = 0;
        _resources = new List<Resource>();
    }

    void Start()
    {
        for (int i = 0; i < _quantity; i++)
        {
            if(TrySpamResource())
            {
                _failedAttempts = 0;
            }
            else
            {
                i--;

                if(++_failedAttempts >= _spawnAttempts)
                {
                    break;
                }
            }
        }
    }
}
