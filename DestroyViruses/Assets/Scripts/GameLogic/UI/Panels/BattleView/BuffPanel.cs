using UnityEngine;
using System.Collections;
namespace DestroyViruses
{
    public class BuffPanel : MonoBehaviour
    {
        public ContentGroup group;

        private void Update()
        {
            var proxy = ProxyManager.GetProxy<BuffProxy>();
            group.SetData<UIBuffItem, BuffData>(proxy.buffs.Values, (index, item, data) =>
            {
                item.SetData(data);
            });
        }
    }
}