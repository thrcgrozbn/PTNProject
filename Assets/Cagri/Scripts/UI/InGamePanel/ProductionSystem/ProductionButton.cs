using Cagri.Scripts.GenericSystems.Utils;
using Cagri.Scripts.GridBuildingSystem.Buildings.Type;
using Cagri.Scripts.Resources;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Cagri.Scripts.UI.InGamePanel.ProductionSystem
{
    public class ProductionButton : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI productPriceTmp;
        [SerializeField] private TextMeshProUGUI productNameTmp;
        [SerializeField] private Image productImage;
        


        [HideInInspector]public BuildingsProducingSoldiers myBuildingsProducingSoldiers;
        private ProductSO _mySO;

        public void SetProduct(ProductSO productSO,BuildingsProducingSoldiers buildingsProducingSoldiers)
        {
            myBuildingsProducingSoldiers = buildingsProducingSoldiers;
            _mySO = productSO;
            productNameTmp.text = productSO.productName;
            productPriceTmp.text = productSO.productPrice.ToString();
            productImage.sprite = productSO.productSprite;
        }

        public void WhenSoldierProductionButtonClicked()
        {
            if (ResourcesControl.instance.GetEnergyAmount()<_mySO.productPrice)
            {
                Utils.CreateWorldTextPopup("Insufficient Energy", myBuildingsProducingSoldiers.transform.position);
                return;
            }
            myBuildingsProducingSoldiers.Product(_mySO);
        }
    }
}