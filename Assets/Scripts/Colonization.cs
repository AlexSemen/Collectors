using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class Colonization : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private Flag _flag;
    [SerializeField] private LayerMask _interactionWithMouse;

    private Camera _camera;
    private RaycastHit _hit;
    private Ray _myRay;
    private float _raycastDistance;

    private void Awake()
    {
        _raycastDistance = 100;
        _camera = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            _myRay = Camera.main.ScreenPointToRay(Input.mousePosition);
           
            if (Physics.Raycast(_myRay, out _hit, _raycastDistance, _interactionWithMouse))
            {
                if (_hit.collider.TryGetComponent<Base>(out Base newBase))
                {
                    _base = newBase;
                }
                else
                {
                    if (_base != null)
                    {
                        if (_base.Flag == null)
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
