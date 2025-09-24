using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI1_1_YAGNI_KISS_DRY
{
    public class ShopingCartServiceRefacotried
    {
        public decimal CalculateTotalPrice(string customerType, decimal baseTotal)
        {
            //Исключен дополнительный цикл суммирования цен позиций
            //Сумма скидки вычисляется в отдельном методе
            decimal discount = GetDiscount(customerType, baseTotal);

            decimal finalPrice = baseTotal - discount;

            Console.WriteLine($"Base: {baseTotal}, Discount: {discount}, Final: {finalPrice}");
            return finalPrice;
        }

        public decimal CalculateTotalPriceWithQuantities(string customerType, Dictionary<decimal, int> itemsWithQuantities)
        {
            //Упрощена логика вычисления базовой стоимости корзины
            decimal baseTotal = new();
            foreach (var item in itemsWithQuantities)
            {
                baseTotal += item.Key * item.Value;
            }
            return CalculateTotalPrice(customerType, baseTotal);
        }

        //Вычисление скидки выведено в отдельный метод с использованием конструкции switch
        public decimal GetDiscount(string customerType, decimal baseTotal)
        {
            decimal discount = 0;
            switch (customerType)
            {
                case "Regular":
                    discount = baseTotal * 0.05m;
                    break;
            }

            return discount;
        }
    }
}
