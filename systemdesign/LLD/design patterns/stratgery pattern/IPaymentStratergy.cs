using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Enter the payment mode (Cash/CreditCard/PayPal/UPI):");

        foreach (var mode in Enum.GetValues(typeof(PaymentMode)))
        {
            Console.WriteLine($"- {mode}");
        }

        var paymentModeInput = Console.ReadLine()?.Trim();

        try
        {
            var validMode = PaymentContext.IsPaymentModeValid(paymentModeInput);

            var paymentContext = new PaymentContext();
            paymentContext.SetPaymentStrategy(PaymentFactory.GetPaymentStrategy(validMode));

            Console.WriteLine("Enter the amount to pay:");
            if (decimal.TryParse(Console.ReadLine(), out var amount))
            {
                var payStatus = paymentContext.Pay(amount);
                Console.WriteLine($"Payment status: {(payStatus ? "Success" : "Failure")}");
            }
            else
            {
                Console.WriteLine("Invalid amount entered.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}

public enum PaymentMode
{
    Cash,
    CreditCard,
    PayPal,
    UPI
}

// Interfaces should use the 'interface' keyword, not 'public class'
public interface IPaymentStrategy
{
    string? PayMode { get; set; }
    bool Pay(decimal amount);
}

public class PaymentContext
{
    private IPaymentStrategy? _paymentStrategy;

    public PaymentContext(IPaymentStrategy? paymentStrategy = null)
    {
        _paymentStrategy = paymentStrategy;
    }

    public void SetPaymentStrategy(IPaymentStrategy paymentStrategy)
    {
        _paymentStrategy = paymentStrategy ?? throw new ArgumentNullException(nameof(paymentStrategy));
    }

    public static PaymentMode IsPaymentModeValid(string? paymentMode)
    {
        if (string.IsNullOrEmpty(paymentMode))
        {
            throw new ArgumentException("Payment mode cannot be null or empty.");
        }

        if (Enum.TryParse<PaymentMode>(paymentMode, ignoreCase: true, out var result) && Enum.IsDefined(typeof(PaymentMode), result))
        {
            return result;
        }
        else
        {
            throw new Exception($"Invalid payment mode: {paymentMode}. Valid modes are: {string.Join(", ", Enum.GetNames(typeof(PaymentMode)))}");
        }
    }

    public bool Pay(decimal amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Amount should be greater than zero.");
            return false;
        }

        if (_paymentStrategy == null)
        {
            throw new InvalidOperationException("Payment strategy is not set.");
        }

        return _paymentStrategy.Pay(amount);
    }
}

// Factory class to adhere to the Open/Closed Principle
public static class PaymentFactory
{
    public static IPaymentStrategy GetPaymentStrategy(PaymentMode mode)
    {
        return mode switch
        {
            PaymentMode.Cash => new CashPayment(),
            PaymentMode.CreditCard => new CreditCardPayment(),
            PaymentMode.PayPal => new PayPalPayment(),
            PaymentMode.UPI => new UpiPayment(),
            _ => throw new NotSupportedException($"Payment mode {mode} is not supported.")
        };
    }
}

public class CashPayment : IPaymentStrategy
{
    public string? PayMode { get; set; } = nameof(PaymentMode.Cash);

    public bool Pay(decimal amount)
    {
        Console.WriteLine($"Paying {amount:C} using Cash.");
        return true;
    }
}

public class CreditCardPayment : IPaymentStrategy
{
    public string? PayMode { get; set; } = nameof(PaymentMode.CreditCard);

    public bool Pay(decimal amount)
    {
        Console.WriteLine($"Paying {amount:C} using Credit Card.");
        return true;
    }
}

public class PayPalPayment : IPaymentStrategy
{
    public string? PayMode { get; set; } = nameof(PaymentMode.PayPal);

    public bool Pay(decimal amount)
    {
        Console.WriteLine($"Paying {amount:C} using PayPal.");
        return true;
    }
}

public class UpiPayment : IPaymentStrategy
{
    public string? PayMode { get; set; } = nameof(PaymentMode.UPI);

    public bool Pay(decimal amount)
    {
        Console.WriteLine($"Paying {amount:C} using UPI.");
        return true;
    }
}