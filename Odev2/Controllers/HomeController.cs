using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Odev2.Extensions;
using Odev2.Models;
using Odev2.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Odev2.Controllers
{
    public class HomeController : Controller
    {
        public NORTHWNDContext context = new();
        public static Employee employeeLoggin;

        [HttpGet]
        public IActionResult Index()
        {
            LoginDTO model = new();
            ViewBag.User = employeeLoggin;
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(LoginDTO model)
        {

            if (employeeLoggin == null)
            {
                string LastName = model.UserName.ToLastName().FirstCharToUpper();
                string FirstName = model.UserName.ToFirstName().FirstCharToUpper();

                Employee employee = context.Employees.SingleOrDefault(a => a.FirstName == FirstName && a.LastName == LastName);

                if (employee != null && employee.LastName.FirstCharToLower() + employee.BirthDate.Value.Year == model.Password)
                {
                    employeeLoggin = new Employee
                    {
                        EmployeeId = employee.EmployeeId,
                        FirstName = employee.FirstName,
                        LastName = employee.LastName
                    };
                    return RedirectToAction("Order");
                }
                return NotFound("Kullanıcı bulunamadı!");
            }
            else
            {
                return BadRequest("Kullanıcı zaten giriş yaptı! Önce çıkış yapınız");
            }
        }

        public IActionResult Logout()
        {
            employeeLoggin = null;
            ViewBag.User = employeeLoggin;
            return RedirectToAction("Index");
        }

        public IActionResult Order()
        {
            if (employeeLoggin != null)
            {
                List<OrdersViewModel> ordersViewModels;

                var data = context.Orders
                .Where(a => a.EmployeeId == employeeLoggin.EmployeeId)
                .Include(a => a.Customer)
                .Include(a => a.OrderDetails)
                .Select(a => new OrdersViewModel()
                {
                    OrderId = a.OrderId,
                    Date = (DateTime)a.OrderDate,
                    CompanyName = a.Customer.CompanyName,
                    Price = a.OrderDetails.Sum(a => a.UnitPrice)
                })
                .ToList();

                ordersViewModels = data;

                // EF Core Query 2. Seçenek

                //List <Order> orders = context.Orders.Where(a => a.EmployeeId == employeeLoggin.EmployeeId)
                //  .Include(a => a.Customer)
                //  .Include(a => a.OrderDetails)
                //  .ToList();

                //foreach (var order in orders)
                //{
                //    ordersViewModels.Add(new OrdersViewModel()
                //    {
                //        OrderId = order.OrderId,
                //        Date = order.OrderDate.Value.Date,
                //        CompanyName = order.Customer.CompanyName,
                //        Price = order.OrderDetails.Sum(a => a.UnitPrice)
                //    });
                //}

                List<SelectListItem> items = new();
                foreach (var item in ordersViewModels)
                {
                    items.Add(new SelectListItem()
                    {
                        Text = item.CompanyName,
                        Value = item.OrderId.ToString()
                    });
                }

                ViewBag.Company = items;
                ViewBag.User = employeeLoggin;
                return View(ordersViewModels);
            }
            return Unauthorized("Kullanıcı giriş yapmalı!");
        }

        public IActionResult Profile()
        {
            ViewBag.User = employeeLoggin;

            Employee employee = context.Employees
                .Where(a => a.EmployeeId == employeeLoggin.EmployeeId)
                .SingleOrDefault();

            return View("ProfileList", employee);
        }

        [HttpPost]
        public IActionResult Profile(Employee employee)
        {
            ViewBag.User = employeeLoggin;

            var employeeGet = context.Employees
                .Where(a => a.EmployeeId == employee.EmployeeId)
                .SingleOrDefault();
            employeeGet.Address = employee.Address;
            employeeGet.HomePhone = employee.HomePhone;
            context.Employees.Update(employeeGet);
            context.SaveChanges();

            return View("ProfileList", employeeGet);
        }

        public IActionResult Download()
        {
            return View();
        }
    }
}
