using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class RaySync : MonoBehaviour
{
    [SerializeField] private Transform _rayTransform;
    
    private LineRenderer _lineRenderer;
    
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
    }

    private void Update()
    {
        // var lineRendererTransform = _lineRenderer.transform;
        // lineRendererTransform.localPosition = _rayTransform.localPosition;
        // lineRendererTransform.localRotation = _rayTransform.localRotation;
        // var origin = lineRendererTransform.position;
        // _lineRenderer.SetPosition(0, origin);
        // _lineRenderer.SetPosition(1, origin + lineRendererTransform.forward * 10f);
    }

    public void SetRay(Ray ray)
    {
        _lineRenderer.SetPosition(0, ray.origin);
        _lineRenderer.SetPosition(1, ray.origin + ray.direction);
    }
}
