using UnityEngine;
 
[RequireComponent(typeof(LineRenderer))]
public class MyRayPointer : OVRCursor
{
    [SerializeField] private GameObject _pointerObj;
    private LineRenderer _lineRenderer;
    private bool _isCalledSetCursorCurrentFrame;
 
    void Start()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = 2;
        _lineRenderer.enabled = false;
        _pointerObj.SetActive(false);
    }
 
    private void LateUpdate()
    {
        if (!_isCalledSetCursorCurrentFrame)
        {
            _lineRenderer.enabled = false;
            _pointerObj.SetActive(false);
        }
        _isCalledSetCursorCurrentFrame = false;
    }
 
    public override void SetCursorRay(Transform ray)
    {
        
    }
 
    public override void SetCursorStartDest(Vector3 start, Vector3 dest, Vector3 normal)
    {
        _lineRenderer.enabled = true;
        _isCalledSetCursorCurrentFrame = true;
        _pointerObj.SetActive(true);
        _lineRenderer.SetPosition(0, start);
        _lineRenderer.SetPosition(1, dest);
 
        transform.position = dest;
    }
}