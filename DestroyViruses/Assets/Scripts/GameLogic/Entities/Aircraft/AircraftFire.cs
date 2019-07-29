using UnityEngine;
using System.Collections;

namespace DestroyViruses
{
    public partial class AircraftFire : MonoBehaviour
    {
        public float fireOnceCD = 0.5f;
        public int fireOnceBullets = 5;
        public float bulletHSpace = 5;
        public float bulletVSpace = 5;

        public bool isFiring { get; private set; }

        private float mFireOnceCD = 0;

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
            isFiring = true;
        }

        public void HoldFire()
        {
            isFiring = false;
        }

        private void Update()
        {
            mFireOnceCD = this.UpdateCD(mFireOnceCD);
            if (isFiring)
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