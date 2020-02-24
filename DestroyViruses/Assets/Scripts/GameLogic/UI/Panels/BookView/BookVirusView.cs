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
        public int collectCount { get { return D.I.BookGetCollectCount(id); } }
        public int startCount { get { return D.I.GetBookCountBegin(id); } }
        public int endCount { get { return D.I.GetBookCountEnd(id); } }
        public bool isMax { get { return D.I.IsBookCollectMax(id); } }
        public bool needPlayAd { get { return D.I.IsBookCollectNeedPlayAd(id); } }
        public float progress { get { return 1f * (collectCount - startCount) / (endCount - startCount); } }
        public bool isUnlock { get { return D.I.BookIsUnlock(id); } }
        public bool isReceivable { get { return !isMax && collectCount >= endCount; } }
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
        public Text fillText;
        public RadioObjects adReceiveRadio;

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

            if (mVirus != null)
            {
                mVirus.stunEffect.Stop(true);
            }
            if (v.isMax)
            {
                fill.value = 1f;
                fillText.text = "";
                receiveBtn.SetData(LTKey.COLLECT_FINISH.LT(), true, false);
            }
            else
            {
                fill.value = v.progress;
                fillText.text = $"{v.collectCount - v.startCount}/{v.endCount - v.startCount}";
                receiveBtn.SetData(LTKey.RECEIVE.LT(), !v.isReceivable, false);
            }
            diamondCount.text = "x" + D.I.GetBookDiamond(v.id);
            title.text = v.name;
            tips.text = v.tips;
            description.text = v.description;
            adReceiveRadio.Radio(v.needPlayAd);
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
            if (v.isMax)
            {
                Toast.Show(LTKey.COLLECT_FINISH.LT());
            }
            else if (v.isReceivable)
            {
                if (v.needPlayAd)
                {
                    AdProxy.Ins.ShowAd(() =>
                    {
                        D.I.BookCollect(v.id);
                        AudioManager.PlaySound("collect_coin");
                        Analytics.Event.Advertising("book_reward");
                    }, () =>
                    {
                        Toast.Show(LTKey.AD_PLAY_FAILED.LT());
                    });
                }
                else
                {
                    D.I.BookCollect(v.id);
                    AudioManager.PlaySound("collect_coin");
                }
            }
            else
            {
                Toast.Show(LTKey.VIRUS_COLLECT_NOT_ENOUGH.LT());
            }
        }
    }
}