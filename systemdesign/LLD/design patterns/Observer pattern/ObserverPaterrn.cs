


// Observer Pattern in C#


// notification interface that defines the Notify method
class INotification
{
    public void Notify(int orderId);

}


// Concrete notification classes that implement the INotification interface 
class EmailNotification : INotification
{
    public void Notify(int orderId)
    {
        Console.WriteLine($"Email notification sent for order {orderId}");
    }
}


class SMSNotification : INotification
{
    public void Notify(int orderId)
    {
        Console.WriteLine($"SMS notification sent for order {orderId}");
    }
}


class WarehouseNotification: INotification
{
    public void Notify(int orderId)
    {
        Console.WriteLine($"Warehouse notification sent for order {orderId}");
    }
}



// NotificationService class that manages the observers and notifies them when an order is placed : subject in the observer pattern

class NotificationService
{

    private readonly List<INotification> _Observers = new List<INotification>();

    public void Subscribe(INotification subscriber)
    {
        _Observers.Add(subscriber);
    }

    public void UnSubscribe(INotification subscriber)
    {
        _Observers.Remove(subscriber);
    }

    public void PlaceOrder(int orderId)
    {
        Console.WriteLine($"Order {orderId} placed successfully.");
        NotifyAll(orderId);
    }

    public void NotifyAll(int orderId)
    {
        foreach (var observer in _Observers)
        {
            observer.Notify(orderId);
        }
    }


}




// usage of the observer pattern
class ObserverPattern
{
    static void Main(string[] args)
    {
        var notificationService = new NotificationService();
        var emailNotification = new EmailNotification();
        var smsNotification = new SMSNotification();
        var warehouseNotification = new WarehouseNotification();
        notificationService.Subscribe(emailNotification);
        notificationService.Subscribe(smsNotification);
        notificationService.Subscribe(warehouseNotification);
        notificationService.PlaceOrder(12345);
        notificationService.UnSubscribe(smsNotification);
        notificationService.PlaceOrder(67890);
    }
}