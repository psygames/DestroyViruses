using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class VirusData
    {
        public int id { get; private set; }
        public TableVirus table { get; private set; }
        public int collectCount { get { return DataProxy.Ins.BookGetCollectCount(id); } }
        public int needCount { get { return ConstTable.table.bookVirusCollectKillCount; } }
        public bool isUnlock { get { return DataProxy.Ins.BookIsUnlock(id); } }
        public bool isReceivable { get { return collectCount >= needCount; } }
        public string name { get { return LT.Get(table.nameID); } }
        public string description { get { return LT.Get(table.descriptionID); } }
        public string tips { get { return LT.Get(table.tipsID); } }
        public string prefabPath { get { return PathUtil.Entity(table.type); } }

        public void SetData(int id)
        {
            this.id = id;
            this.table = TableVirus.Get(id);
        }
    }

    public class BookVirusView : ViewBase
    {
        public Text title;
        public Text description;
        public Text tips;
        public Text diamondCount;
        public ButtonPro receiveBtn;
        public Transform modelRoot;
        public Slider fill;

        public static int VirusID { get; set; }
        public static int ColorIndex { get; set; }
        private VirusData v = new VirusData();
        private VirusBase mVirus;
        private string mLastPrefabPath;

        protected override void OnOpen()
        {
            base.OnOpen();
            this.BindUntilDisable<EventGameData>(OnEventGameData);
            v.SetData(VirusID);
            Refresh();
        }

        private void Refresh()
        {
            if (v.prefabPath != mLastPrefabPath)
            {
                if (mVirus != null)
                    DestroyImmediate(mVirus.gameObject);

                var prefab = ResourceUtil.Load<VirusBase>(v.prefabPath);
                if (mVirus == null && prefab != null)
                {
                    mVirus = Instantiate(prefab);
                    if (mVirus != null)
                    {
                        mVirus.rectTransform.SetParent(modelRoot, false);
                        mVirus.SetColor(ColorIndex);
                    }
                }

                mLastPrefabPath = v.prefabPath;
            }

            fill.value = 1f * v.collectCount / v.needCount;
            receiveBtn.SetBtnGrey(!v.isReceivable);
            diamondCount.text = "x" + ConstTable.table.bookVirusCollectRewardDiamond;
            title.text = v.name;
            tips.text = v.tips;
            description.text = v.description;
        }

        private void OnEventGameData(EventGameData evt)
        {
            if (evt.action == EventGameData.Action.DataChange)
            {
                Refresh();
            }
        }

        private void OnClickReceive()
        {
            if (v.isReceivable)
            {
                D.I.BookCollect(VirusID);
                AudioManager.Instance.PlaySound("collect_coin");
            }
            else
            {
                Toast.Show("收集数量不足");
            }
        }
    }
}