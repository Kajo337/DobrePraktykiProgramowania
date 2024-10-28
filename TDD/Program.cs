using System;
using Xunit;

// Interfejs PaymentGateway
public interface PaymentGateway
{
    TransactionResult Charge(string userId, double amount);
    TransactionResult Refund(string transactionId);
    TransactionStatus GetStatus(string transactionId);
}

// Interfejs ILogger
public interface ILogger
{
    void Log(string message);
}

// Klasa PaymentProcessor
public class PaymentProcessor
{
    private readonly PaymentGateway _gateway;
    private readonly ILogger _logger;

    public PaymentProcessor(PaymentGateway gateway, ILogger logger)
    {
        _gateway = gateway;
        _logger = logger;
    }

    public TransactionResult ProcessPayment(string userId, double amount)
    {
        if (string.IsNullOrWhiteSpace(userId))
        {
            var message = "Nieprawidłowy userId: pole jest puste.";
            _logger.Log(message);
            return new TransactionResult(false, null, message);
        }

        if (amount <= 0)
        {
            var message = "Kwota musi być dodatnia.";
            _logger.Log(message);
            return new TransactionResult(false, null, message);
        }

        try
        {
            var result = _gateway.Charge(userId, amount);
            if (result.Success)
            {
                _logger.Log("Płatność przetworzona pomyślnie.");
            }
            else
            {
                _logger.Log($"Płatność nie powiodła się: {result.Message}");
            }
            return result;
        }
        catch (NetworkException ex)
        {
            var message = $"Błąd sieciowy: płatność nieudana. {ex.Message}";
            _logger.Log(message);
            return new TransactionResult(false, null, message);
        }
        catch (PaymentException ex)
        {
            var message = $"Błąd płatności: {ex.Message}";
            _logger.Log(message);
            return new TransactionResult(false, null, message);
        }
    }

    public TransactionResult RefundPayment(string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId))
        {
            var message = "Nieprawidłowy transactionId: pole jest puste.";
            _logger.Log(message);
            return new TransactionResult(false, null, message);
        }

        try
        {
            var result = _gateway.Refund(transactionId);
            if (result.Success)
            {
                _logger.Log("Zwrot przetworzony pomyślnie.");
            }
            else
            {
                _logger.Log($"Zwrot nie powiódł się: {result.Message}");
            }
            return result;
        }
        catch (NetworkException ex)
        {
            var message = $"Błąd sieciowy podczas zwrotu: {ex.Message}";
            _logger.Log(message);
            return new TransactionResult(false, null, message);
        }
        catch (RefundException ex)
        {
            var message = $"Błąd zwrotu: {ex.Message}";
            _logger.Log(message);
            return new TransactionResult(false, null, message);
        }
    }

    public TransactionStatus GetPaymentStatus(string transactionId)
    {
        if (string.IsNullOrWhiteSpace(transactionId))
        {
            throw new ArgumentException("Nieprawidłowy transactionId: pole jest puste.", nameof(transactionId));
        }

        try
        {
            var status = _gateway.GetStatus(transactionId);
            _logger.Log($"Stan płatności: {status}");
            return status;
        }
        catch (NetworkException)
        {
            throw new Exception("Błąd sieciowy podczas pobierania statusu.");
        }
    }
}

// Klasa pomocnicza: TransactionResult
public class TransactionResult
{
    public bool Success { get; }
    public string TransactionId { get; }
    public string Message { get; }

    public TransactionResult(bool success, string transactionId, string message)
    {
        Success = success;
        TransactionId = transactionId;
        Message = message;
    }
}

// Enum: TransactionStatus
public enum TransactionStatus
{
    PENDING,
    COMPLETED,
    FAILED
}

// Wyjątki
public class NetworkException : Exception
{
    public NetworkException(string message) : base(message) { }
}

public class PaymentException : Exception
{
    public PaymentException(string message) : base(message) { }
}

public class RefundException : Exception
{
    public RefundException(string message) : base(message) { }
}

// Stub do PaymentGateway
public class StubPaymentGateway : PaymentGateway
{
    public TransactionResult ChargeResult { get; set; }
    public TransactionResult RefundResult { get; set; }
    public TransactionStatus GetStatusResult { get; set; }

    public TransactionResult Charge(string userId, double amount)
    {
        return ChargeResult;
    }

    public TransactionResult Refund(string transactionId)
    {
        return RefundResult;
    }

    public TransactionStatus GetStatus(string transactionId)
    {
        return GetStatusResult;
    }
}

// Stub do ILogger
public class StubLogger : ILogger
{
    public string LastLog { get; private set; }

    public void Log(string message)
    {
        LastLog = message;
    }
}

// Testy jednostkowe
public class PaymentProcessorTests
{
    [Fact]
    public void ProcesujPlatnosc_PowinnoZwracacSukces_GdyPlatnoscZostalaPrzetworzonaPomyślnie()
    {
        // Arrange
        var gateway = new StubPaymentGateway
        {
            ChargeResult = new TransactionResult(true, "trans123", "Obciążenie zakończone sukcesem.")
        };
        var logger = new StubLogger();
        var processor = new PaymentProcessor(gateway, logger);

        // Act
        var result = processor.ProcessPayment("user123", 150.00);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("trans123", result.TransactionId);
        Assert.Equal("Obciążenie zakończone sukcesem.", result.Message);
        Assert.Equal("Płatność przetworzona pomyślnie.", logger.LastLog);
    }

    [Fact]
    public void ProcesujPlatnosc_PowinnoZwracacNiepowodzenie_GdyUserIdJestPusty()
    {
        // Arrange
        var gateway = new StubPaymentGateway();
        var logger = new StubLogger();
        var processor = new PaymentProcessor(gateway, logger);

        // Act
        var result = processor.ProcessPayment("", 150.00);

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.TransactionId);
        Assert.Equal("Nieprawidłowy userId: pole jest puste.", result.Message);
        Assert.Equal("Nieprawidłowy userId: pole jest puste.", logger.LastLog);
    }

    [Fact]
    public void ProcesujPlatnosc_PowinnoZwracacNiepowodzenie_GdyKwotaJestUjemna()
    {
        // Arrange
        var gateway = new StubPaymentGateway();
        var logger = new StubLogger();
        var processor = new PaymentProcessor(gateway, logger);

        // Act
        var result = processor.ProcessPayment("user123", -50.00);

        // Assert
        Assert.False(result.Success);
        Assert.Null(result.TransactionId);
        Assert.Equal("Kwota musi być dodatnia.", result.Message);
        Assert.Equal("Kwota musi być dodatnia.", logger.LastLog);
    }

    [Fact]
    public void DokonajZwrotu_PowinnoZwracacSukces_GdyZwrotZostaniePrzetworzonyPomyślnie()
    {
        // Arrange
        var gateway = new StubPaymentGateway
        {
            RefundResult = new TransactionResult(true, "trans123", "Zwrot zakończony sukcesem.")
        };
        var logger = new StubLogger();
        var processor = new PaymentProcessor(gateway, logger);

        // Act
        var result = processor.RefundPayment("trans123");

        // Assert
        Assert.True(result.Success);
        Assert.Equal("trans123", result.TransactionId);
        Assert.Equal("Zwrot zakończony sukcesem.", result.Message);
        Assert.Equal("Zwrot przetworzony pomyślnie.", logger.LastLog);
    }

    [Fact]
    public void PobierzStatusPlatnosci_PowinnoZwracacStatus_GdyTransakcjaIstnieje()
    {
        // Arrange
        var gateway = new StubPaymentGateway
        {
            GetStatusResult = TransactionStatus.COMPLETED
        };
        var logger = new StubLogger();
        var processor = new PaymentProcessor(gateway, logger);

        // Act
        var status = processor.GetPaymentStatus("trans123");

        // Assert
        Assert.Equal(TransactionStatus.COMPLETED, status);
        Assert.Equal("Stan płatności: COMPLETED", logger.LastLog);
    }

    [Fact]
    public void PobierzStatusPlatnosci_PowinnoRzucacWyjatek_GdyTransactionIdJestPusty()
    {
        // Arrange
        var gateway = new StubPaymentGateway();
        var logger = new StubLogger();
        var processor = new PaymentProcessor(gateway, logger);

        // Act & Assert
        var ex = Assert.Throws<ArgumentException>(() => processor.GetPaymentStatus(""));
        Assert.Equal("Nieprawidłowy transactionId: pole jest puste.", ex.Message);
    }
}

// Metoda Main
public class Program
{
    public static void Main(string[] args)
    {
        
    }
}
