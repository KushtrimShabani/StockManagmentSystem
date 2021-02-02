using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjektiSMS.Data;
using ProjektiSMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjektiSMS.Areas.UserPanel.Controllers
{
   


  
    [Authorize(Roles = "Admin, User")]
    [Area("UserPanel")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        public HomeController(ApplicationDbContext context) {


            _context = context;
        }
    
        public IActionResult Dashboard()
        {
            return View();
        }
        public ActionResult BestProduckt()
        {
            return View();
/*
return
            var topProductsIds = _context.Seller // table with a row for each view of a product
                .GroupBy(x => x.productID) //group all rows with same product id together
                .OrderByDescending(g => g.Sum(a => a.quantity)) // move products with highest views to the top
                .Take(3) // take top 5
                .Select(x => x.Key) // get id of products
                .ToList(); // execute query and convert it to a list

            var topProducts = _context.Products // table with products information
                .Where(x => topProductsIds.Contains(x.Id));



            *//*    List<Seller> seller = _context.Seller.GroupBy(x => x.quantity , )

                    //Where(d => d.productID != null).ToList();
               var query =(
                                from item in seller
                                group item.quantity by item.productID into g
                                orderby g.Sum() descending
                                select new { g.Key, Sum = g.Sum() }).Take(3);

                Seller = await query.FirstOrDefaultAsync();
    *//*


            return View(topProducts);*/
        }

    }
}
