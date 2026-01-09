using Microsoft.EntityFrameworkCore;

namespace Kagitv.Server.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options)
        : base(options)
    {
    }
}
