using Cagri.Scripts.GenericSystems.Utils;
using Cagri.Scripts.Resources;
using Cagri.Scripts.UI.InGamePanel.ProductionSystem;
using CharacterController;
using UnityEngine;

namespace Cagri.Scripts.GridBuildingSystem.Buildings.Type
{
    public class BuildingsProducingSoldiers : Buildings
    {
        
        public void Product(ProductSO _productSO)
        {
            Transform newParent=null;
            foreach (Transform spawnTransform in soldierHolderAroundBuildingList)
            {
                if (spawnTransform.childCount==0)
                {
                    newParent = spawnTransform;
                    break;
                }
            }
            if (newParent==null)
            {
                Utils.CreateWorldTextPopup("At max capacity, clear the area.", transform.position);
                return;
            }
            ResourcesControl.instance.EnergyUpdate(-_productSO.productPrice);
            
            Transform product = Instantiate(_productSO.productPrefab,newParent).transform;
            Soldier newSoldier = product.GetComponent<Soldier>();
            newSoldier.speed = _productSO.speed;
            newSoldier.currentHealth = newSoldier.maxHp = _productSO.totalHp;
            newSoldier.damage = _productSO.totalDamage;
            newSoldier.HealthBarControl();
            product.localPosition = Vector3.zero;
            

        }
    }
}