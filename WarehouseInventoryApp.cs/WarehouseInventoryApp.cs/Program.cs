using System;
using System.Collections.Generic;

namespace DCIT318_Assignment3
{
    // Marker interface
    public interface IInventoryItem
    {
        int Id { get; }
        string Name { get; }
        int Quantity { get; set; }
    }

    // ElectronicItem class
    public class ElectronicItem : IInventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Brand { get; set; }
        public int WarrantyMonths { get; set; }

        public ElectronicItem(int id, string name, int quantity, string brand, int warrantyMonths)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            Brand = brand;
            WarrantyMonths = warrantyMonths;
        }
    }

    // GroceryItem class
    public class GroceryItem : IInventoryItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }

        public GroceryItem(int id, string name, int quantity, DateTime expiryDate)
        {
            Id = id;
            Name = name;
            Quantity = quantity;
            ExpiryDate = expiryDate;
        }
    }

    // Custom exceptions
    public class DuplicateItemException : Exception
    {
        public DuplicateItemException(string message) : base(message) { }
    }

    public class ItemNotFoundException : Exception
    {
        public ItemNotFoundException(string message) : base(message) { }
    }

    public class InvalidQuantityException : Exception
    {
        public InvalidQuantityException(string message) : base(message) { }
    }

    // Generic InventoryRepository
    public class InventoryRepository<T> where T : IInventoryItem
    {
        private Dictionary<int, T> _items = new();

        public void AddItem(T item)
        {
            if (_items.ContainsKey(item.Id))
                throw new DuplicateItemException("Item with the same ID already exists.");
            _items[item.Id] = item;
        }

        public T GetItemById(int id)
        {
            if (_items.TryGetValue(id, out T item))
                return item;
            throw new ItemNotFoundException("Item not found.");
        }

        public void RemoveItem(int id)
        {
            if (!_items.Remove(id))
                throw new ItemNotFoundException("Cannot remove item. ID not found.");
        }

        public List<T> GetAllItems()
        {
            return new List<T>(_items.Values);
        }

        public void UpdateQuantity(int id, int newQuantity)
        {
            if (newQuantity < 0)
                throw new InvalidQuantityException("Quantity cannot be negative.");

            if (_items.ContainsKey(id))
                _items[id].Quantity = newQuantity;
            else
                throw new ItemNotFoundException("Item not found for updating quantity.");
        }
    }

    // WarehouseManager
    public class WareHouseManager
    {
        private InventoryRepository<ElectronicItem> _electronics = new();
        private InventoryRepository<GroceryItem> _groceries = new();

        public void SeedData()
        {
            _electronics.AddItem(new ElectronicItem(1, "Laptop", 5, "Dell", 24));
            _electronics.AddItem(new ElectronicItem(2, "Smartphone", 10, "Samsung", 12));

            _groceries.AddItem(new GroceryItem(101, "Rice", 20, DateTime.Now.AddMonths(6)));
            _groceries.AddItem(new GroceryItem(102, "Milk", 15, DateTime.Now.AddDays(30)));
        }

        public void PrintAllItems<T>(InventoryRepository<T> repo) where T : IInventoryItem
        {
            foreach (var item in repo.GetAllItems())
            {
                Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}");
            }
        }

        public void IncreaseStock<T>(InventoryRepository<T> repo, int id, int quantity) where T : IInventoryItem
        {
            try
            {
                var item = repo.GetItemById(id);
                repo.UpdateQuantity(id, item.Quantity + quantity);
                Console.WriteLine($"Updated quantity for item ID {id}: {item.Quantity + quantity}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void RemoveItemById<T>(InventoryRepository<T> repo, int id) where T : IInventoryItem
        {
            try
            {
                repo.RemoveItem(id);
                Console.WriteLine($"Item ID {id} removed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public void RunSimulation()
        {
            Console.WriteLine("-- Grocery Items --");
            PrintAllItems(_groceries);

            Console.WriteLine("\n-- Electronic Items --");
            PrintAllItems(_electronics);

            Console.WriteLine("\n-- Testing Exception Handling --");
            try
            {
                _groceries.AddItem(new GroceryItem(101, "Duplicate Rice", 10, DateTime.Now));
            }
            catch (DuplicateItemException ex)
            {
                Console.WriteLine($"Duplicate error: {ex.Message}");
            }

            RemoveItemById(_electronics, 999); // Non-existent ID

            try
            {
                _groceries.UpdateQuantity(102, -5); // Invalid quantity
            }
            catch (InvalidQuantityException ex)
            {
                Console.WriteLine($"Quantity error: {ex.Message}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var warehouse = new WareHouseManager();
            warehouse.SeedData();
            warehouse.RunSimulation();
        }
    }
}
