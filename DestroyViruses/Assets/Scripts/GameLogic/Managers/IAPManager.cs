using UnityEngine;
using UnityEngine.Purchasing;
namespace DestroyViruses
{

    public class IAPManager : IStoreListener
    {
        private IStoreController controller;
        private IExtensionProvider extensions;

        public void Init()
        {
            var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
            foreach(var t in TableShop.GetAll())
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
        }

        /// <summary>
        /// Called when a purchase completes.
        ///
        /// May be called at any time after OnInitialized().
        /// </summary>
        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
        {
            return PurchaseProcessingResult.Complete;
        }

        /// <summary>
        /// Called when a purchase fails.
        /// </summary>
        public void OnPurchaseFailed(Product i, PurchaseFailureReason p)
        {
        }
    }
}