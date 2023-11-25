using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Base : MonoBehaviour
{
    [SerializeField] private Collector _collector;
    [SerializeField] private float _spawnDistance;
    [SerializeField] private float _spawnHeight;
    [SerializeField] private float _spawnDistanceFromUnit;
    [SerializeField] private Transform _spawn—ircle;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _collectorsParent;
    [SerializeField] private Transform _resourceParent;
    [SerializeField] private int _numberResources;

    private int _checkDelay;
    private int _spawnAttempts;
    private int _failedAttempts;
    private bool _isSpawnPointFree;
    private float _spawnTurn;
    private Resource _resourceTarget;
    private List<Resource> _resource;
    private static List<Collector> _collectors;
    private List<Collector> _collectorsIdle;

    private void Awake()
    {
        _collectors = new List<Collector>();
        _collectorsIdle = new List<Collector>();
        _checkDelay = 1;
        _spawnAttempts = 10;
        _spawnTurn = 360;
        _spawn—ircle.localPosition = new Vector3(0, -transform.position.y, 0);
        _spawnPoint.localPosition = new Vector3(0, _spawnHeight, _spawnDistance);
    }

    private void Start()
    {
        SpamCollector();
        SpamCollector();
        SpamCollector();

        StartCoroutine(CheckStatus());
    }

    public void TakeResource(Collector collector)
    {
        _numberResources++;
        _collectorsIdle.Add(collector);
    }

    private IEnumerator CheckStatus()
    {
        var waitForDelay = new WaitForSeconds(_checkDelay);
        bool isWork = true;

        while (isWork)
        {
            if (_collectorsIdle.Count > 0)
            {
                CollectorSetTargetResource();
            }

            yield return waitForDelay;
        }
    }

    private void SpamCollector()
    {
        _failedAttempts = 0;

        do
        {
            _isSpawnPointFree = true;

            _spawn—ircle.Rotate(0.0f, Random.Range(-_spawnTurn, _spawnTurn), 0.0f);

            for (int i = 0; i < _collectors.Count; i++)
            {
                if (Vector3.Distance(_collectors[i].transform.position, _spawnPoint.position) < _spawnDistanceFromUnit)
                {
                    _isSpawnPointFree = false;
                }
            }
        }while(++_failedAttempts < _spawnAttempts && _isSpawnPointFree == false);

        if (_isSpawnPointFree) 
        {
            Collector collector = Instantiate(_collector, _spawnPoint.position, Quaternion.identity);
            collector.transform.localRotation = _spawn—ircle.localRotation;
            collector.transform.SetParent(_collectorsParent);
            _collectors.Add(collector);
            _collectorsIdle.Add(collector);

            collector.SetBase(this);
        }
    }

    private void CollectorSetTargetResource()
    {
        _resourceTarget = null;

        _resource = _resourceParent.GetComponentsInChildren<Resource>().ToList();

        foreach (var resource in _resource)
        {
            if(resource.Busy == false && (_resourceTarget == null || Vector3.Distance(transform.position, _resourceTarget.transform.position) > Vector3.Distance(transform.position, resource.transform.position)))
            {
                _resourceTarget = resource;
            }
        }

        if (_resourceTarget != null && _collectorsIdle.Count > 0)
        {
            _collectorsIdle[0].SetTargetResource(_resourceTarget);
            _collectorsIdle.RemoveAt(0);
        }
    }
}
