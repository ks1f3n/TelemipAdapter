using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TelemipAdapter;
using TelemipAdapter.Models.Gaps;

namespace TelemipAdapter.Controllers
{
    public class GapsController : Controller
    {
        private readonly SensorDbContext _context;

        public GapsController(SensorDbContext context)
        {
            _context = context;
        }

        // GET: Gaps
        [Authorize(Roles = "admin, user")]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Gap.ToListAsync());
        }

        // GET: Gaps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gap = await _context.Gap
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gap == null)
            {
                return NotFound();
            }

            return View(gap);
        }

        // GET: Gaps/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Gaps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InitValue,Value,Period,Id")] Gap gap)
        {
            if (ModelState.IsValid)
            {
                _context.Add(gap);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(gap);
        }

        // GET: Gaps/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gap = await _context.Gap.FindAsync(id);
            if (gap == null)
            {
                return NotFound();
            }
            return View(gap);
        }

        // POST: Gaps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("InitValue,Value,Period,Id")] Gap gap)
        {
            Gap currGap = new Gap();
            if (id != gap.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    currGap = await _context.Gap.FindAsync(gap.Id);
                    currGap.Period = gap.Period;
                    _context.Update(currGap);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GapExists(gap.Id))
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
            return View(currGap);
        }

        // GET: Gaps/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gap = await _context.Gap
                .FirstOrDefaultAsync(m => m.Id == id);
            if (gap == null)
            {
                return NotFound();
            }

            return View(gap);
        }

        // POST: Gaps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gap = await _context.Gap.FindAsync(id);
            _context.Gap.Remove(gap);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GapExists(int id)
        {
            return _context.Gap.Any(e => e.Id == id);
        }
    }
}
