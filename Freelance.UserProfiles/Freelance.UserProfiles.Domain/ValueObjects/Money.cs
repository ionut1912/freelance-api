using Freelance.Shared.Domain.Common;

namespace Freelance.UserProfiles.Domain.ValueObjects;

public class Money : ValueObject
{
    public decimal Amount { get; private set; }
    public string Currency { get; private set; }

    private Money() { } // For EF Core

    private Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be empty", nameof(currency));

        Amount = amount;
        Currency = currency.ToUpperInvariant();
    }

    public static Money Create(decimal amount, string currency)
    {
        return new Money(amount, currency);
    }

    public Money Add(Money other)
    {
        return Currency != other.Currency ? throw new InvalidOperationException($"Cannot add money with different currencies: {Currency} and {other.Currency}") : new Money(Amount + other.Amount, Currency);
    }

    public Money Subtract(Money other)
    {
        return Currency != other.Currency ? throw new InvalidOperationException($"Cannot subtract money with different currencies: {Currency} and {other.Currency}") : new Money(Amount - other.Amount, Currency);
    }

    public Money Multiply(decimal multiplier)
    {
        return new Money(Amount * multiplier, Currency);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Amount;
        yield return Currency;
    }

    public override string ToString()
    {
        return $"{Amount:N2} {Currency}";
    }
}