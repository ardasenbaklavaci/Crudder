
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Data;

public class CustomersModel : PageModel
{
    private readonly NewDbContext _context;

    public CustomersModel(NewDbContext context)
    {
        _context = context;
    }

    public List<Customers> Customers { get; set; }

    public async Task OnGetAsync()
    {
        Customers = await _context.Customers.ToListAsync();
    }
}