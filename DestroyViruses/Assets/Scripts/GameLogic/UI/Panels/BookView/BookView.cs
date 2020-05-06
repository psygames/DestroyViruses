using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class BookView : ViewBase
    {
        public FadeGroup fadeGroup;
        public ContentGroup itemGroup;
        private List<VirusData> mVirus = new List<VirusData>();

        private void OnClickClose()
        {
            NavigationView.BlackSetting(false);
            fadeGroup.FadeOut(Close);
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            this.BindUntilDisable<EventGameData>(OnEventGameData);
            mVirus.Clear();
            foreach (var v in TableVirus.GetAll())
            {
                if (v.collectable <= 0)
                    continue;
                var _data = new VirusData();
                _data.SetData(v.id);
                mVirus.Add(_data);
            }
            mVirus.Sort((a, b) => b.isUnlock.CompareTo(a.isUnlock));

            Refresh();

            fadeGroup.FadeIn(() =>
            {
                NavigationView.BlackSetting(true);
            });
        }

        private void OnEventGameData(EventGameData evt)
        {
            if (evt.action == EventGameData.Action.DataChange)
            {
                Refresh();
            }
        }

        private void Refresh()
        {
            itemGroup.SetData<BookVirusItem, VirusData>(mVirus);
        }
    }
}