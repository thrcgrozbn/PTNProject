using System.Collections;
using System.Collections.Generic;
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
        private int sign;
        [HideInInspector]public List<Transform> soldierHolderAroundBuildingList;
       

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
            CreateSpawnObjects();
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
            UIManager.instance.informationPanel.SetInformationPanel(buildingDescription,currentHealth,buildingSprite,this);
        }
        
        public void TakeDamage(int damageAmount)
        {
            currentHealth -= damageAmount;

            currentHealth = Mathf.Clamp(currentHealth, 0, buildingsTypeSo.totalHp);

            if (currentHealth<=0)
            {
                DestroyBuilding();
            }
        }

        public int GetCurrentHp()
        {
            return currentHealth;
        }


        private void DestroyBuilding()
        {
            foreach (Transform transform1 in soldierHolderAroundBuildingList)
            {
                if (transform1.childCount>0)
                {
                    transform1.GetChild(0).SetParent(null);
                }
            }
            DestroySelf();
            
        }

        
        
        
        private void CreateSpawnObjects()
        {
            int maximumTroopCount = buildingsTypeSo.height * buildingsTypeSo.width;
            for (int i = 0; i < maximumTroopCount; i++)
            {
                GameObject newObject = new GameObject(i.ToString());
                newObject.transform.SetParent(transform);
                Vector3 buildingCenter = transform.position +
                                         Vector3.up * buildingsTypeSo.height * .5f +
                                         Vector3.right * buildingsTypeSo.width * .5f;

                float y, x;

                switch (sign)
                {
                    case 0: // up
                        x = -buildingsTypeSo.height / 2f + (i % (maximumTroopCount / 4));
                        y = buildingsTypeSo.width / 2f;
                        if (i == maximumTroopCount / 4 - 1)
                        {
                            sign++;
                        }

                        y += .5f;
                        break;
                    case 1: // right
                        x = buildingsTypeSo.height / 2f;
                        y = buildingsTypeSo.width / 2f - (i % (maximumTroopCount / 4));
                        if (i == maximumTroopCount / 2 - 1)
                        {
                            sign++;
                        }

                        x += .5f;
                        break;
                    case 2: // down
                        x = buildingsTypeSo.height / 2f - (i % (maximumTroopCount / 4));
                        y = -buildingsTypeSo.width / 2f;
                        if (i == maximumTroopCount * 3 / 4 - 1)
                        {
                            sign++;
                        }

                        y -= .5f;
                        break;
                    case 3: // left
                        x = -buildingsTypeSo.height / 2f;
                        y = -buildingsTypeSo.width / 2f + (i % (maximumTroopCount / 4));
                        x -= .5f;
                        break;
                    default:
                        x = 0f;
                        y = 0f;
                        break;
                }


                Vector3 newPosition = new Vector3(buildingCenter.x + x, buildingCenter.y + y, 0f);
                newObject.transform.position = newPosition;
                newObject.transform.rotation = Quaternion.identity;
                SoldierHolderAroundBuilding soldierHolderAroundBuilding = newObject.AddComponent<SoldierHolderAroundBuilding>();
                soldierHolderAroundBuilding.myBuildings = this;
                soldierHolderAroundBuildingList.Add(newObject.transform);
            }
        }
    }
}
