using System.Collections.Generic;
using Cagri.Scripts.GenericSystems.GridMap;
using UnityEngine;

namespace Cagri.Scripts.GenericSystems.Pathfinding
{
    public class Pathfinding
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;
        
        public static Pathfinding Instance { get; private set; }
        
        public Grid<Node> grid;
        private List<Node> openList;
        private List<Node> closedList;
        public Pathfinding(int width, int height)
        {
            Instance = this;
            GridBuildingSystem.GridBuildingSystem.Instance.grid =grid = new Grid<Node>(width, height, 1f, Vector3.zero, (g, x, y) => new Node(g, x, y));
        }

        public Grid<Node> GetGrid()
        {
            return grid;
        }

        public List<Node> FindPath(int startX, int startY, int endX, int endY)
        {
            Node startNode = grid.GetGridObject(startX, startY);
            Node endNode = grid.GetGridObject(endX, endY);
            
            if (startNode == null || endNode == null) {
                // Invalid Path
                return null;
            }

            
            openList = new List<Node>{startNode};
            closedList = new List<Node>();
            for (int x = 0; x < grid.GetWidth(); x++)
            {
                for (int y = 0; y < grid.GetHeight(); y++)
                {
                    Node node = grid.GetGridObject(x, y);
                    node.gCost = 99999999;
                    node.CalculateFCost();
                    node.cameFromNode = null;
                }
            }

            startNode.gCost = 0;
            startNode.hCost = CalculateDistanceCost(startNode,endNode);
            startNode.CalculateFCost();
            
            while (openList.Count > 0)
            {
                Node currentNode = GetLowestFCostNode(openList);
                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closedList.Add(currentNode);
                
                foreach (var neighbourNode in GetNeighbourList(currentNode))
                {
                    if (closedList.Contains(neighbourNode)) continue;
                    if (!neighbourNode.isWalkable)
                    {
                        closedList.Add(neighbourNode);
                        continue;
                    }
                    
                    int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                    
                    if (tentativeGCost<neighbourNode.gCost)
                    {
                        neighbourNode.cameFromNode = currentNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                        neighbourNode.CalculateFCost();
                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                }
            }
            //ouy of nodes on the openlist
            return null;
        }
        
        public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition)
        {
            grid.GetXY(startWorldPosition, out int startX, out int startY);
            grid.GetXY(endWorldPosition, out int endX, out int endY);

            List<Node> path = FindPath(startX, startY, endX, endY);
            if (path == null) 
            {
                return null;
            } 
            else 
            {
                List<Vector3> vectorPath = new List<Vector3>();
                foreach (Node pathNode in path) {
                    vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
                }
                return vectorPath;
            }
        }

        private List<Node> GetNeighbourList(Node currentNode)
        {
            List<Node> neighbourList = new List<Node>();
            if (currentNode.x -1 >=0)
            {
                //L
                neighbourList.Add(GetNode(currentNode.x -1,currentNode.y));
                //LD
                if(currentNode.y-1>=0) neighbourList.Add(GetNode(currentNode.x -1,currentNode.y-1));
                //LU
                if(currentNode.y+1<grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x -1,currentNode.y+1));
            }   
            if (currentNode.x +1 <grid.GetWidth())
            {
                //R
                neighbourList.Add(GetNode(currentNode.x +1,currentNode.y));
                //RD
                if(currentNode.y-1>=0) neighbourList.Add(GetNode(currentNode.x +1,currentNode.y-1));
                //RU
                if(currentNode.y+1<grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x +1,currentNode.y+1));
            } 
            //D
            if(currentNode.y-1>=0) neighbourList.Add(GetNode(currentNode.x,currentNode.y-1));
            //U
            if(currentNode.y-1<grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x,currentNode.y+1));

            return neighbourList;
        }

        public Node GetNode(int x,int y)
        {
            return grid.GetGridObject(x, y);
        }

        private List<Node> CalculatePath(Node endNode)
        {
            List<Node> path = new List<Node>();
            path.Add(endNode);
            Node currentNode = endNode;
            while (currentNode.cameFromNode != null)
            {
              path.Add(currentNode.cameFromNode);
              currentNode = currentNode.cameFromNode;
            }
            path.Reverse();
            return path;
        }

        private int CalculateDistanceCost(Node a,Node b)
        {
            int xDistance = Mathf.Abs(a.x - b.x);
            int yDistance = Mathf.Abs(a.y - b.y);
            int remaining = Mathf.Abs(xDistance - yDistance);
            return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
        }

        private Node GetLowestFCostNode(List<Node> pathNodeList)
        {
            Node lowestFCostNode = pathNodeList[0];
            for (int i = 0; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].fCost<lowestFCostNode.fCost)
                {
                    lowestFCostNode = pathNodeList[i];
                }
            }
            return lowestFCostNode;
        }
    }
}