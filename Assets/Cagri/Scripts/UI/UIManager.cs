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
        private bool _toggle=false;
        
        
        public InformationPanel informationPanel;
        public ProductionPanel productionPanel;
        public ResourcesPanel resourcesPanel;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            ToggleEndGamePanel(_toggle);
        }

        private void Update()
        {
            if (GameManager.instance.loseGame)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _toggle = !_toggle;
                ToggleEndGamePanel(_toggle);
            }
        }

        public void ToggleEndGamePanel(bool toggle)
        {
            Time.timeScale = toggle ? 0 : 1;
            inGamePanel.SetActive(!toggle);
            endGamePanel.gameObject.SetActive(toggle);
        }
    }
}