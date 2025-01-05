
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using Models;
using Data;

public class CustomersIndexModel : PageModel
{
    private readonly NewDbContext _context;

    public CustomersIndexModel(NewDbContext context)
    {
        _context = context;
    }

    public List<Customers> Customers { get; set; }
    public List<string> Columns { get; set; }

    public async Task OnGetAsync()
    {
        Customers = await _context.Customers.ToListAsync();
        Columns = new List<string>();

        foreach (var prop in typeof(Customers).GetProperties())
        {
            Columns.Add(prop.Name);
        }
    }
}