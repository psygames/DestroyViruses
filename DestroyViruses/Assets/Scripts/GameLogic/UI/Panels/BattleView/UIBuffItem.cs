using UnityEngine;
using System.Collections;
using UnityEngine.UI;


namespace DestroyViruses
{
    public class UIBuffItem : MonoBehaviour
    {
        public Image bg;
        public Image forward;
        public Text nameText;

        public void SetData(BuffData buff)
        {
            bg.SetSprite(buff.uiIcon);
            forward.SetSprite($"ui_buff_fill_{buff.table.type}");
            forward.fillAmount = 1f - buff.progress;
            nameText.text = buff.name;
        }
    }
}