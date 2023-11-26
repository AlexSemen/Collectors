using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerCollectors : MonoBehaviour
{
    [SerializeField] private Collector _collector;
    [SerializeField] private float _spawnDistance;
    [SerializeField] private float _spawnHeight;
    [SerializeField] private float _spawnDistanceFromUnit;
    [SerializeField] private Transform _spawnPoint;

    private float _spawnTurn;
    private int _spawnAttempts;
    private int _failedAttempts;
    private bool _isSpawnPointFree;

    private void Awake()
    {
        _spawnAttempts = 10;
        _spawnTurn = 180;
        _spawnPoint.localPosition = new Vector3(0, _spawnHeight, _spawnDistance);
    }

    public Collector SpamCollector()
    {
        _failedAttempts = 0;

        do
        {
            _isSpawnPointFree = true;

            transform.Rotate(0.0f, Random.Range(-_spawnTurn, _spawnTurn), 0.0f);

            for (int i = 0; i < Collector.Collectors.Count; i++)
            {
                if (Vector3.Distance(Collector.Collectors[i].transform.position, _spawnPoint.position) < _spawnDistanceFromUnit)
                {
                    _isSpawnPointFree = false;
                }
            }
        } while (++_failedAttempts < _spawnAttempts && _isSpawnPointFree == false);

        if (_isSpawnPointFree)
        {
            Collector collector = Instantiate(_collector, _spawnPoint.position, Quaternion.identity);
            collector.transform.localRotation = transform.localRotation;

            return collector;
        }

        return null;
    }
}
