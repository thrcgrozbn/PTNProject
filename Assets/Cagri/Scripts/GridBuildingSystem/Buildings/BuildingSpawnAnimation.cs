using UnityEngine;

namespace Cagri.Scripts.GridBuildingSystem.Buildings
{
    public class BuildingSpawnAnimation : MonoBehaviour {

        [SerializeField] private AnimationCurve animationCurve = null;

        private float time;

        private void Update() {
            time += Time.deltaTime;

            transform.localScale = new Vector3(1, animationCurve.Evaluate(time), 1);
        }

    }
}
