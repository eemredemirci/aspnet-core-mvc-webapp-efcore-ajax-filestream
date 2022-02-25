using Microsoft.AspNetCore.Mvc.Rendering;
using Odev2.Models;
using System;
using System.Collections.Generic;

namespace Odev2.ViewModels
{
    public class OrdersViewModel
    {
        public int OrderId { get; set; }
        public string CompanyName { get; set; }
        public DateTime Date { get; set; }
        public decimal Price { get; set; }
    }
}
