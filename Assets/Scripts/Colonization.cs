using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Colonization : MonoBehaviour
{
    private Camera _camera;
    private RaycastHit _hit;
    private Ray _myRay;
    [SerializeField] private Base _base;
    [SerializeField] private Flag _flag;
    [SerializeField] private LayerMask _interactionWithMouse;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            Debug.DrawRay(_myRay.origin, _myRay.direction * 10, Color.yellow);
            if (Physics.Raycast(_myRay, out _hit, 100, _interactionWithMouse))
            {
                Debug.Log(_hit.collider.name);

                if (_hit.collider.TryGetComponent<Base>(out Base @base))
                {
                    _base = @base;
                }
                else
                {
                    if(_base != null)
                    {
                        if(_base.Flag == null)
                        {
                            _base.SetFlag(Instantiate(_flag, _hit.point, Quaternion.identity));
                        }
                        else
                        {
                            _base.Flag.transform.position = _hit.point;
                        }
                        _base = null;
                    }
                }
            }
        }
    }
}
