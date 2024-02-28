using System.Collections;
using System.Collections.Generic;
using Cagri.Scripts.GenericSystems.Pathfinding;
using Cagri.Scripts.GridBuildingSystem.Buildings.BuildingTypesSO;
using Cagri.Scripts.UI;
using Cagri.Scripts.UI.InGamePanel.ProductionSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cagri.Scripts.GridBuildingSystem.Buildings
{
    public class Buildings : MonoBehaviour 
    {

        public static Buildings Create(Vector3 worldPosition, Vector2Int origin, BuildingsTypeSO.Dir dir, BuildingsTypeSO buildingsTypeSo) {
            Transform placedObjectTransform = Instantiate(buildingsTypeSo.prefab, worldPosition, Quaternion.Euler(0, buildingsTypeSo.GetRotationAngle(dir), 0));
            
            Buildings buildings = placedObjectTransform.GetComponent<Buildings>();
            buildings.Setup(buildingsTypeSo, origin, dir);
            
            return buildings;
        }

        


        [HideInInspector] public BuildingsTypeSO buildingsTypeSo;
        private Vector2Int origin;
        private BuildingsTypeSO.Dir dir;
        
        public List<ProductSO> listOfProductionBuildingButtons;
        private string buildingDescription;
        private Sprite buildingSprite;
        private int currentHealth;
        private bool selectable;
        
        public List<Node> neighbourNodeList = new List<Node>();
        public List<Vector2> sortedxylist;
        private void Setup(BuildingsTypeSO buildingsTypeSo, Vector2Int origin, BuildingsTypeSO.Dir dir) {
            this.buildingsTypeSo = buildingsTypeSo;
            this.origin = origin;
            this.dir = dir;

            buildingDescription = buildingsTypeSo.buildingDescription;
            buildingSprite = buildingsTypeSo.mySprite;
            currentHealth = buildingsTypeSo.totalHp;
        }
        private void Start()
        {
            StartCoroutine(SelectableTime());
        }

        IEnumerator SelectableTime()
        {
            yield return new WaitForSeconds(.5f);
            selectable = true;
        }


        public List<Vector2Int> GetGridPositionList() {
            return buildingsTypeSo.GetGridPositionList(origin, dir);
        }

        public void DestroySelf() {
            foreach (Vector2Int vector2 in GetGridPositionList())
            {
                GridBuildingSystem.Instance.grid.GetGridObject(vector2.x,vector2.y).ClearPlacedObject();
            }
            Destroy(gameObject);
        }

        public override string ToString() {
            return buildingsTypeSo.nameString;
        }


        public void SelectPlacedObject()
        {
            if (!selectable)
            {
                return;
            }
            SelectObject();
        }

        public void SelectObject()
        {
            UIManager.instance.informationPanel.SetInformationPanel(buildingDescription,currentHealth,buildingSprite,this);
        }

        public void TakeDamage(int damageAmount)
        {
            currentHealth -= damageAmount;

            currentHealth = Mathf.Clamp(currentHealth, 0, buildingsTypeSo.totalHp);

            if (currentHealth<=0)
            {
                DestroySelf();
            }
        }

        public int GetCurrentHp()
        {
            return currentHealth;
        }
        
    }
}
