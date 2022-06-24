using CleanArch.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CleanArch.Infra.Data.Context
{
	public class ProductDbContext : DbContext
	{
		public ProductDbContext(DbContextOptions options) : base(options)
		{ }
		public DbSet<Product> Products { get; set; }
	}

}
