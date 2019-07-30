using UnityEngine;
using System.Collections;

namespace DestroyViruses
{
    public class AircraftFire : MonoBehaviour
    {
        public float fireSpeed = 20; // bullets/sec
        public float bulletHSpace = 5;
        public float bulletVSpace = 50;

        public bool IsFiring { get; private set; }

        private float mFireOnceCD;

        private void Awake()
        {
        }

        private void FireOnce()
        {
            for (int i = 0; i < fireOnceBullets; i++)
            {

            }

            Debug.LogError("fire once");
        }

        public void Fire()
        {
            IsFiring = true;
        }

        public void HoldFire()
        {
            IsFiring = false;
        }

        private void Update()
        {
            mFireOnceCD = this.UpdateCD(mFireOnceCD);
            if (IsFiring)
            {
                if (mFireOnceCD <= 0)
                {
                    FireOnce();
                    mFireOnceCD = fireOnceCD;
                }
            }
        }
    }
}