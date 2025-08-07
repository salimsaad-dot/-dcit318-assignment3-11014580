using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DCIT318_Assignment3
{
    // Marker Interface
    public interface IInventoryEntity
    {
        int Id { get; }
    }

    // Immutable InventoryItem record
    public record InventoryItem(int Id, string Name, int Quantity, DateTime DateAdded) : IInventoryEntity;

    // Generic Inventory Logger
    public class InventoryLogger<T> where T : IInventoryEntity
    {
        private List<T> _log = new();
        private string _filePath;

        public InventoryLogger(string filePath)
        {
            _filePath = filePath;
        }

        public void Add(T item)
        {
            _log.Add(item);
        }

        public List<T> GetAll()
        {
            return new List<T>(_log);
        }

        public void SaveToFile()
        {
            try
            {
                var json = JsonSerializer.Serialize(_log);
                File.WriteAllText(_filePath, json);
                Console.WriteLine("Inventory saved to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving to file: " + ex.Message);
            }
        }

        public void LoadFromFile()
        {
            try
            {
                if (File.Exists(_filePath))
                {
                    var json = File.ReadAllText(_filePath);
                    var items = JsonSerializer.Deserialize<List<T>>(json);
                    if (items != null)
                        _log = items;
                }
                else
                {
                    Console.WriteLine("File not found. Starting with an empty log.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error loading from file: " + ex.Message);
            }
        }
    }

    // Inventory App
    public class InventoryApp
    {
        private InventoryLogger<InventoryItem> _logger = new("inventory.json");

        public void SeedSampleData()
        {
            _logger.Add(new InventoryItem(1, "Notebook", 50, DateTime.Now));
            _logger.Add(new InventoryItem(2, "Pen", 100, DateTime.Now));
            _logger.Add(new InventoryItem(3, "Stapler", 20, DateTime.Now));
        }

        public void SaveData()
        {
            _logger.SaveToFile();
        }

        public void LoadData()
        {
            _logger.LoadFromFile();
        }

        public void PrintAllItems()
        {
            foreach (var item in _logger.GetAll())
            {
                Console.WriteLine($"ID: {item.Id}, Name: {item.Name}, Quantity: {item.Quantity}, Date Added: {item.DateAdded:yyyy-MM-dd}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var app = new InventoryApp();

            // Seed and save data
            app.SeedSampleData();
            app.SaveData();

            // Clear memory and simulate a new session
            Console.WriteLine("\nSimulating new session...\n");
            app = new InventoryApp();
            app.LoadData();
            app.PrintAllItems();
        }
    }
}
