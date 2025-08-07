using System;
using System.Collections.Generic;
using System.Linq;

namespace DCIT318_Assignment3
{
    // Generic Repository
    public class Repository<T>
    {
        private List<T> items = new();

        public void Add(T item)
        {
            items.Add(item);
        }

        public List<T> GetAll()
        {
            return new List<T>(items);
        }

        public T? GetById(Func<T, bool> predicate)
        {
            return items.FirstOrDefault(predicate);
        }

        public bool Remove(Func<T, bool> predicate)
        {
            var item = items.FirstOrDefault(predicate);
            if (item != null)
            {
                items.Remove(item);
                return true;
            }
            return false;
        }
    }

    // Patient class
    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }
    }

    // Prescription class
    public class Prescription
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string MedicationName { get; set; }
        public DateTime DateIssued { get; set; }

        public Prescription(int id, int patientId, string medicationName, DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }
    }

    // HealthSystemApp
    public class HealthSystemApp
    {
        private Repository<Patient> _patientRepo = new();
        private Repository<Prescription> _prescriptionRepo = new();
        private Dictionary<int, List<Prescription>> _prescriptionMap = new();

        public void SeedData()
        {
            _patientRepo.Add(new Patient(1, "Alice Mensah", 30, "Female"));
            _patientRepo.Add(new Patient(2, "Kwame Boateng", 45, "Male"));
            _patientRepo.Add(new Patient(3, "Ama Owusu", 25, "Female"));

            _prescriptionRepo.Add(new Prescription(1, 1, "Paracetamol", DateTime.Now.AddDays(-5)));
            _prescriptionRepo.Add(new Prescription(2, 1, "Amoxicillin", DateTime.Now.AddDays(-3)));
            _prescriptionRepo.Add(new Prescription(3, 2, "Ibuprofen", DateTime.Now.AddDays(-7)));
            _prescriptionRepo.Add(new Prescription(4, 3, "Vitamin C", DateTime.Now.AddDays(-2)));
            _prescriptionRepo.Add(new Prescription(5, 3, "Cough Syrup", DateTime.Now.AddDays(-1)));
        }

        public void BuildPrescriptionMap()
        {
            foreach (var prescription in _prescriptionRepo.GetAll())
            {
                if (!_prescriptionMap.ContainsKey(prescription.PatientId))
                {
                    _prescriptionMap[prescription.PatientId] = new List<Prescription>();
                }
                _prescriptionMap[prescription.PatientId].Add(prescription);
            }
        }

        public void PrintAllPatients()
        {
            foreach (var patient in _patientRepo.GetAll())
            {
                Console.WriteLine($"ID: {patient.Id}, Name: {patient.Name}, Age: {patient.Age}, Gender: {patient.Gender}");
            }
        }

        public void PrintPrescriptionsForPatient(int id)
        {
            if (_prescriptionMap.ContainsKey(id))
            {
                Console.WriteLine($"Prescriptions for Patient ID {id}:");
                foreach (var prescription in _prescriptionMap[id])
                {
                    Console.WriteLine($"- {prescription.MedicationName} (Issued: {prescription.DateIssued:yyyy-MM-dd})");
                }
            }
            else
            {
                Console.WriteLine("No prescriptions found for this patient.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var app = new HealthSystemApp();
            app.SeedData();
            app.BuildPrescriptionMap();
            app.PrintAllPatients();

            Console.Write("\nEnter Patient ID to view prescriptions: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                app.PrintPrescriptionsForPatient(id);
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }
    }
}
