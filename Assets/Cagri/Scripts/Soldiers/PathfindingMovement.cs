using System.Collections.Generic;
using Cagri.Scripts.GenericSystems.Pathfinding;
using CharacterController;
using UnityEngine;

namespace Cagri.Scripts.Soldiers
{
    public class PathfindingMovement : MonoBehaviour
    {
            private int currentPathIndex;
            private List<Vector3> pathVectorList;
            private Node nodeIsLocated;

            [HideInInspector]public Soldier soldier;
            public Transform target;
            private void Awake()
            {
                soldier = GetComponentInChildren<Soldier>();
            }

            private void Start()
            {
                CanBuildControl();
            }


            private void Update() 
            {
                HandleMovement();
            }
            
            private void HandleMovement()
            {
                if (soldier.dead)
                {
                    return;
                }
                if (pathVectorList != null) 
                {
                    if (nodeIsLocated!=null)
                    {
                        soldier.animator.SetTrigger("Charge");
                        nodeIsLocated.canBuild = true;
                        nodeIsLocated = null;
                    }
                    
                    Vector3 targetPosition = pathVectorList[currentPathIndex];
                    if (Vector3.Distance(transform.position, targetPosition) > 1f)
                    {
                        var position = transform.position;
                        Vector3 moveDir = (targetPosition - position).normalized;
                        position += moveDir * (soldier.speed * Time.deltaTime);
                        transform.position = position;
                    } else 
                    {
                        currentPathIndex++;
                        if (currentPathIndex >= pathVectorList.Count)
                        {
                            StopMoving();
                        }
                    }
                } 
            }

            private void CanBuildControl()
            {
                nodeIsLocated = GetNode();
                nodeIsLocated.canBuild = false;
            }

            private void StopMoving() 
            {
                CanBuildControl();
                pathVectorList = null;
                if (soldier.attack)
                {
                    soldier.Attack(target);
                }
                else
                {
                    soldier.animator.SetTrigger("Idle");
                }
            }
        
            private Vector3 GetPosition() {
                return transform.position;
            }
        
            private Node GetNode()
            {
                Pathfinding.Instance.grid.GetXY(GetPosition(), out int x, out int y);
                return Pathfinding.Instance.GetNode(x,y);;
            }
            
        
        
            public void SetTargetPosition(Vector3 targetPosition)
            {
                currentPathIndex = 0;
                pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);
                pathVectorList[^1] = targetPosition;
        
                if (pathVectorList != null && pathVectorList.Count > 1) {
                    pathVectorList.RemoveAt(0);
                }
            }
    }
}