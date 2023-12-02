using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private int _initialNumberCollectors;
    [SerializeField] private SpawnerCollectors _spawnerCollectors;
    [SerializeField] private ScanResources _scanResources;
    [SerializeField] private int _numberResources;

    private int _checkDelay;
    private int _priceCollector;
    private int _priceBase;
    private Resource _resourceTarget;
    private List<Collector> _collectorsIdle;
    private Collector _newCollector;

    public Flag Flag { get; private set; }

    public void TakeResource(Collector collector)
    {
        _numberResources++;
        _collectorsIdle.Add(collector);
    }

    public void AddCollector(Collector collector)
    {
        _collectorsIdle.Add(collector);
        collector.SetBase(this);
    }

    private void Awake()
    {
        _collectorsIdle = new List<Collector>();
        _priceCollector = 3;
        _priceBase = 5;
        _numberResources = _initialNumberCollectors * _priceCollector;
        _checkDelay = 1;
        _spawnerCollectors.transform.localPosition = new Vector3(0, -transform.position.y, 0);
    }

    private void Start()
    {
        for(int i = 0; i < _initialNumberCollectors; i++)
        {
            SpamCollector();
        }

        StartCoroutine(CheckStatus());
    }
   
    private IEnumerator CheckStatus()
    {
        var waitForDelay = new WaitForSeconds(_checkDelay);
        bool isWork = true;

        while (isWork)
        {
            if (Flag == null)
            {
                if (_numberResources >= _priceCollector)
                {
                    SpamCollector();
                }

                if (_collectorsIdle.Count > 0)
                {
                    CollectorSetTargetResource();
                }
            }
            else
            {
                if (_numberResources >= _priceBase)
                {
                    CollectorSetTargetFlag();
                }
                else
                {
                    if (_collectorsIdle.Count > 0)
                    {
                        CollectorSetTargetResource();
                    }
                }
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

    private void CollectorSetTargetFlag()
    {
        _numberResources -= _priceBase;

        if (Flag != null && _collectorsIdle.Count > 0)
        {
            _collectorsIdle[0].SetTargetFlag(Flag);
            _collectorsIdle.RemoveAt(0);
        }

        Flag = null;
    }

    private void SpamCollector()
    {
        _newCollector = _spawnerCollectors.SpamCollector();

        if(_newCollector != null)
        {
            _numberResources -= _priceCollector;
            AddCollector(_newCollector);
        }
    }

    public void SetFlag(Flag flag)
    {
        Flag = flag;
    }
}
