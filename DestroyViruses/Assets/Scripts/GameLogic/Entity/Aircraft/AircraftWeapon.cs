using UnityEngine;
using System.Collections;

namespace DestroyViruses
{
    public class AircraftWeapon : MonoBehaviour
    {
        private Transform mWeaponRoot;
        private WeaponBase mWeapon;
        private string mLastWeaponType;

        private void Awake()
        {
            if (mWeaponRoot == null)
                mWeaponRoot = transform.Find("weaponRoot");
            if (mWeaponRoot == null)
                Debug.LogError("WeaponRoot is null, please take care of this!");
        }

        public void Reset()
        {
            var weaponType = D.I.weaponId <= 0 ? "WeaponNone" : TableWeapon.Get(D.I.weaponId).type;

            if (mLastWeaponType != weaponType)
            {
                if (mWeapon != null)
                {
                    DestroyImmediate(mWeapon.gameObject);
                }
                var prefab = ResourceUtil.Load<WeaponBase>(PathUtil.Entity(weaponType));
                if (prefab != null)
                {
                    mWeapon = Instantiate(prefab);
                    mWeapon.rectTransform.SetParent(mWeaponRoot, false);
                }
            }

            mWeapon.Reset(D.I.weaponId, D.I.weaponPowerLevel, D.I.weaponSpeedLevel);
        }
    }
}