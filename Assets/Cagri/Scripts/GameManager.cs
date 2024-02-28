using System;
using Cagri.Scripts.GridBuildingSystem.Buildings;
using Cagri.Scripts.UI;
using UnityEngine;

namespace Cagri.Scripts
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance;
        [HideInInspector]public bool loseGame;
        [HideInInspector]public Buildings mainBase;
        [Tooltip("This variable only runs at the start.")]
        public bool showGrid;
        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            if (!mainBase && !loseGame)
            {
                loseGame = true;
                UIManager.instance.ToggleEndGamePanel(loseGame);
            }
        }
    }
}