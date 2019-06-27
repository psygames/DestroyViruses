using UnityEngine;
using System.Collections;
using GameFramework.Entity;
namespace DestroyViruses
{
    public class MainPlayer : IEntity
    {
        public int Id { get; protected set; }

        public string EntityAssetName { get; protected set; }

        public object Handle { get; protected set; }

        public IEntityGroup EntityGroup { get; protected set; }

        public void OnAttached(IEntity childEntity, object userData)
        {
        }

        public void OnAttachTo(IEntity parentEntity, object userData)
        {
        }

        public void OnDetached(IEntity childEntity, object userData)
        {
        }

        public void OnDetachFrom(IEntity parentEntity, object userData)
        {
        }

        public void OnHide(object userData)
        {
        }

        public void OnInit(int entityId, string entityAssetName, IEntityGroup entityGroup, bool isNewInstance, object userData)
        {
        }

        public void OnRecycle()
        {
        }

        public void OnShow(object userData)
        {
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }
    }
}