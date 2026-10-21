
using System;

public interface IOrderState
{
    void ShipOrder(Order order);
    void ConfirmOrder(Order order);
    void CancelOrder(Order order);
    void DeliverOrder(Order order);
}

public class Order
{
    public int ID { get; }

    private IOrderState _state;

    public Order(int id)
    {
        ID = id;
        _state = new PlacedOrderState();
    }

    public void SetOrderState(IOrderState state)
    {
        _state = state;
    }

    public void ShipOrder() => _state.ShipOrder(this);
    public void ConfirmOrder() => _state.ConfirmOrder(this);
    public void CancelOrder() => _state.CancelOrder(this);
    public void DeliverOrder() => _state.DeliverOrder(this);
}

// 1. Placed state
public class PlacedOrderState : IOrderState
{
    public void ConfirmOrder(Order order)
    {
        Console.WriteLine($"Order {order.ID} confirmed.");
        order.SetOrderState(new ConfirmedOrderState());
    }

    public void ShipOrder(Order order)
    {
        Console.WriteLine($"Order {order.ID} must be confirmed before shipping.");
    }

    public void CancelOrder(Order order)
    {
        Console.WriteLine($"Order {order.ID} cancelled.");
        order.SetOrderState(new CancelledOrderState());
    }

    public void DeliverOrder(Order order)
    {
        Console.WriteLine($"Order {order.ID} must be shipped before delivery.");
    }
}

// 2. Confirmed state
public class ConfirmedOrderState : IOrderState
{
    public void ConfirmOrder(Order order)
    {
        Console.WriteLine($"Order {order.ID} is already confirmed.");
    }

    public void ShipOrder(Order order)
    {
        Console.WriteLine($"Order {order.ID} shipped.");
        order.SetOrderState(new ShippedOrderState());
    }

    public void CancelOrder(Order order)
    {
        Console.WriteLine($"Order {order.ID} cancelled.");
        order.SetOrderState(new CancelledOrderState());
    }

    public void DeliverOrder(Order order)
    {
        Console.WriteLine($"Order {order.ID} must be shipped before delivery.");
    }
}

// 3. Shipped state
public class ShippedOrderState : IOrderState
{
    public void ConfirmOrder(Order order)
    {
        Console.WriteLine($"Order {order.ID} is already shipped.");
    }

    public void ShipOrder(Order order)
    {
        Console.WriteLine($"Order {order.ID} is already shipped.");
    }

    public void CancelOrder(Order order)
    {
        Console.WriteLine($"Order {order.ID} cannot be cancelled after shipping.");
    }

    public void DeliverOrder(Order order)
    {
        Console.WriteLine($"Order {order.ID} delivered.");
        order.SetOrderState(new DeliveredOrderState());
    }
}

// 4. Cancelled state
public class CancelledOrderState : IOrderState
{
    public void ConfirmOrder(Order order) =>
        Console.WriteLine($"Order {order.ID} is cancelled.");

    public void ShipOrder(Order order) =>
        Console.WriteLine($"Order {order.ID} cannot be shipped.");

    public void CancelOrder(Order order) =>
        Console.WriteLine($"Order {order.ID} is already cancelled.");

    public void DeliverOrder(Order order) =>
        Console.WriteLine($"Order {order.ID} cannot be delivered.");
}

// 5. Delivered state
public class DeliveredOrderState : IOrderState
{
    public void ConfirmOrder(Order order) =>
        Console.WriteLine($"Order {order.ID} is already delivered.");

    public void ShipOrder(Order order) =>
        Console.WriteLine($"Order {order.ID} is already delivered.");

    public void CancelOrder(Order order) =>
        Console.WriteLine($"Order {order.ID} cannot be cancelled after delivery.");

    public void DeliverOrder(Order order) =>
        Console.WriteLine($"Order {order.ID} is already delivered.");
}

public class Program
{
    public static void Main()
    {
        Order order = new Order(1);

        order.ShipOrder();     // Rejected: Placed
        order.ConfirmOrder();  // Placed -> Confirmed
        order.ShipOrder();     // Confirmed -> Shipped
        order.DeliverOrder();  // Shipped -> Delivered
        order.CancelOrder();   // Rejected: Delivered
    }
}
