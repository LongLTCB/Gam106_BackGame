using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; // Cần thiết cho DbContext
using WebApplication1.Data;
using WebApplication1.Models;
using System.Linq;

namespace WebApplication1.Controllers;

public class PlayerController : Controller
{
    private readonly ApplicationDbContext _context;

    public PlayerController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: Player/Index (Hiển thị danh sách)
    public IActionResult Index()
    {
        var players = _context.Players.ToList();
        return View(players);
    }

    // ------------------------------------------------------------------
    // A. CHỨC NĂNG THÊM MỚI (CREATE)
    // ------------------------------------------------------------------

    // GET: Player/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Player/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create([Bind("Name,Email,Score")] Player player)
    {
        if (ModelState.IsValid)
        {
            _context.Add(player);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(player);
    }

    // ------------------------------------------------------------------
    // B. CHỨC NĂNG SỬA (EDIT)
    // ------------------------------------------------------------------

    // GET: Player/Edit/5
    public IActionResult Edit(int? id)
    {
        if (id == null) return NotFound();

        var player = _context.Players.Find(id);
        if (player == null) return NotFound();

        return View(player);
    }

    // POST: Player/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, [Bind("PlayerId,Name,Email,Score")] Player player)
    {
        if (id != player.PlayerId) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(player);
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Players.Any(e => e.PlayerId == player.PlayerId))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(player);
    }

    // ------------------------------------------------------------------
    // C. CHỨC NĂNG XÓA (DELETE)
    // ------------------------------------------------------------------

    // GET: Player/Delete/5 (Hiển thị trang xác nhận)
    public IActionResult Delete(int? id)
    {
        if (id == null) return NotFound();

        var player = _context.Players.FirstOrDefault(m => m.PlayerId == id);

        if (player == null) return NotFound();

        return View(player);
    }

    // POST: Player/Delete/5 (Thực hiện xóa)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var player = _context.Players.Find(id);
        if (player != null)
        {
            _context.Players.Remove(player);
            _context.SaveChanges();
        }
        return RedirectToAction(nameof(Index));
    }
}