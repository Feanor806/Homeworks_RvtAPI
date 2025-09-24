namespace TaskAPI1_1_YAGNI_KISS_DRY
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Потребительская корзина
            Dictionary<decimal, int> cart = new Dictionary<decimal, int>();

            cart[(decimal)1500.0] = 3;
            cart[(decimal)2000.0] = 2;
            cart[(decimal)750.0] = 7;

            //Вычисление стоимости старым методом
            Console.WriteLine("Вычисление стоимости корзины методом до рефакторинга:");
            new ShopingCartService().CalculateTotalPriceWithQuantities("Regular", cart);

            //Вычисление стоимости новым методом
            Console.WriteLine("Вычисление стоимости корзины отредактированным методом:");
            new ShopingCartServiceRefacotried().CalculateTotalPriceWithQuantities("Regular", cart);

            Console.ReadKey();
        }
    }
}
