using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FreeLookCamera : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CinemachineFreeLook _freeLook;
    private Image _camRotationImage;
    private string _mouseX = "Mouse X";
    private string _mouseY = "Mouse Y";
    
    
    public void OnDrag(PointerEventData eventData)
    {
        _camRotationImage = GetComponent<Image>();
        
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _camRotationImage.rectTransform,
                eventData.position,
                eventData.enterEventCamera,
                out Vector2 positionOut))
        {
            //Debug.Log(positionOut);
            _freeLook.m_XAxis.m_InputAxisName = _mouseX;
            _freeLook.m_YAxis.m_InputAxisName = _mouseY;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _freeLook.m_XAxis.m_InputAxisName = null;
        _freeLook.m_YAxis.m_InputAxisName = null;
        _freeLook.m_XAxis.m_InputAxisValue = 0;
        _freeLook.m_YAxis.m_InputAxisValue = 0;
    }
}
