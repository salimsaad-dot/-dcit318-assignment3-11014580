using System;
using System.Collections.Generic;

namespace DCIT318_Assignment3
{
    // Record type to represent a transaction
    public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

    // Interface for processing transactions
    public interface ITransactionProcessor
    {
        void Process(Transaction transaction);
    }

    public class BankTransferProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Bank Transfer] Processed GHC {transaction.Amount:F2} for {transaction.Category}");
        }
    }

    public class MobileMoneyProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Mobile Money] Processed GHC {transaction.Amount:F2} for {transaction.Category}");
        }
    }

    public class CryptoWalletProcessor : ITransactionProcessor
    {
        public void Process(Transaction transaction)
        {
            Console.WriteLine($"[Crypto Wallet] Processed GHC {transaction.Amount:F2} for {transaction.Category}");
        }
    }

    // Base Account class
    public class Account
    {
        public string AccountNumber { get; }
        public decimal Balance { get; protected set; }

        public Account(string accountNumber, decimal initialBalance)
        {
            AccountNumber = accountNumber;
            Balance = initialBalance;
        }

        public virtual void ApplyTransaction(Transaction transaction)
        {
            Balance -= transaction.Amount;
            Console.WriteLine($"Applied transaction of GHC {transaction.Amount:F2}. New Balance: GHC {Balance:F2}");
        }
    }

    // Sealed SavingsAccount class
    public sealed class SavingsAccount : Account
    {
        public SavingsAccount(string accountNumber, decimal initialBalance)
            : base(accountNumber, initialBalance) { }

        public override void ApplyTransaction(Transaction transaction)
        {
            if (transaction.Amount > Balance)
            {
                Console.WriteLine("Insufficient funds");
            }
            else
            {
                Balance -= transaction.Amount;
                Console.WriteLine($"Transaction of GHC {transaction.Amount:F2} applied. New Balance: GHC {Balance:F2}");
            }
        }
    }

    // FinanceApp class to simulate the system
    public class FinanceApp
    {
        private List<Transaction> _transactions = new();

        public void Run()
        {
            var account = new SavingsAccount("ACC12345", 1000m);

            var t1 = new Transaction(1, DateTime.Now, 150m, "Groceries");
            var t2 = new Transaction(2, DateTime.Now, 200m, "Utilities");
            var t3 = new Transaction(3, DateTime.Now, 300m, "Entertainment");

            ITransactionProcessor processor1 = new MobileMoneyProcessor();
            ITransactionProcessor processor2 = new BankTransferProcessor();
            ITransactionProcessor processor3 = new CryptoWalletProcessor();

            processor1.Process(t1);
            processor2.Process(t2);
            processor3.Process(t3);

            account.ApplyTransaction(t1);
            account.ApplyTransaction(t2);
            account.ApplyTransaction(t3);

            _transactions.AddRange(new[] { t1, t2, t3 });
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var app = new FinanceApp();
            app.Run();
        }
    }
}
