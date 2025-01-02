
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Data;

public class ProductsModel : PageModel
{
    private readonly NewDbContext _context;

    public ProductsModel(NewDbContext context)
    {
        _context = context;
    }

    public List<Products> Products { get; set; }

    public async Task OnGetAsync()
    {
        Products = await _context.Products.ToListAsync();
    }
}