using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleCRMApp.Data;
using SaleCRMApp.Models;
using SaleCRMApp.Models;
using System;
using System.IO;
using System.Threading.Tasks;
namespace SwadeshiApp.Controllers.Admin;
public class StateController : Controller
{
    private readonly ApplicationDbContext _context;

    public StateController(ApplicationDbContext context)
    {
        _context = context;
    }
    [HttpGet]
    public IActionResult Index()
    {
        var states = _context.States.ToList();
        return View(states);
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var state = _context.States.Find(id);
        if (state == null)
        {
            return NotFound();
        }

        return View(state);
    }

    // Edit action for handling the form submission
    [HttpPost]
    public async Task<IActionResult> Edit(int id, State model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            var state = await _context.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }

            state.StateName = model.StateName;
            state.StateDescription = model.StateDescription;
            state.StateImage = await ConvertImageToByteArrayAsync(model.StateImageFile);

            _context.Update(state);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        return View(model);
    }

    // Delete action for displaying the delete view
    [HttpGet]
    public IActionResult Delete(int id)
    {
        var state = _context.States.Find(id);
        if (state == null)
        {
            return NotFound();
        }

        return View(state);
    }

    // Delete action for handling the form submission
    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var state = await _context.States.FindAsync(id);
        if (state == null)
        {
            return NotFound();
        }

        _context.States.Remove(state);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(State model)
    {
        if (!ModelState.IsValid)
        {
            // Convert the IFormFile to a byte array
            model.StateImage = await ConvertImageToByteArrayAsync(model.StateImageFile);

            // Save other state details to the database
            _context.States.Add(model);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
            
        }

        // If the model is not valid, return to the create view with the model
        return RedirectToAction("Index");
    }

    private async Task<byte[]> ConvertImageToByteArrayAsync(IFormFile image)
    {
        if (image != null && image.Length > 0)
        {
            using (var memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                return memoryStream.ToArray();
            }
        }

        return null;
    }
}
