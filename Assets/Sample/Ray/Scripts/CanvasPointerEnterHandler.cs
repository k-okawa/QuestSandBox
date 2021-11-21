 using System;
 using UnityEngine;
 using UnityEngine.EventSystems;

 public class CanvasPointerEnterHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
 {
     [SerializeField] private Camera _camera;
     [SerializeField] private RaySync _raySync;

     private bool _isEnter = false;
     
     public void OnPointerEnter(PointerEventData eventData)
     {
         _isEnter = true;
         Debug.Log($"PointerEnter!!{eventData.position.ToString()}");
     }

     public void OnPointerClick(PointerEventData eventData)
     {
         Debug.Log("Click!!");
         var ray = _camera.ScreenPointToRay(eventData.position);
         _raySync.SetRay(ray);
     }

     public void OnPointerExit(PointerEventData eventData)
     {
         _isEnter = false;
         Debug.Log("PointerExit!!");
     }
 }