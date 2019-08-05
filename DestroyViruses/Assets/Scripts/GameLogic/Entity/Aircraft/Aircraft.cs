using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DestroyViruses
{
    public class Aircraft : EntityBase<Aircraft>
    {
        private AircraftInputHandle mInputHandle;
        private AircraftMovement mMovement;
        private AircraftFire mFire;

        private void Awake()
        {
            mInputHandle = gameObject.GetOrAddComponent<AircraftInputHandle>();
            mMovement = gameObject.GetOrAddComponent<AircraftMovement>();
            mFire = gameObject.GetOrAddComponent<AircraftFire>();
            mInputHandle.onFire = mFire.Fire;
            mInputHandle.onHoldFire = mFire.HoldFire;
            mInputHandle.onMove = mMovement.Move;
        }

        private void Start()
        {
            rectTransform.anchoredPosition3D = UIUtil.center;
            Debug.LogError(ConfigAircraft.Get("1").hp);
        }

        private void Update()
        {

        }

    }
}