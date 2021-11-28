using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class HitTest : MonoBehaviour
{
    // QuadのMesh
    [SerializeField] private MeshFilter _meshFilter;
    // レイを飛ばす手のTransform
    [SerializeField] private Transform _handTrans;
    // スクリーンを映しているカメラ
    [SerializeField] private Camera _hitTestCamera;
    // QuadにアタッチされているMeshCollider
    [SerializeField] private MeshCollider _screenMeshCollider;
    // レイが当たっている位置に表示するオブジェクト
    [SerializeField] private GameObject _hitPointPrefab;
    // スクリーンとのレイキャストの最大距離
    [SerializeField] private float _rayCastDistance = 100f;
    // スクリーンの解像度横
    [SerializeField] private float _screenWidth = 1920f;
    // スクリーンの解像度縦
    [SerializeField] private float _screenHeight = 1080f;

    // レイを表示する用のLineRenderer
    private LineRenderer _lineRenderer;
    // Meshの４頂点をワールド座標に変換して格納するためのリスト
    private List<Vector3> _meshVertices;
    // Quadメッシュの幅と高さ
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
            // 右手コントローラーの人差し指のボタンが押されたときに判定
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
        Ray screenRay = new Ray()
        {
            origin = _handTrans.position,
            direction = _handTrans.forward
        };
        if (_screenMeshCollider.Raycast(screenRay, out var screenHitInfo, _rayCastDistance))
        {
            var p = screenHitInfo.point;
            float xDistance = p.x - _meshVertices[0].x;
            float yDistance = p.y - _meshVertices[0].y;
            float xRatio = xDistance / _meshWidth;
            float yRatio = yDistance / _meshHeight;
            Vector3 sp = Vector3.zero;
            sp.x = _screenWidth * xRatio;
            sp.y = _screenHeight * yRatio;

            var cameraRay = _hitTestCamera.ScreenPointToRay(sp);
            if (Physics.Raycast(cameraRay, out var cameraHitInfo))
            {
                var hitGo = Instantiate(_hitPointPrefab);
                // Zファイティングが発生するためヒットした位置から少し浮かして配置
                hitGo.transform.position = cameraHitInfo.point + cameraHitInfo.normal * 0.01f;
                hitGo.transform.LookAt(cameraHitInfo.point + cameraHitInfo.normal);
            }
        }
    }
}
