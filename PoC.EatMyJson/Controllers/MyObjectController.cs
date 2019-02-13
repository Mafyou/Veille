using System;
using Microsoft.AspNetCore.Mvc;
using PoC.EatMyJson.Models;
using PoC.EatMyJson.Repos;

namespace PoC.EatMyJson.Controllers
{
    [Route("api/MyObject")]
    public class MyObjectController : Controller
    {
        [HttpGet("GetMyObject")]
        public MyObject GetMyObject()
        {
            return new MyObjectRepository().GetMyObject();
        }
    }
}