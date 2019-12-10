using UnityEngine;
using UnityEngine.UI;

namespace DestroyViruses
{
    [RequireComponent(typeof(Text))]
    public class LocalizationText : MonoBehaviour
    {
        public string key = "";

        private Text mText;

        private void OnEnable()
        {
            Validate();
        }

        public void Validate()
        {
            if (mText == null)
                mText = GetComponent<Text>();
            mText.text = LT.Get(key);
        }
    }
}