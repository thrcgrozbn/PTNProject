using Cagri.Scripts.GridBuildingSystem.Buildings.BuildingTypesSO;
using UnityEngine;

namespace Cagri.Scripts.GridBuildingSystem.Buildings
{
    public class BuildingGhost2D : MonoBehaviour {

        private Transform visual;
        private BuildingsTypeSO buildingsTypeSo;
        
        public static SpriteRenderer spriteRenderer;

        private void Start() {
            RefreshVisual();

           GridBuildingSystem.Instance.OnSelectedChanged += Instance_OnSelectedChanged;
        }

        private void Instance_OnSelectedChanged(object sender, System.EventArgs e) {
            RefreshVisual();
        }

        private void LateUpdate() {
            Vector3 targetPosition = global::Cagri.Scripts.GridBuildingSystem.GridBuildingSystem.Instance.GetMouseWorldSnappedPosition();
            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 15f);

            transform.rotation = Quaternion.Lerp(transform.rotation, global::Cagri.Scripts.GridBuildingSystem.GridBuildingSystem.Instance.GetPlacedObjectRotation(), Time.deltaTime * 15f);
        }

        private void RefreshVisual() {
            if (visual != null) {
                Destroy(visual.gameObject);
                visual = null;
                spriteRenderer = null;
            }

            BuildingsTypeSO buildingsTypeSo = global::Cagri.Scripts.GridBuildingSystem.GridBuildingSystem.Instance.GetPlacedObjectTypeSO();

            if (buildingsTypeSo != null) {
                visual = Instantiate(buildingsTypeSo.visual, Vector3.zero, Quaternion.identity);
                spriteRenderer = visual.GetComponentInChildren<SpriteRenderer>();
                visual.parent = transform;
                visual.localPosition = Vector3.zero;
                visual.localEulerAngles = Vector3.zero;
            }
        }

    }
}
