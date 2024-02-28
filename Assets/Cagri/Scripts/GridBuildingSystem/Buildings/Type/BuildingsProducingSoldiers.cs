using Cagri.Scripts.GenericSystems.Pathfinding;
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
            bool isThereAnyEmptyNode = false;
            Node spawnNode = null;
            foreach (Node neighbour in neighbourNodeList)
            {
                if (neighbour.IsItEmpty())
                {
                    spawnNode = neighbour;
                    isThereAnyEmptyNode = true;
                    break;
                }
            }
            if (!isThereAnyEmptyNode)
            {
                Utils.CreateWorldTextPopup("At max capacity, clear the area.", transform.position);
                return;
            }
            ResourcesControl.instance.EnergyUpdate(-_productSO.productPrice);
            Vector3 spawnPos = spawnNode.grid.PositionInTheMiddleOfGrid(spawnNode.x, spawnNode.y);
            Transform product = Instantiate(_productSO.productPrefab,spawnPos,Quaternion.Euler(0,180,0)).transform;
            Soldier newSoldier = product.GetComponent<Soldier>();
            newSoldier.speed = _productSO.speed;
            newSoldier.currentHealth = newSoldier.maxHp = _productSO.totalHp;
            newSoldier.damage = _productSO.totalDamage;
            newSoldier.HealthBarControl();
            spawnNode.SetSoldier(newSoldier);
            
        }
    }
}