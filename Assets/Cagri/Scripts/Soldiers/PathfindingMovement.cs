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

            [HideInInspector]public Soldier soldier;
            public Transform target;
            
            
            private void Awake()
            {
                soldier = GetComponentInChildren<Soldier>();
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
                    Vector3 targetPosition = pathVectorList[currentPathIndex];
                    if (Vector3.Distance(transform.position, targetPosition) > .25f)
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
                            transform.position = targetPosition;
                            StopMoving();
                        }
                    }
                } 
            }
            
            private void StopMoving() 
            {
                GetNode().SetSoldier(soldier);
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
        
            public Node GetNode()
            {
                Pathfinding.Instance.grid.GetXY(GetPosition(), out int x, out int y);
                return Pathfinding.Instance.GetNode(x,y);;
            }
            
            
            public void SetTargetPosition(Vector3 targetPosition)
            {
                soldier.animator.SetTrigger("Charge");
                GetNode().ClearSoldier();
                currentPathIndex = 0;
                pathVectorList = Pathfinding.Instance.FindPath(GetPosition(), targetPosition);
                pathVectorList[^1] = targetPosition;
        
                if (pathVectorList != null && pathVectorList.Count > 1) {
                    pathVectorList.RemoveAt(0);
                }
            }
    }
}