using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class ScanResources : MonoBehaviour
{
    private Resource _resourceTarget;
    public Resource FindNearestFreeResource(Transform transform)
    {
        _resourceTarget = null;

        foreach (var resource in Resource.ResourcesOnEarth)
        {
            if (resource.Busy == false && (_resourceTarget == null || Vector3.Distance(transform.position, _resourceTarget.transform.position) > Vector3.Distance(transform.position, resource.transform.position)))
            {
                _resourceTarget = resource;
            }
        }

        return _resourceTarget;
    }
}
