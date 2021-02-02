using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjektiSMS.Data;
using ProjektiSMS.Models;

namespace ProjektiSMS.Controllers
{
    [Authorize]
    public class SellersController : Controller
    {
        private readonly ApplicationDbContext _context;

     
        public SellersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Sellers
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Seller.Include(s => s.product);
            return View(await applicationDbContext.ToListAsync());
        }
        public async Task<IActionResult> Best()
        {
            var blogs = _context.Seller
                .FromSqlRaw(" select top(3) p.Id, SUM(s.quantity) as 'Sasia' from Seller s inner join Products p on s.productID = p.Id group by p.Id order by Sasia desc").Include(s => s.product);
/*
         
            var query = _context.Seller
                                    .Include(p => p.productID)
                                    
                                    .OrderBy(p => p.Customer.Name)
                                    .ThenBy(p => p.Title)
                                    .Select(p => new { Project = p.Title, Customer = p.Customer.Name });

            var queryString = query.ToQueryString();
            var result = query.FirstOrDefault();
            return View();
*/
            /*  var seller = _context.Seller.FromSql("select top(3) p.Id, SUM(s.quantity) as 'Sasia' from Seller s inner join Products p on s.productID = p.Id group by p.Id order by Sasia desc")
                            .Include(s => s.product)
                            .FirstOrDefault();
   */
            return View(await blogs.ToListAsync());
        }

        // GET: Sellers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Seller
                .Include(s => s.product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // GET: Sellers/Create
        public IActionResult Create()
        {
            ViewData["productID"] = new SelectList(_context.Products, "Id", "Id");
            return View();
        }

        // POST: Sellers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,price_sell,quantity,timeSeller,productID,CreateBy,CreateData,UpdateBy,UpdateData")] Seller seller)
        {
            if (ModelState.IsValid)
            {
                var product = await _context.Products.FindAsync(seller.productID);
                if (product.Quantity >= seller.quantity)
                {
                    product.Quantity -= seller.quantity;
                    _context.Update(product);
                     _context.Add(seller);
                    await _context.SaveChangesAsync();
                }else {
                    ModelState.AddModelError("", $"Cannot seller you have only {product.Quantity}");
                    return View(seller);
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["productID"] = new SelectList(_context.Products, "Id", "Id", seller.productID);
            return View(seller);
        }

        // GET: Sellers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Seller.FindAsync(id);
            if (seller == null)
            {
                return NotFound();
            }
            ViewData["productID"] = new SelectList(_context.Products, "Id", "Id", seller.productID);
            return View(seller);
        }

        // POST: Sellers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "EditSellerPolicy")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,price_sell,quantity,timeSeller,productID,CreateBy,CreateData,UpdateBy,UpdateData")] Seller seller)
        {
            if (id != seller.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(seller);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SellerExists(seller.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["productID"] = new SelectList(_context.Products, "Id", "Id", seller.productID);
            return View(seller);
        }

        // GET: Sellers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seller = await _context.Seller
                .Include(s => s.product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return View(seller);
        }

        // POST: Sellers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "DeleteSellerPolicy")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seller = await _context.Seller.FindAsync(id);
            _context.Seller.Remove(seller);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SellerExists(int id)
        {
            return _context.Seller.Any(e => e.Id == id);
        }
    }
}
