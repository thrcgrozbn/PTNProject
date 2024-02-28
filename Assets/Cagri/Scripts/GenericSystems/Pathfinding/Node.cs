using System.Collections.Generic;
using Cagri.Scripts.GenericSystems.GridMap;
using Cagri.Scripts.GridBuildingSystem.Buildings;
using CharacterController;
using UnityEngine;

namespace Cagri.Scripts.GenericSystems.Pathfinding
{
    public class Node
    {
        public Grid<Node> grid;
        public int x;
        public int y;

        public int gCost;
        public int hCost;
        public int fCost;
        
        private Buildings Buildings;
        private Soldier soldier;
        
        [HideInInspector]public Node cameFromNode;
        public bool isItEmpty;
        public Node(Grid<Node> grid,int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
        }
        
        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }
        
        public override string ToString() {
            return x + ", " + y + "\n" + Buildings;
        }
        
        public void SetBuildingNeighbourList(Buildings building)
        {
            foreach (Node neighbour in Pathfinding.Instance.GetNeighbourList(this))
            {
                if (!building.neighbourNodeList.Contains(neighbour)&& neighbour.Buildings!=building)
                {
                    building.neighbourNodeList.Add(neighbour);
                }
            }
        }
       

       
        public void SetPlacedObject(Buildings buildings)
        {
            Buildings = buildings;
            grid.TriggerGridObjectChanged(x, y);
        }
        public void ClearPlacedObject()
        {
            Buildings = null;
            grid.TriggerGridObjectChanged(x, y);
        }
    
        public void SetSoldier(Soldier soldier)
        {
            this.soldier = soldier;
        }

        public void ClearSoldier()
        {
            soldier = null;
        }

        public Buildings GetPlacedObject() {
            return Buildings;
        }

        public bool IsItEmpty() {
           
            return Buildings==null && soldier == null;
        }

    }
}