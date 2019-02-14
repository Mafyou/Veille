using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FillMyJson.Models;
using FillMyJson.Helpers;
using System.Collections.Generic;

namespace FillMyJson.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost("/Home/PostFormData")]
        public void PostFormData([FromForm] UserViewModel newUser)
        {
            ActionsOnJson.SaveMyUser(newUser.User);
        }

        [HttpGet("Home/GetUsers")]
        public List<User> GetUsers()
        {
            List<User> users = ActionsOnJson.ReadMyUsers();
            return users;
        }

        [HttpPost("/Home/ResetJson")]
        public void ResetJson()
        {
            ActionsOnJson.ResetJson();
        }
    }
}