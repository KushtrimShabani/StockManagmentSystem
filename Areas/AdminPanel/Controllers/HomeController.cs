using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektiSMS.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektiSMS.Areas.AdminPanel.Controllers
{
    [Authorize(Roles ="Admin")]
  
 [Area("AdminPanel")]
    public class HomeController : Controller
    {
        
       

        public IActionResult Index()
        {
            return View();
        }
    }
}
