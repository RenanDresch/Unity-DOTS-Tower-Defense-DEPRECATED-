using Game.Contracts.Enums;
using UnityEngine;

namespace Game.MB
{
    public class FXPicker_MB : MonoBehaviour
    {
        [SerializeField]
        private GameObject _turretMuzzleFxPrefab = default;
        [SerializeField]
        private GameObject _enemyExplosion_01Prefab = default;

        private static FXPicker_MB _instance;
        public static FXPicker_MB Instance
        {
            get
            {
                return _instance;
            }
            set
            {
                if (_instance)
                {
                    Destroy(_instance);
                }
                _instance = value;
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        public GameObject GetFXPrefab(FXCodeEnum fxCode)
        {
            switch (fxCode)
            {
                case FXCodeEnum.EnemyExplosion_01:
                    return _enemyExplosion_01Prefab;
                case FXCodeEnum.TurretMuzzle:
                    return _turretMuzzleFxPrefab;
                default:
                    return null;
            }
        }
    }
}
