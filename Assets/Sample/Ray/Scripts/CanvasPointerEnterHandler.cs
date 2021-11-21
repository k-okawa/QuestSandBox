 using System;
 using UnityEngine;
 using UnityEngine.EventSystems;

 public class CanvasPointerEnterHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
 {
     [SerializeField] private OVRInputModule _ovrInputModule;
     [SerializeField] private OVRRaycaster _ovrRaycaster;

     private bool _isEnter = false;
     
     public void OnPointerEnter(PointerEventData eventData)
     {
         _isEnter = true;
         Debug.Log($"PointerEnter!!{eventData.position.ToString()}");
     }

     public void Update()
     {
         if (!_isEnter)
         {
             return;
         }
     }

     public void OnPointerExit(PointerEventData eventData)
     {
         _isEnter = false;
         Debug.Log("PointerExit!!");
     }
 }