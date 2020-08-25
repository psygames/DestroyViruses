using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnibusEvent;
using System.IO;
using System;

namespace DestroyViruses
{
    public class VipView : ViewBase
    {
        public ContentGroup rewardGroup;
        public RadioObjects radioBtn;
        public GameObject expirationObj;
        public Text expirationTime;

        protected override void OnOpen()
        {
            base.OnOpen();
            this.BindUntilDisable<EventGameData>(OnEventGameData);
            RefreshUI();
        }

        private void OnEventGameData(EventGameData evt)
        {
            if (evt.action == EventGameData.Action.DataChange)
            {
                RefreshUI();
            }
        }

        private void RefreshUI()
        {   
            var rewards = new List<Tuple<string, string>> {
                new Tuple<string, string>("icon_vip_fund",LT.Get("vipRewardFund")),
                new Tuple<string, string>("icon_vip_diamond",LT.Get("vipRewardDiamond")),
                new Tuple<string, string>("icon_vip_ad",LT.Get("vipRewardAd")),
                new Tuple<string, string>("icon_vip_difficulty",LT.Get("vipRewardDifficulty")),
                new Tuple<string, string>("icon_vip_revival",LT.Get("vipRewardRevival")),
            };
            rewardGroup.SetData<VipRewardItem, Tuple<string, string>>(rewards);
            radioBtn.Radio(!D.I.IsVip() ? 0 : D.I.HasVipReward() ? 1 : 2);
            expirationObj.SetActive(D.I.IsVip() && false);
            if (D.I.IsVip())
            {
                expirationTime.text = D.I.VipExpirationDate().ToString("yyyy-MM-dd HH:mm:ss");
            }
        }

        private void OnClickVip()
        {
            if (D.I.IsVip())
            {
                Toast.Show("Already VIP");
                return;
            }
            D.I.Purchase(6);
        }

        private void OnClickReward()
        {
            if (!D.I.HasVipReward())
            {
                Toast.Show("Already Received");
            }
            D.I.ReceiveVipReward();
        }

        private void OnClickRestore()
        {
            // IAPManager.Instance.Restore();
        }

        bool lastIsVip = false;
        public void Update()
        {
            if (lastIsVip && !D.I.IsVip())
            {
                RefreshUI();
            }
            lastIsVip = D.I.IsVip();
        }
    }
}