using System.Collections.Generic;
using UnityEngine;

namespace Cagri.Scripts
{
    public class SafeAreaControl : MonoBehaviour
    {
        public Canvas canvas;
        public List<RectTransform> rectTransformList;


        private void Start()
        {
            var safeArea = Screen.safeArea;

            var anchorMin = safeArea.position;
            var anchorMax = safeArea.position + safeArea.size;

            anchorMin.x /= canvas.pixelRect.width;
            anchorMin.y /= canvas.pixelRect.height;
            anchorMax.x /= canvas.pixelRect.width;
            anchorMax.y /= canvas.pixelRect.height;

            foreach (RectTransform rectTransform in rectTransformList)
            {
                rectTransform.anchorMax = anchorMax;
                rectTransform.anchorMin = anchorMin;
            }
        }

    }
}