using Microsoft.AspNetCore.Mvc;
using WEB_153503_DAINOVICH.Models;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WEB_153503_DAINOVICH.Controllers
{
    public class HomeController : Controller
    {
        public IEnumerable<ListDemo> Lists { get; set; } = new List<ListDemo>
        {
            new ListDemo(1, "Item 1"),
            new ListDemo(2, "Item 2"),
            new ListDemo(3, "Item 3"),
            new ListDemo(4, "Item 4"),
            new ListDemo(5, "Item 5"),
        };
        public IActionResult Index()
        {
            ViewData["LabName"] = "Лабораторная работа 2";
            ViewBag.Lists = new SelectList(Lists, "Id", "Name");
            return View();
        }
    }
}
