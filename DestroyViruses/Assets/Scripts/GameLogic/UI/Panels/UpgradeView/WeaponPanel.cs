using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class WeaponPanel : ViewBase
    {
        public ContentGroup weaponGroup;

        public void SetData()
        {
            weaponGroup.SetData<WeaponItem, TableWeapon>(TableWeapon.GetAll(),
                (index, item, data) =>
                {
                    item.SetData(data.id);
                });
        }
    }
}