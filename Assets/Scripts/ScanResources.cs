using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class ScanResources : MonoBehaviour
{
    [SerializeField] private float _distanceScan;

    private Resource _resourceTarget;
    private Resource _newResourceTarget;
    private Collider[] _hitColliders;

    public Resource FindNearestFreeResource(Transform transform)
    {
        _resourceTarget = null;

        _hitColliders = Physics.OverlapSphere(transform.position, _distanceScan);

        foreach (Collider collider in _hitColliders)
        {
            if (collider.TryGetComponent<Resource>(out _newResourceTarget))
            {
                if (_newResourceTarget.Busy == false &&
                    (_resourceTarget == null || Vector3.Distance(transform.position, _resourceTarget.transform.position) > Vector3.Distance(transform.position, _newResourceTarget.transform.position)))
                {
                    _resourceTarget = _newResourceTarget;
                }
            }
        }

        return _resourceTarget;
    }
}
