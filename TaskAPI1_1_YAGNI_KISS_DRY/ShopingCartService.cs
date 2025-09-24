using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskAPI1_1_YAGNI_KISS_DRY
{
    public class ShopingCartService
    {
        public decimal CalculateTotalPrice(string customerType, List<decimal> itemPrices) //Нарушение принципа KISS - Необходимость предварительного вычисления суммы по позициям, после чего итоговая сумма вычисляется в текущем методе.
        {
            decimal baseTotal = 0;
            for (int i = 0; i < itemPrices.Count; i++) //Нарушение принципа DRY - двойное перечисление позиций - здесь и в методе CalculateTotalPriceWithQuantities
            {
                baseTotal += itemPrices[i];
            }

            decimal discount = 0;

            //Получение цены с учетом скидки можно вывести в отдельный метод, для упрощения расширения функционала в дальнейшем
            if (customerType == "Regular")
            {
                discount = baseTotal * 0.05m; // 5%
            }
            else if (customerType == "Premium") //Нарушение принципа YAGNI - расчёт скидки для премиум покупателей не требуется
            {
                discount = baseTotal * 0.15m; // 15%
                if (discount > 1000)
                {
                    discount = 1000 + (discount - 1000) * 0.1m;
                }
            }
            else if (customerType == "VIP") //Нарушение принципа YAGNI - расчёт скидки для премиум покупателей не требуется
            {
                discount = baseTotal * 0.20m; // 20%
            }

            decimal finalPrice = baseTotal - discount;

            Console.WriteLine($"Base: {baseTotal}, Discount: {discount}, Final: {finalPrice}");
            return finalPrice;
        }

        public decimal CalculateTotalPriceWithQuantities(string customerType, Dictionary<decimal, int> itemsWithQuantities) //Нарушение принципа KISS - тип покупателя в методе не используется.
        {
            List<decimal> allPrices = new List<decimal>();
            foreach (var item in itemsWithQuantities)
            {
                //Нарушение принципа DRY - двойное перечисление позиций для вычисления итоговой стоимости - здесь и в методе CalculateTotalPrice
                //Нарушение принципа KISS - сложная логика добавления стоимости каждого элемента в отдельный список
                for (int i = 0; i < item.Value; i++)
                {
                    allPrices.Add(item.Key);
                }
            }
            return CalculateTotalPrice(customerType, allPrices);
        }
    }
}
