using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using TelemipAdapter.Models.Incls;

namespace TelemipAdapter.Controllers
{
    public class InclsController : Controller
    {
        private readonly SensorDbContext _context;

        public InclsController(SensorDbContext context)
        {
            _context = context;
        }

        // GET: Incls
        public async Task<IActionResult> Index()
        {
            return View(await _context.Incl.ToListAsync());
        }

        // GET: Incls/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incl = await _context.Incl
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incl == null)
            {
                return NotFound();
            }

            return View(incl);
        }

        // GET: Incls/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Incls/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InitX,X,InitY,Y,Period,Id")] Incl incl)
        {
            if (ModelState.IsValid)
            {
                _context.Add(incl);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(incl);
        }

        // GET: Incls/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incl = await _context.Incl.FindAsync(id);
            if (incl == null)
            {
                return NotFound();
            }
            return View(incl);
        }

        // POST: Incls/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InitX,X,InitY,Y,Period,Id")] Incl incl)
        {
            if (id != incl.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(incl);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InclExists(incl.Id))
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
            return View(incl);
        }

        // GET: Incls/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var incl = await _context.Incl
                .FirstOrDefaultAsync(m => m.Id == id);
            if (incl == null)
            {
                return NotFound();
            }

            return View(incl);
        }

        // POST: Incls/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incl = await _context.Incl.FindAsync(id);
            _context.Incl.Remove(incl);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InclExists(int id)
        {
            return _context.Incl.Any(e => e.Id == id);
        }
    }
}
