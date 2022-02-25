using System;

namespace Odev_2.ViewModels
{
    public class OrdersViewModel
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}
