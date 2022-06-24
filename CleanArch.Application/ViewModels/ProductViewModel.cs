using CleanArch.Domain.Entities;
using System.Collections.Generic;

namespace CleanArch.Application.ViewModels
{
    public class ProductViewModel
    {
        public IEnumerable<Product> Produtos { get; set; }
    }
}