using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private SpawnerCollectors _spawnerCollectors;
    [SerializeField] private ScanResources _scanResources;
    [SerializeField] private int _numberResources;

    public static List<Base> Bases { get; private set; }

    private int _checkDelay;
    private Resource _resourceTarget;
    private List<Collector> _collectorsIdle;
    private Collector _newCollectors;

    static Base()
    {
        Bases = new List<Base>();
    }

    private void Awake()
    {
        _collectorsIdle = new List<Collector>();
        _checkDelay = 1;
        _spawnerCollectors.transform.localPosition = new Vector3(0, -transform.position.y, 0);
        Bases.Add(this);
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

    private void CollectorSetTargetResource()
    {
        _resourceTarget = _scanResources.FindNearestFreeResource(transform);

        if (_resourceTarget != null && _collectorsIdle.Count > 0)
        {
            _collectorsIdle[0].SetTargetResource(_resourceTarget);
            _collectorsIdle.RemoveAt(0);
        }
    }

    private void SpamCollector()
    {
        _newCollectors = _spawnerCollectors.SpamCollector();

        if(_newCollectors != null)
        {
            _collectorsIdle.Add( _newCollectors );
            _newCollectors.SetBase(this);
        }
    }
}
