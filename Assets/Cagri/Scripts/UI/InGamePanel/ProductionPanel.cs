using System;
using UnityEngine;
using UnityEngine.UI;

namespace Cagri.Scripts.UI.InGamePanel
{
    public class ProductionPanel : MonoBehaviour
    {
        public Scrollbar scrollBar;

        private void Start()
        {
            scrollBar.value = 1f;
        }
    }
}