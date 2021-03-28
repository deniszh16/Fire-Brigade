using UnityEngine;
using UnityEngine.Purchasing;

namespace Cubra
{
    public class InAppManager : MonoBehaviour
    {
        // Идентификаторы товаров
        private readonly string _stretcherX3 = "stretcher_3";
        private readonly string _stretcherX10 = "stretcher_10";

        /// <summary>
        /// Успешное завершение покупки
        /// </summary>
        /// <param name="product">товар</param>
        public void OnPurshaceComplete(Product product)
        {
            int quantity = 0;

            if (product.definition.id == _stretcherX3) quantity = 3;
            if (product.definition.id == _stretcherX10) quantity = 10;

            PlayerPrefs.SetInt("super-stretcher", PlayerPrefs.GetInt("super-stretcher") + quantity);
        }

        /// <summary>
        /// Неудачное завершение покупки
        /// </summary>
        public void OnPurchaseFailed(Product product, PurchaseFailureReason reason) {}
    }
}