using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.Purchasing.Security;

namespace DestroyViruses
{

    public class IAPManager : Singleton<IAPManager>, IStoreListener
    {
        private IStoreController controller;
        private IExtensionProvider extensions;

        public bool isInit { get; private set; }
        public bool isInitFailed { get; private set; }

        public void Init()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (var t in TableShop.GetAll())
            {
                builder.AddProduct(t.productID, (ProductType)t.type, new IDs
                {
                    {t.productID, GooglePlay.Name},
                    {t.productID, AppleAppStore.Name }
                });
            }
            builder.Configure<IGooglePlayConfiguration>().SetPublicKey("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAt1lvHFO1vq2djnUEgv3PGrTsieVKtLUIRSlW99wnqHZMYLRoXnK0qOnWU6Lz/jt07LkktqJzWrW+OE/w67VtLJMrycBaA0eCDqErr62pg4HkXK4tS9+0oJJxJ9pvr+QACLigJlHefPGtoZ+JumUILnxM6dPwW7xalxaCTmhSpfg3CVZ75NXTmIx4X5qLAHFPOCPrjdTTZPdX7zZMRTdIYAAseXn4X+BE2gRhhyCVA0T9+m1AD8lC+wftuQz+xRksYjEj9XgNJJ3A0Z9QKr3MZoyqYQ0+6HPpNzKY6owf7t3fix36oJl4OYu+99ksO2p2oqVrpbPSJVWFkmbJRrFepQIDAQAB");
            builder.useCatalogProvider = false;
            UnityPurchasing.Initialize(this, builder);
        }

        /// <summary>
        /// Called when Unity IAP is ready to make purchases.
        /// </summary>
        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            isInit = true;
            this.controller = controller;
            this.extensions = extensions;
        }

        /// <summary>
        /// Called when Unity IAP encounters an unrecoverable initialization error.
        ///
        /// Note that this will not be called if Internet is unavailable; Unity IAP
        /// will attempt initialization until it becomes available.
        /// </summary>
        public void OnInitializeFailed(InitializationFailureReason error)
        {
            isInitFailed = true;
            Debug.LogError("IAP Initialize Failed: " + error.ToString());
        }

        #region Purchase
        // Example method called when the user presses a 'buy' button
        // to start the purchase process.
        public void Purchase(string productId)
        {
            if (!isInit || controller == null)
            {
                Toast.Show("Purchase Failed cause IAP not initialized");
                return;
            }
            controller.InitiatePurchase(productId);
        }

        /// <summary>
        /// Called when a purchase completes.
        ///
        /// May be called at any time after OnInitialized().
        /// </summary>
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            bool validPurchase = true; // Presume valid for platforms with no R.V.
            System.DateTime purchaseData = System.DateTime.Now;

            // Unity IAP's validation logic is only included on these platforms.
#if !UNITY_EDITOR && (UNITY_ANDROID || UNITY_IOS)
            // Prepare the validator with the secrets we prepared in the Editor
            // obfuscation window.
            purchaseData = System.DateTime.MinValue;
            var validator = new CrossPlatformValidator(GooglePlayTangle.Data(),
                AppleTangle.Data(), Application.identifier);

            try
            {
                // On Google Play, result has a single product ID.
                // On Apple stores, receipts contain multiple products.
                var result = validator.Validate(e.purchasedProduct.receipt);
                // For informational purposes, we list the receipt(s)
                //Debug.Log("Receipt is valid. Contents:");
                foreach (IPurchaseReceipt productReceipt in result)
                {
                    if (purchaseData.Ticks < productReceipt.purchaseDate.Ticks)
                    {
                        purchaseData = productReceipt.purchaseDate;
                    }
                }
            }
            catch (IAPSecurityException ex)
            {
                Debug.LogError("Invalid receipt:" + ex.Message);
                validPurchase = false;
            }
#endif

            if (validPurchase)
            {
                var pid = e.purchasedProduct.definition.id;
                var goods = TableShop.Get(a => a.productID == pid);
                D.I.OnPurchaseSuccess(goods.id, purchaseData);
            }
            else
            {
                Toast.Show("Purchase Failed, Invalid receipt.");
            }
            return PurchaseProcessingResult.Complete;
        }

        /// <summary>
        /// Called when a purchase fails.
        /// </summary>
        public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
        {
            Toast.Show(LTKey.PURCHASE_FAILED.LT());
            Debug.LogError("Purchase Failed: " + p.ToString() + ", productID: " + i.definition.id);
        }
        #endregion
    }
}