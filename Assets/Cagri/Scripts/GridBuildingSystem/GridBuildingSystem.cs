using System;
using System.Collections.Generic;
using System.Linq;
using Cagri.Scripts.GenericSystems.GridMap;
using Cagri.Scripts.GenericSystems.Pathfinding;
using Cagri.Scripts.GenericSystems.Utils;
using Cagri.Scripts.GridBuildingSystem.Buildings;
using Cagri.Scripts.GridBuildingSystem.Buildings.BuildingTypesSO;
using Cagri.Scripts.GridBuildingSystem.Buildings.Type;
using Cagri.Scripts.Resources;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Cagri.Scripts.GridBuildingSystem
{
    public class GridBuildingSystem : MonoBehaviour
    {
        public static GridBuildingSystem Instance { get; private set; }

        public event EventHandler OnSelectedChanged;
        public event EventHandler OnObjectPlaced;

        [HideInInspector] public Grid<Node> grid;
        [SerializeField] private List<BuildingsTypeSO> placedObjectTypeSOList = null;
        private BuildingsTypeSO buildingsTypeSo;
        private BuildingsTypeSO.Dir dir;

        [Header("EnemyTowerPos")] 
        public List<Vector3> enemyTowerPos;

        [HideInInspector] public List<EnemyTower> enemyTowerList;
        
        [Header("MainBase")] 
        public Vector3 mainBasePos;
        private void Awake()
        {
            Instance = this;
            buildingsTypeSo = null;
        }

        private void EnemyTowerUpdater()
        {
            if (enemyTowerList.Count<5)
            {
                Vector3 newPos = new Vector3(Random.Range(25,50), Random.Range(20,70), 0);
                PlaceBuildingsWithoutProduction(newPos,0);
            }
        }
        
        private void Start()
        {
            PlaceBuildingsWithoutProduction(mainBasePos,placedObjectTypeSOList.Count-1,true);
            foreach (Vector3 towerPos in enemyTowerPos)
            {
                PlaceBuildingsWithoutProduction(towerPos,0);
            }
        }

        private void PlaceBuildingsWithoutProduction(Vector3 pos,int index,bool mainBase=false)
        {
            BuildingsTypeSO pSO = placedObjectTypeSOList[index];
            grid.GetXY(pos,out int x, out int z);
            Vector2Int placedObjectOrigin = new Vector2Int(x, z);
            bool canBuild = true;
            List<Vector2Int> gridPositionList = pSO.GetGridPositionList(placedObjectOrigin, dir);
            foreach (Vector2Int gridPosition in gridPositionList) 
            {
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).IsItEmpty()) 
                {
                    canBuild = false;
                   
                    break;
                }
            }
            if (canBuild)
            {
                Build(placedObjectOrigin,gridPositionList,x,z,pSO,mainBase);
            }
        }


        public void BuildingTypeSelectionButtonClicked(int index)
        {
            DeselectObjectType();
            if (ResourcesControl.instance.GetEnergyAmount()<placedObjectTypeSOList[index].myPrice)
            {
                Utils.CreateWorldTextPopup("Insufficient Energy", Camera.main.transform.position + Camera.main.transform.forward * 10f);
                return;
            }
            buildingsTypeSo = placedObjectTypeSOList[index]; RefreshSelectedObjectType();
        }

        private void BuildControl()
        {
            if (buildingsTypeSo == null)
            {
                return;
            }
            Vector3 mousePosition = Utils.GetMouseWorldPosition();
            grid.GetXY(mousePosition, out int x, out int z);

            Vector2Int placedObjectOrigin = new Vector2Int(x, z);

            // Test Can Build
            List<Vector2Int> gridPositionList = buildingsTypeSo.GetGridPositionList(placedObjectOrigin, dir);
            bool canBuild = true;
            foreach (Vector2Int gridPosition in gridPositionList) 
            {
                if (grid.GetGridObject(gridPosition.x, gridPosition.y)==null)
                {
                    canBuild = false;
                    break;
                }
                if (!grid.GetGridObject(gridPosition.x, gridPosition.y).IsItEmpty()) 
                {
                    canBuild = false;
                    break;
                }
            }
            BuildingGhost2D.spriteRenderer.enabled = !canBuild;
            if (canBuild) 
            {
                if (Input.GetMouseButtonDown(0))
                {
                    if (Utils.IsPointerOverUIObject())
                    {
                        return;
                    }
                    ResourcesControl.instance.EnergyUpdate(-buildingsTypeSo.myPrice);
                    Build(placedObjectOrigin,gridPositionList,x,z,buildingsTypeSo);
                }
            } 
            else 
            {
                // Cannot build here
                //Utils.Utils.CreateWorldTextPopup("Cannot Build Here!", mousePosition);
                //Debug.Log("Cannot Build Here!");
            }
        }
        
        private void SortNeighbourList(Buildings.Buildings building)
        {
            List<Vector2> sortedV2List = building.neighbourNodeList
                .Select(node => new Vector2(node.x, node.y))
                .OrderBy(v2 => v2.x)
                .ThenBy(v2 => v2.y)
                .ToList();

            building.neighbourNodeList = sortedV2List
                .Select(v2 => building.neighbourNodeList
                    .First(node => node.x == v2.x && node.y == v2.y))
                .ToList();
        }
       
       
        private void Build(Vector2Int placedObjectOrigin, List<Vector2Int> gridPositionList,int x ,int z, BuildingsTypeSO pSO,bool mainBase=false)
        {
            Vector2Int rotationOffset = pSO.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, z) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();

            global::Cagri.Scripts.GridBuildingSystem.Buildings.Buildings buildings = global::Cagri.Scripts.GridBuildingSystem.Buildings.Buildings.Create(placedObjectWorldPosition, placedObjectOrigin, dir, pSO);
            buildings.transform.rotation = Quaternion.Euler(0, 0, -pSO.GetRotationAngle(dir));

            foreach (Vector2Int gridPosition in gridPositionList) 
            {
                grid.GetGridObject(gridPosition.x, gridPosition.y).SetPlacedObject(buildings);
            }

            foreach (Vector2Int gridPosList in gridPositionList)
            {
                grid.GetGridObject(gridPosList.x, gridPosList.y).SetBuildingNeighbourList(buildings);
            }
            SortNeighbourList(buildings);
            OnObjectPlaced?.Invoke(this, EventArgs.Empty);
            if (buildings.GetComponent<EnemyTower>())
            {
                enemyTowerList.Add(buildings.GetComponent<EnemyTower>());
            }
            if (mainBase)
            {
                GameManager.instance.mainBase = buildings;
                buildings.SelectObject();
            }
            DeselectObjectType();
        }
        private void Update() 
        {
            if (buildingsTypeSo)
            {
                BuildControl();
                if (Input.GetMouseButtonDown(1)) { DeselectObjectType(); }
            }
           EnemyTowerUpdater();
            
        /*if (Input.GetMouseButtonDown(1)) 
        {
            Vector3 mousePosition = Utils.Utils.GetMouseWorldPosition();
            Buildings buildings = grid.GetGridObject(mousePosition).GetPlacedObject();
            if (buildings != null) {
                // Demolish
                buildings.DestroySelf();

            }
        }*/
        }
    private void DeselectObjectType() {
        buildingsTypeSo = null; RefreshSelectedObjectType();
    }

    private void RefreshSelectedObjectType()
    {
        ObjectSelector.ObjectSelector.instance.controller = null;
        OnSelectedChanged?.Invoke(this, EventArgs.Empty);
    }


    public Vector2Int GetGridPosition(Vector3 worldPosition) {
        grid.GetXY(worldPosition, out int x, out int z);
        return new Vector2Int(x, z);
    }

    public Vector3 GetMouseWorldSnappedPosition() {
        Vector3 mousePosition = Utils.GetMouseWorldPosition();
        grid.GetXY(mousePosition, out int x, out int y);
        if (buildingsTypeSo != null) {
            Vector2Int rotationOffset = buildingsTypeSo.GetRotationOffset(dir);
            Vector3 placedObjectWorldPosition = grid.GetWorldPosition(x, y) + new Vector3(rotationOffset.x, rotationOffset.y) * grid.GetCellSize();
            return placedObjectWorldPosition;
        } 
        else 
        {
            return mousePosition;
        }
    }

    public Quaternion GetPlacedObjectRotation() {
        if (buildingsTypeSo != null) {
            return Quaternion.Euler(0, 0, -buildingsTypeSo.GetRotationAngle(dir));
        } else {
            return Quaternion.identity;
        }
    }
    
    public BuildingsTypeSO GetPlacedObjectTypeSO() {
        return buildingsTypeSo;
    }
    
    }
    
}