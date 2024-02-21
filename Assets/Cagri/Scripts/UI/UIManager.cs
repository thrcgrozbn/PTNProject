using System;
using Cagri.Scripts.UI.InGamePanel;
using UnityEngine;

namespace Cagri.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        public GameObject inGamePanel;
        public EndGamePanel endGamePanel;
        private bool _endGame=false;
        
        
        public InformationPanel informationPanel;
        public ProductionPanel productionPanel;
        public ResourcesPanel resourcesPanel;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            ToggleEndGamePanel(_endGame);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _endGame = !_endGame;
                ToggleEndGamePanel(_endGame);
            }
        }

        private void ToggleEndGamePanel(bool endGame)
        {
            inGamePanel.SetActive(!endGame);
            endGamePanel.gameObject.SetActive(endGame);
        }
    }
}