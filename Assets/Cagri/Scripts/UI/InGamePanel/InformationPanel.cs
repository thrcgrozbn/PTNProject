using System;
using Cagri.Scripts.GridBuildingSystem.Buildings;
using Cagri.Scripts.GridBuildingSystem.Buildings.Type;
using Cagri.Scripts.UI.InGamePanel.ProductionSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Cagri.Scripts.UI.InGamePanel
{
    public class InformationPanel : MonoBehaviour
    {
        [SerializeField]private Image informationImage;
        [SerializeField]private TextMeshProUGUI descriptionText;
        [SerializeField]private GameObject productionButtonHolder;
        public ProductionButton productionButtonPrefab;
        private Buildings currentBuildings;
        public void SetInformationPanel(string description,int currentHealth, Sprite informationSprite,Buildings buildings)
        {
            CleanUpOldProduction();
            currentBuildings = buildings;
            descriptionText.text = description+" \n\nHP = "+currentHealth;
            informationImage.sprite = informationSprite;
            if (buildings.listOfProductionBuildingButtons.Count>0)
            {
               SetProductionButtons((BuildingsProducingSoldiers)buildings);
            }
        }
        
        private void Update()
        {
            if (currentBuildings)
            {
                descriptionText.text = currentBuildings.buildingsTypeSo.buildingDescription+" \n\nHP = "+currentBuildings.GetCurrentHp();
            }
        }

        private void SetProductionButtons(BuildingsProducingSoldiers buildingsProducingSoldiers)
        {
            foreach (ProductSO productSo in buildingsProducingSoldiers.listOfProductionBuildingButtons)
            {
                
                ProductionButton productionButton = Instantiate(productionButtonPrefab, productionButtonHolder.transform);
                productionButton.SetProduct(productSo,buildingsProducingSoldiers);
            }
        }

        private void CleanUpOldProduction()
        {
            for (int i = 0; i < productionButtonHolder.transform.childCount; i++)
            {
                Destroy(productionButtonHolder.transform.GetChild(i).gameObject);
            }
        }
    }
}