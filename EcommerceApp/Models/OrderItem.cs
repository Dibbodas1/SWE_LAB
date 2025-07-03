using System.ComponentModel.DataAnnotations;

namespace EcommerceApp.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        public Order? Order { get; set; }
        
        public int ProductId { get; set; }
        public Product? Product { get; set; }
        
        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
        
        public decimal Price { get; set; }
    }
}