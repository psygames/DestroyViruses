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
            if (mText == null)
                mText = GetComponent<Text>();
            var prop = LT.table.GetType().GetProperty(key
                , System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (prop == null)
            {
                mText.text = key;
            }
            else
            {
                mText.text = prop.GetValue(LT.table) as string;
            }
        }
    }
}