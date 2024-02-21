using System;
using Cagri.Scripts.GridBuildingSystem.Buildings.BuildingTypesSO;
using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Cagri.Scripts.UI
{
    public class BuildingTypeSelectButton : MonoBehaviour
    {
        [SerializeField]private BuildingsTypeSO buildingObjectTypeSO;
        [SerializeField]private Image myImage;
        [SerializeField]private TextMeshProUGUI priceTMP;
        
        private int myPrice;

        private void Start()
        {
            myImage.sprite = buildingObjectTypeSO.mySprite;
            
            myPrice = buildingObjectTypeSO.myPrice;
            
            priceTMP.text = myPrice.ToString();

        }
    }
}