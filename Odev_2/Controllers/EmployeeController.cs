using Microsoft.AspNetCore.Mvc;
using Odev_2.Models;
using Odev_2.Extensions;
using System.Linq;
using System.Collections.Generic;

namespace Odev_2.Controllers
{
    public class EmployeeController : Controller
    {
        readonly NORTHWNDContext context = new();
        readonly Employee employeeLoggin = new();

        [HttpGet]
        public IActionResult Index()
        {
            LoginDTO model = new();

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(LoginDTO model)
        {

            if (employeeLoggin != null)
            {
                string LastName = model.UserName.ToLastName().FirstCharToUpper();
                string FirstName = model.UserName.ToFirstName().FirstCharToUpper();

                Employee employee = context.Employees.SingleOrDefault(a => a.FirstName == FirstName && a.LastName == LastName);
                if (employee != null && employee.LastName.FirstCharToLower() + employee.BirthDate.Value.Year == model.Password)
                {
                    employeeLoggin.EmployeeId = employee.EmployeeId;
                    employeeLoggin.FirstName = employee.FirstName;
                    employeeLoggin.LastName = employee.LastName;

                    return RedirectToAction("Order");
                }
                else
                {
                    return NotFound("Kullanıcı girişi yapınız");
                }
                
            }
            else
            {
                return BadRequest("Kullanıcı zaten giriş yaptı! Önce çıkış yapınız");
            }
        }

        public IActionResult Order()
        {

            //List<Order> orders = context.Orders.Where(a => a.EmployeeId == employeeLoggin.EmployeeId).ToList();

            return View();
        }
    }
}
