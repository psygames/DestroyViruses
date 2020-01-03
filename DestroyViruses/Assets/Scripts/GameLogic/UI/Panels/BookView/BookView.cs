using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using DG.Tweening;
using UnibusEvent;

namespace DestroyViruses
{
    public class BookView : ViewBase
    {
        public ContentGroup itemGroup;
        private List<VirusData> mVirus = new List<VirusData>();

        private void OnClickClose()
        {
            Close();
        }

        protected override void OnOpen()
        {
            base.OnOpen();
            this.BindUntilDisable<EventGameData>(OnEventGameData);
            mVirus.Clear();
            foreach (var v in TableVirus.GetAll())
            {
                var _data = new VirusData();
                _data.SetData(v.id);
                mVirus.Add(_data);
            }
            mVirus.Sort((a, b) => b.isUnlock.CompareTo(a.isUnlock));

            Refresh();
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