using Cagri.Scripts.GenericSystems.Utils;
using Cagri.Scripts.Resources;
using UnityEngine;

namespace Cagri.Scripts.GridBuildingSystem.Buildings.Type
{
    public class PowerPlant :Buildings
    {
        public float productionTime;
        private float _productionTime;
        public int energyValue;

        private void EnergyProduction()
        {
            _productionTime += Time.deltaTime;
            if (_productionTime>=productionTime)
            {
                _productionTime = 0;
                ResourcesControl.instance.EnergyUpdate(energyValue);
                Utils.CreateWorldTextPopup("+"+energyValue, transform.position);
            }
        }

        private void Update()
        {
            EnergyProduction();
        }
    }
}