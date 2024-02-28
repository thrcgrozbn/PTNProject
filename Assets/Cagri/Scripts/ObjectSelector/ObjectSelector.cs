using Cagri.Scripts.GenericSystems.Pathfinding;
using Cagri.Scripts.GenericSystems.Utils;
using Cagri.Scripts.GridBuildingSystem.Buildings;
using Cagri.Scripts.GridBuildingSystem.Buildings.Type;
using Cagri.Scripts.Soldiers;
using Unity.Mathematics;
using UnityEngine;

namespace Cagri.Scripts.ObjectSelector
{
    public class ObjectSelector : MonoBehaviour
    {
        public static ObjectSelector instance;
        private Pathfinding pathfinding;
        public PathfindingMovement controller;
        private Node currentAttackNode;

        private void Awake()
        {
            instance = this;
            pathfinding = new Pathfinding(100, 100);
        }
        

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero,Mathf.Infinity);
                if (hit )
                {
                    Buildings buildings=hit.transform.GetComponentInParent<Buildings>();
                    if (buildings)
                    {
                        buildings.SelectPlacedObject();
                        controller = null;
                    }
                    PathfindingMovement pathfindingMovement=hit.transform.GetComponent<PathfindingMovement>();
                    if (pathfindingMovement)
                    {
                        controller = pathfindingMovement;
                    }
                }
            }
            
            if (controller.soldier.dead)
            {
                return;
            }
            
            if (controller && Input.GetMouseButtonDown(1))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero,Mathf.Infinity);
                if (hit )
                {
                    EnemyTower enemyTower=hit.transform.GetComponentInParent<EnemyTower>();
                    if (enemyTower)
                    {
                        Node getSoldierNode = controller.GetNode();
                        Node nearestNode = null;
                        Vector2 soldierNodeXY = new Vector2(getSoldierNode.x, getSoldierNode.y);
                        foreach (Node node in enemyTower.neighbourNodeList)
                        {
                            if (node.IsItEmpty())
                            {
                                if (nearestNode==null)
                                {
                                    nearestNode = node;
                                    continue;
                                } 
                                if (Vector2.Distance(new Vector2(node.x,node.y),soldierNodeXY)<Vector2.Distance(new Vector2(nearestNode.x,nearestNode.y),soldierNodeXY))
                                {
                                    nearestNode = node;
                                }
                            }
                        }
                        if (nearestNode==null)
                        {
                            return;
                        }
                        controller.SetTargetPosition(nearestNode.grid.PositionInTheMiddleOfGrid(nearestNode.x,nearestNode.y));
                        nearestNode.SetSoldier(controller.soldier);
                        currentAttackNode = nearestNode;
                        controller.soldier.attack = true;
                        controller.target = enemyTower.transform;
                    }
                }
            }
            
            
            if (Utils.IsPointerOverUIObject())
            {
                return;
            }
            if (Input.GetMouseButtonDown(1)&& controller) 
            {
                
                if (controller.soldier.attack)
                {
                    currentAttackNode.ClearSoldier();
                    controller.soldier.attack = false;
                }
                pathfinding.grid.GetXY(Utils.GetMouseWorldPosition(), out int x, out int y);
                Node node = pathfinding.GetNode(x, y);
                if (node.IsItEmpty())
                {
                    controller.SetTargetPosition(pathfinding.grid.PositionInTheMiddleOfGrid(x,y));
                }
            }
        }

       
    }
}