// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      Mono Runtime Version: 2.0.50727.1433
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace QFramework.Example
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.UI;
    
    
    public partial class UIGamePanel
    {
        
        public string NAME = "UIGamePanel";
        
        [SerializeField()]
        public Text gameText;
        
        [SerializeField()]
        public Button backBtn;
        
        private UIGamePanelData mPrivateData = null;
        
        public UIGamePanelData mData
        {
            get
            {
                return mPrivateData ?? (mPrivateData = new UIGamePanelData());
            }
            set
            {
                mUIData = value;
                mPrivateData = value;
            }
        }
        
        protected override void ClearUIComponents()
        {
            gameText = null;
            backBtn = null;
            mData = null;
        }
    }
}
