using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using cloudscribe.Pagination.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using E_Shop.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;

namespace E_Shop.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;

        public Phone Phone { get; set; }

        public HomeController(AppDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(string searchString, string manufacturer, int? priceFrom, int? priceTo, int pageNumber = 1, int pageSize = 6)
        {
            IQueryable<Phone> phones = _db.Phones;

            if (!String.IsNullOrEmpty(searchString))
                phones = phones.Where(x => x.Name.Contains(searchString));

            if (!String.IsNullOrEmpty(manufacturer))
                phones = phones.Where(x => x.Manufacturer == manufacturer);

            if (priceFrom.HasValue)
                phones = phones.Where(x => x.Price >= priceFrom);

            if (priceTo.HasValue)
                phones = phones.Where(x => x.Price <= priceTo);


            var excludeRecords = (pageSize * pageNumber) - pageSize;
            var phoneCount = phones.Count();
            phones = phones.Skip(excludeRecords)
                           .Take(pageSize);


            var result = new PagedResult<Phone>
            {
                Data = phones.AsNoTracking().ToList(),
                TotalItems = phoneCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            ViewBag.Show = true;
            return View(result);
        }

        public IActionResult Upsert(int? id)
        {
            if (id == null)
            {
                //create
                return View(new Phone());
            }
            //update
            Phone = _db.Phones.FirstOrDefault(u => u.Id == id);
            if (Phone == null)
            {
                return NotFound();
            }
            return View(Phone);
        }

        [HttpPost]
        public IActionResult Upsert(Phone phone)
        {
            if (ModelState.IsValid)
            {
                if (phone.Id == 0)
                {
                    _db.Phones.Add(phone);
                }
                else
                {
                    _db.Phones.Update(phone);
                }

                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(phone);
        }

        public IActionResult Delete(int? id)
        {
            Phone = _db.Phones.FirstOrDefault(u => u.Id == id);
            if (Phone == null)
            {
                return NotFound();
            }
            _db.Phones.Remove(Phone);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult PhoneDetails(int id)
        {
            Phone = _db.Phones.FirstOrDefault(u => u.Id == id);
            if (Phone == null)
            {
                return NotFound();
            }
            return View(Phone);
        }
    }
}
