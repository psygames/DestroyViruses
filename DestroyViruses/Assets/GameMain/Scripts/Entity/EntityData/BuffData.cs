using System;
using UnityEngine;

namespace DestroyViruses
{
    [Serializable]
    public class BuffData : EntityData
    {
        [SerializeField]
        private int m_OwnerId = 0;

        [SerializeField]
        private int m_Attack = 0;

        [SerializeField]
        private float m_Speed = 0f;

        public BuffData(int entityId, int typeId, int ownerId, int attack, float speed)
            : base(entityId, typeId)
        {
            m_OwnerId = ownerId;
            m_Attack = attack;
            m_Speed = speed;
        }

        public int OwnerId
        {
            get
            {
                return m_OwnerId;
            }
        }

        public int Attack
        {
            get
            {
                return m_Attack;
            }
        }

        public float Speed
        {
            get
            {
                return m_Speed;
            }
        }
    }
}
