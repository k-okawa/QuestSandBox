using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSync : MonoBehaviour
{
    [SerializeField] private Transform _origin;

    private Vector3 _delta;

    void Awake()
    {
        _delta = transform.localPosition - _origin.localPosition;
    }
    
    void Update()
    {
        transform.localPosition = _origin.localPosition + _delta;
    }
}
