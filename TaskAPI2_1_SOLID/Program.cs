namespace TaskAPI2_1_SOLID
{
    internal class Program
    {
        static void Main()
        {
            var service = new NotificationService();
            service.SendNotification("Ваш заказ готов", "user@example.com");
        }
    }
}
