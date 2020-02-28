using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class GameLevelPanel : MonoBehaviour
    {
        public GameLevelPanelItem[] items;

        public void PlayLevelUp()
        {
            for (int i = 0; i < items.Length; i++)
            {
                var level = D.I.gameLevel + i - 2;
                items[i].FadeToLevelPos(level);
            }
        }

        public void SetData()
        {
            for (int i = 0; i < items.Length; i++)
            {
                var level = D.I.gameLevel + i - 1;
                items[i].SetLevel(level);
            }
        }
    }
}