using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flag : MonoBehaviour
{
    [SerializeField] private Base _base;

    public void BuildBase(Collector collector)
    {
        Base newBase = Instantiate(_base, transform.position, Quaternion.identity);
        newBase.AddCollector(collector);
        Destroy(gameObject);
    }
}
