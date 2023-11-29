using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SpawnerResource : MonoBehaviour
{
    [SerializeField] private Resource _resource;
    [SerializeField] private int _spawnInitial;
    [SerializeField] private int _spawnMin;
    [SerializeField] private int _spawnMax;
    [SerializeField] private int _spawnDelay;
    [SerializeField] private float _distance;
    [SerializeField] private float _spawnHeight;
    [SerializeField] private float _turn;
    [SerializeField] private float _distanceFromResource;
    [SerializeField] private float _distanceFromBase;

    private int _spawnAttempts;
    private int _failedAttempts;
    private Vector3 _curretTransformSpam;

    private void Awake()
    {
        _spawnAttempts = 10;
        _failedAttempts = 0;
    }

    private void Start()
    {
        SpamResourcesNumber(_spawnInitial);
        StartCoroutine(SpamResources());
    }

    private IEnumerator SpamResources()
    {
        bool isWork = true;
        var waitForDelay = new WaitForSeconds(_spawnDelay);

        while (isWork)
        {
            SpamResourcesNumber(Random.Range(_spawnMin, _spawnMax + 1));
            yield return waitForDelay;
        }
    }

    private void SpamResourcesNumber(int quantity)
    {
        _failedAttempts = 0;

        for (int i = 0; i < quantity; i++)
        {
            if (TrySpamResource())
            {
                _failedAttempts = 0;
            }
            else
            {
                i--;

                if (++_failedAttempts >= _spawnAttempts)
                {
                    break;
                }
            }
        }
    }

    private bool TrySpamResource()
    {
        _curretTransformSpam = new Vector3(Random.Range(-_distance, _distance), _spawnHeight, Random.Range(-_distance, _distance));

        for (int i = 0; i < Base.Bases.Count; i++)
        {
            if (Vector3.Distance(Base.Bases[i].transform.position, _curretTransformSpam) < _distanceFromResource)
            {
                return false;
            }
        }

        for (int i = 0; i < Resource.ResourcesOnEarth.Count; i++)
        {
            if (Vector3.Distance(Resource.ResourcesOnEarth[i].transform.position, _curretTransformSpam) < _distanceFromResource)
            {
                return false;
            }
        }

        Resource resource = Instantiate(_resource, _curretTransformSpam, Quaternion.identity);
        resource.transform.Rotate(0.0f, Random.Range(-_turn, _turn), 0.0f);
        resource.transform.SetParent(transform);

        return true;
    }
}
