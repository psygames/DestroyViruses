using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class WeaponItem : ViewBase
    {
        public Text title;
        public Image icon;
        public RadioGroup radioState;
        public GameObject selectedObj;
        public Text unlockText;
        public GameObject trialTag;

        private int mId = -1;

        public void SetData(int id)
        {
            mId = id;
            var table = TableWeapon.Get(id);
            icon.SetSprite(table.icon);
            icon.SetGrey(D.I.unlockedGameLevel < table.unlockLevel);
            title.text = LT.Get(table.nameID);
            selectedObj.SetActive(id == D.I.weaponId);
            radioState.Radio(D.I.unlockedGameLevel < table.unlockLevel ? 0 : D.I.weaponId == id ? 2 : 1);
            unlockText.text = LTKey.WEAPON_UNLOCK_ON_GAME_LEVEL_X.LT(table.unlockLevel - 1);
            trialTag.SetActive(D.I.GetTrialWeaponID() == id && !D.I.IsInTrial());
        }

        private void OnClickSelf()
        {
            if (D.I.GetTrialWeaponID() == mId && !D.I.IsInTrial())
            {
                AdProxy.Ins.ShowAd(() =>
                {
                    D.I.TrialBegin();
                    Analytics.Event.Advertising("weapon_trial");
                }, () =>
                {
                    Toast.Show(LTKey.AD_PLAY_FAILED.LT());
                });
            }

            if (mId == D.I.weaponId)
            {
                //D.I.ChangeWeapon(0);
            }
            else
            {
                D.I.ChangeWeapon(mId);
            }
        }
    }
}