using System;
using Cinemachine;
using UnityEngine;

namespace Cagri.Scripts.CameraControl
{
    public class CameraControl : MonoBehaviour
    {
        [SerializeField]private float panSpeed = 20f; 
        [SerializeField]private float panBorderThickness = 10f;
        [SerializeField]private Vector2 panMinLimit;
        [SerializeField]private Vector2 panMaxLimit;

       
        
        void Update()
        {
            MapPan();
        }
        
        private void MapPan()
        {
            Vector3 pos = transform.position;

            Vector3 mousePos = Input.mousePosition;

            if (Input.GetKey("w") || mousePos.y >= Screen.height - panBorderThickness)
            {
                pos.y += panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("s") || mousePos.y <= panBorderThickness)
            {
                pos.y -= panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("d") || mousePos.x >= Screen.width - panBorderThickness)
            {
                pos.x += panSpeed * Time.deltaTime;
            }
            if (Input.GetKey("a") || mousePos.x <= panBorderThickness)
            {
                pos.x -= panSpeed * Time.deltaTime;
            }
            
            pos.x = Mathf.Clamp(pos.x, panMinLimit.x, panMaxLimit.x);
            pos.y = Mathf.Clamp(pos.y, panMinLimit.y, panMaxLimit.y);
            
            transform.position = pos;
        }
    }
}