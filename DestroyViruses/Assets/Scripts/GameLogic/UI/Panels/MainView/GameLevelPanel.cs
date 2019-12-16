using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace DestroyViruses
{
    public class GameLevelPanel : MonoBehaviour
    {
        // game level
        public DOTweenTrigger gameLevelTweenTrigger;
        public Text previousLevelText;
        public Text currentLevelText;
        public Text nextLevelText;
        public Button previousLevelBtn;
        public Button currentLevelBtn;
        public Button nextLevelBtn;
        public GameObject previousBossTag;
        public GameObject currentBossTag;
        public GameObject nextBossTag;


        private void Awake()
        {
            previousLevelBtn?.OnClick(() =>
            {
                D.I.SelectGameLevel(D.I.gameLevel - 1);
                gameLevelTweenTrigger.DoTrigger();
            });

            currentLevelBtn?.OnClick(() =>
            {
                gameLevelTweenTrigger.DoTrigger();
            });

            nextLevelBtn?.OnClick(() =>
            {
                D.I.SelectGameLevel(D.I.gameLevel + 1);
                gameLevelTweenTrigger.DoTrigger();
            });
        }

        public void SetData()
        {
            if (D.I.gameLevel - 1 <= 0)
            {
                previousLevelText.text = "-";
                previousBossTag.SetActive(false);
            }
            else
            {
                previousLevelText.text = (D.I.gameLevel - 1).ToString();
                previousBossTag.SetActive(TableGameLevel.Get(D.I.gameLevel - 1).isBoss);
            }

            currentLevelText.text = D.I.gameLevel.ToString();
            currentBossTag.SetActive(TableGameLevel.Get(D.I.gameLevel).isBoss);

            if (TableGameLevel.Get(D.I.gameLevel + 1) == null)
            {
                nextLevelText.text = "-";
                // nextLevelText.color = UIUtil.GRAY_COLOR;
                nextBossTag.SetActive(false);
            }
            else
            {
                nextLevelText.text = (D.I.gameLevel + 1).ToString();
                nextBossTag.SetActive(TableGameLevel.Get(D.I.gameLevel + 1).isBoss);
                if (D.I.gameLevel + 1 > D.I.unlockedGameLevel)
                {
                    // nextLevelText.color = UIUtil.GRAY_COLOR;
                }
                else
                {
                    // nextLevelText.color = currentLevelText.color;
                }
            }
        }
    }
}