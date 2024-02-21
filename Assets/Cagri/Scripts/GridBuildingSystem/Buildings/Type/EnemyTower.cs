using System;
using Cagri.Scripts.GridBuildingSystem.Buildings.Interactable.Arrow;
using CharacterController;
using UnityEngine;

namespace Cagri.Scripts.GridBuildingSystem.Buildings.Type
{
    public class EnemyTower : Buildings
    {
        public float radius;
        
        public Transform target; // Target to follow (e.g., player)
        public GameObject arrowPrefab;
        
        
        public float fireRate = 1f; // Fire rate (bullets per second)
        private float fireTimer; // Timer to control firing rate


        void Attack()
        {
            if (target != null && Vector2.Distance(transform.position, target.position) < radius)
            {
                // Fire bullets at the target
                fireTimer += Time.deltaTime;
                if (fireTimer >= 1f / fireRate)
                {
                    FireArrow();
                    fireTimer = 0f;
                }
            }
        }
        void FireArrow()
        {
            // Instantiate a bullet prefab and fire it towards the target
            Buildings building =target.GetComponent<Buildings>();
            
            Vector3 startPos = transform.position + Vector3.right * .5f * buildingsTypeSo.width +
                               Vector3.up * .5f * buildingsTypeSo.height;
            Vector3 endPos = target.position+Vector3.up*.5f;
            if (building)
            {
               endPos = building.transform.position + Vector3.right * .5f * buildingsTypeSo.width +
                                 Vector3.up * .5f * buildingsTypeSo.height;
            }
            
            
            Vector2 direction = (endPos - startPos).normalized;
            
            GameObject bullet = Instantiate(arrowPrefab, startPos, transform.rotation);
            Rigidbody2D bulletRB = bullet.GetComponent<Rigidbody2D>();
            bullet.AddComponent<TowerArrow>();
            //bullet.transform.rotation = Quaternion.LookRotation(endPos);
            bulletRB.velocity = direction * 5;

            // Disable gravity for the bullet
            bulletRB.gravityScale = 0f;
        }
        void OnDrawGizmosSelected()
        {
            // Draw a wire sphere around the detection area
            
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
        private void AreaControl()
        {
            // Detect enemies using CircleCast in all directions
            RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, radius, Vector2.zero, radius);
            // Loop through each detected collision
            Soldier soldier = null;
            Buildings buildings = null;
            foreach (RaycastHit2D hit in hits)
            {
                // Check the tag of the collided object
                if (hit)
                {
                    
                    soldier =hit.collider.GetComponentInParent<Soldier>();
                    if (soldier)
                    {
                        target = soldier.transform;
                        break;
                    }
                    // Enemy detected, perform actions here
                }
            }
            if (!soldier)
            {
                foreach (RaycastHit2D hit in hits)
                {
                    // Check the tag of the collided object
                    if (hit)
                    {
                    
                        buildings =hit.collider.GetComponentInParent<Buildings>();
                        if (buildings)
                        {
                            EnemyTower et = buildings.GetComponent<EnemyTower>();
                            if (!et)
                            {
                                target = buildings.transform;
                                break;
                            }
                        }
                        // Enemy detected, perform actions here
                    }
                }
            }
        }

        private void Update()
        {
            AreaControl();
            Attack();
        }

        private void OnDestroy()
        {
            GridBuildingSystem.Instance.enemyTowerList.Remove(this);
        }
    }
}