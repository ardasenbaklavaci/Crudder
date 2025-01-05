
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Data;

public class ProductsIndexModel : PageModel
{
    private readonly NewDbContext _context;

    public ProductsIndexModel(NewDbContext context)
    {
        _context = context;
    }

    public List<Products> Products { get; set; }
    public List<string> Columns { get; set; }

    public async Task OnGetAsync()
    {
        Products = await _context.Products.ToListAsync();
        Columns = new List<string>();

        foreach (var prop in typeof(Products).GetProperties())
        {
            Columns.Add(prop.Name);
        }
    }
}