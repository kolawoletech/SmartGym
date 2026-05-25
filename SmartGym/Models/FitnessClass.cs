using System;

namespace SmartGym.Models
{
    public class FitnessClass
    {
        public int ClassId { get; set; }
        public string ClassName { get; set; }
        public string Instructor { get; set; }
        public DateTime Schedule { get; set; }
        public int Capacity { get; set; }
        public decimal CreditCost { get; set; }
        public string Description { get; set; }
    }
}
