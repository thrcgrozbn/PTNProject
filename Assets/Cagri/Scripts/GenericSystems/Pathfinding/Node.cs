using Cagri.Scripts.GenericSystems.GridMap;
using Cagri.Scripts.GridBuildingSystem.Buildings;
using UnityEngine;

namespace Cagri.Scripts.GenericSystems.Pathfinding
{
    public class Node
    {
        private Grid<Node> grid;
        public int x;
        public int y;

        public int gCost;
        public int hCost;
        public int fCost;
        
        private Buildings Buildings;
        
        [HideInInspector]public bool isWalkable;
        [HideInInspector]public Node cameFromNode;
        [HideInInspector]public bool canBuild=true;
        public Node(Grid<Node> grid,int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            isWalkable = true;
        }
        
        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
        
        public override string ToString() {
            return x + ", " + y + "\n" + Buildings;
        }
        public void SetPlacedObject(Buildings buildings)
        {
            canBuild = false;
            this.Buildings = buildings;
            isWalkable = false;
            grid.TriggerGridObjectChanged(x, y);
        }
        public void ClearPlacedObject()
        {
            canBuild = true;
            Buildings = null;
            isWalkable = true;
            grid.TriggerGridObjectChanged(x, y);
        }
        public Buildings GetPlacedObject() {
            return Buildings;
        }

        public bool CanBuild() {
            return canBuild;
        }
        
    }
}