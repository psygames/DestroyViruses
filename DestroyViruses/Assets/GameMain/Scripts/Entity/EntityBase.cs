using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Entity;

namespace DestroyViruses
{
    public class EntityBase : MonoBehaviour, IEntity
    {
        public virtual float hp { get; protected set; }
        public virtual bool isAlive { get { return hp > 0; } }

        public int Id { get; }

        public string EntityAssetName { get; }

        public object Handle { get; }

        public IEntityGroup EntityGroup { get; }

        public void OnInit(int entityId, string entityAssetName, IEntityGroup entityGroup, bool isNewInstance, object userData)
        {
        }

        public void OnRecycle()
        {
        }

        public void OnShow(object userData)
        {
        }

        public void OnHide(object userData)
        {
        }

        public void OnAttached(IEntity childEntity, object userData)
        {
        }

        public void OnDetached(IEntity childEntity, object userData)
        {
        }

        public void OnAttachTo(IEntity parentEntity, object userData)
        {
        }

        public void OnDetachFrom(IEntity parentEntity, object userData)
        {
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }
    }
}