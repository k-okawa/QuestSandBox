using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using OVR.OpenVR;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class HitTest : MonoBehaviour
{
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private Transform _handTrans;
    [SerializeField] private Camera _hitTestCamera;
    [SerializeField] private GameObject _hitPointPrefab;
    [SerializeField] private float _rayCastDistance = 100f;
    [SerializeField] private float _screenWidth = 1920f;
    [SerializeField] private float _screenHeight = 1080f;

    private LineRenderer _lineRenderer;
    private List<Vector3> _meshVertices;
    private float _meshWidth, _meshHeight;

    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
        
        // 頂点を取得しワールド座標に変換
        _meshVertices = _meshFilter.mesh.vertices.Select(pos => _meshFilter.transform.TransformPoint(pos)).ToList();
        _meshWidth = _meshVertices[1].x - _meshVertices[0].x;
        _meshHeight = _meshVertices[2].y - _meshVertices[0].y;
    }

    void Update()
    {
        SetRay();
        if (OVRInput.GetDown(OVRInput.RawButton.RIndexTrigger))
        {
            RayCastTest();
        }
    }

    void SetRay()
    {
        var start = _handTrans.position;
        var end = start + _handTrans.forward * _rayCastDistance;
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, end);
    }

    void RayCastTest()
    {
        int layerMast = LayerMask.GetMask("RightEye");
        Ray screenRay = new Ray()
        {
            origin = _handTrans.position,
            direction = _handTrans.forward
        };
        if (Physics.Raycast(screenRay, out var screenHitInfo, _rayCastDistance, layerMast))
        {
            var hitPoint = screenHitInfo.point;
            float xDistance = hitPoint.x - _meshVertices[0].x;
            float yDistance = hitPoint.y - _meshVertices[0].y;
            float xRatio = xDistance / _meshWidth;
            float yRatio = yDistance / _meshHeight;
            Vector3 screenPos = Vector3.zero;
            screenPos.x = _screenWidth * xRatio;
            screenPos.y = _screenHeight * yRatio;
            Debug.Log(screenPos.ToString());

            var cameraRay = _hitTestCamera.ScreenPointToRay(screenPos);
            if (Physics.Raycast(cameraRay, out var cameraHitInfo))
            {
                var hitGo = Instantiate(_hitPointPrefab);
                hitGo.transform.position = cameraHitInfo.point + cameraHitInfo.normal * 0.01f;
                hitGo.transform.LookAt(cameraHitInfo.point + cameraHitInfo.normal);
            }
        }
    }
}
