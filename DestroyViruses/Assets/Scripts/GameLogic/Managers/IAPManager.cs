using UnityEngine;
using UnityEngine.Purchasing;
namespace DestroyViruses
{

    public class IAPManager : Singleton<IAPManager>, IStoreListener
    {
        private IStoreController controller;
        private IExtensionProvider extensions;

        public bool isInit { get; private set; }

        public void Init()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach (var t in TableShop.GetAll())
            {
                builder.AddProduct(t.productID, (ProductType)t.type);
            }
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
            Debug.LogError("IAP Initialize Failed: " + error.ToString());
        }

        /// <summary>
        /// Called when a purchase completes.
        ///
        /// May be called at any time after OnInitialized().
        /// </summary>
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            var pid = e.purchasedProduct.definition.id;
            var goods = TableShop.Get(a => a.productID == pid);
            D.I.OnPurchaseSuccess(goods.id);
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
    }
}