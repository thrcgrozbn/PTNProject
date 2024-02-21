using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Cagri.Scripts.GenericSystems.Utils
{
    public abstract class Utils
    {

        public const int sortingOrderDefault = 5000;
            
        public static bool IsPointerOverUIObject() {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }
        public static Vector3 GetMouseWorldPosition()
        {
            Vector3 vec = GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
            vec.z = 0f;
            return vec;
        }
        public static Vector3 GetMouseWorldPositionWithZ()
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, Camera.main);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Camera worldCamera)
        {
            return GetMouseWorldPositionWithZ(Input.mousePosition, worldCamera);
        }
        public static Vector3 GetMouseWorldPositionWithZ(Vector3 screenPos, Camera worldCamera)
        {
            Vector3 worldPos = worldCamera.ScreenToWorldPoint(screenPos);
            return worldPos;
        }


        public static TextMesh CreateWorldText(string text,Transform parent,Vector3 localPosition ,int fontSize ,Color color,TextAnchor textAnchor,TextAlignment textAlignment= TextAlignment.Center,int sortingOrder=0)
        {
            if (color ==null) color = Color.white;
      
            return CreateWorldText(parent, text, localPosition, fontSize, (Color)color, textAnchor, textAlignment, sortingOrder);
        }

        public static TextMesh CreateWorldText(Transform parent, string text, Vector3 localPos, int fontSize, Color color,
            TextAnchor textAnchor, TextAlignment textAlignment,int sortingOrder )
        {
            GameObject gameObject = new GameObject("World_Text", typeof(TextMesh));
            Transform transform = gameObject.transform;
            transform.SetParent(parent, false);
            transform.localPosition = localPos;
            TextMesh textMesh = gameObject.GetComponent<TextMesh>();
            textMesh.anchor = textAnchor;
            textMesh.alignment = textAlignment;
            textMesh.fontSize = fontSize;
            textMesh.color = color;
            textMesh.GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
            textMesh.text = text;
            return textMesh;
        }
        
        public static void CreateWorldTextPopup(string text, Vector3 localPosition) {
            CreateWorldTextPopup(null, text, localPosition, 15, Color.white, localPosition + new Vector3(0, 3), .5f);
        }
        
        // Create a Text Popup in the World
        public static void CreateWorldTextPopup(Transform parent, string text, Vector3 localPosition, int fontSize, Color color, Vector3 finalPopupPosition, float popupTime) {
            TextMesh textMesh = CreateWorldText(parent, text, localPosition, fontSize, color, TextAnchor.LowerLeft, TextAlignment.Left,  sortingOrderDefault);
            Transform transform = textMesh.transform;
            Vector3 moveAmount = (finalPopupPosition - localPosition) / popupTime;
            FunctionUpdater.Create(delegate () {
                transform.position += moveAmount * Time.deltaTime;
                popupTime -= Time.deltaTime;
                if (popupTime <= 0f) {
                    UnityEngine.Object.Destroy(transform.gameObject);
                    return true;
                } else {
                    return false;
                }
            }, "WorldTextPopup");
        }
    }
}