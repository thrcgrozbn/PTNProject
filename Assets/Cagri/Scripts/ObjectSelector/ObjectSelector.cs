using Cagri.Scripts.GenericSystems.Pathfinding;
using Cagri.Scripts.GenericSystems.Utils;
using Cagri.Scripts.GridBuildingSystem.Buildings;
using Cagri.Scripts.Soldiers;
using UnityEngine;

namespace Cagri.Scripts.ObjectSelector
{
    public class ObjectSelector : MonoBehaviour
    {
        public static ObjectSelector instance;
        private Pathfinding pathfinding;
        public PathfindingMovement controller;

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
                    PathfindingMovement cc=hit.transform.GetComponent<PathfindingMovement>();
                    if (cc)
                    {
                        controller = cc;
                    }
                }
            }

            if (controller && Input.GetMouseButtonDown(1))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero,Mathf.Infinity);
                if (hit )
                {
                    Buildings po=hit.transform.GetComponentInParent<Buildings>();
                    if (po)
                    {
                        Vector3 pos=Vector3.zero;
                        foreach (Transform spawnTransform in po.soldierHolderAroundBuildingList)
                        {
                            if (spawnTransform.childCount<=0)
                            {
                                controller.transform.SetParent(spawnTransform);
                                pos = spawnTransform.position;
                            }
                        }

                        if (pos!=Vector3.zero)
                        {
                            controller.SetTargetPosition(pos);
                            controller.soldier.attack = true;
                            controller.target = po.transform;
                        }
                    }
                }
            }
            
            if (Utils.IsPointerOverUIObject())
            {
                return;
            }
            if (Input.GetMouseButtonDown(1)&& controller) 
            {
                if (controller.transform.parent)
                {
                    controller.transform.SetParent(null);
                    if (controller.soldier.attack)
                    {
                        controller.soldier.attack = false;
                    }
                }
                controller.SetTargetPosition(Utils.GetMouseWorldPosition());
            }
        }
    }
}