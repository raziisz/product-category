using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductCatalogy.Data;
using ProductCatalogy.Models;
using ProductCatalogy.ViewModels;
using ProductCatalogy.ViewModels.ProductViewModels;

namespace ProductCatalogy.Controllers
{
  public class ProductController : Controller
  {
    private readonly StoreDataContext _context;
    public ProductController(StoreDataContext context)
    {
      _context = context;
    }
    [HttpGet("v1/products")]
    public IEnumerable<ListProductViewModel> Get()
    {
        return _context.Products
            .Include(x => x.Category)
            .Select(x => new ListProductViewModel
            {
                Id = x.Id,
                Title = x.Title,
                Price = x.Price,
                Category = x.Category.Title,
                CategoryId = x.Category.Id
            })
            .AsNoTracking()
            .ToList();
    }

    [HttpGet("v1/products/{id}")]
    public Product Get(int id)
    {
        return _context.Products.AsNoTracking().Where(x => x.Id == id).FirstOrDefault();
    }

    [HttpPost("v1/products")]
    public ResultViewModel Post([FromBody]EditorProductViewModel model)
    {
        model.Validate();
        if(model.Invalid) 
            return new ResultViewModel
            {
                Success = false,
                Message = "Não foi possível cadastrar o produto",
                Data = model.Notifications
            };
        
        var product = new Product();
        product.Title = model.Title;
        product.CategoryId = model.CategoryId;
        product.CreateDate = DateTime.Now;
        product.Description = model.Description;
        product.Image = model.Image;
        product.LastUpdateDate = DateTime.Now;
        product.Price = model.Price;
        product.Quantity = model.Quantity;

        _context.Products.Add(product);
        _context.SaveChanges();

        return new ResultViewModel
        {
            Success = true,
            Message = "Produto cadastrado com sucesso",
            Data = product
        };
    }
    [HttpPut("v1/products")]
    public ResultViewModel Put([FromBody]EditorProductViewModel model)
    {
        model.Validate();
        if(model.Invalid) 
            return new ResultViewModel
            {
                Success = false,
                Message = "Não foi possível cadastrar o produto",
                Data = model.Notifications
            };
        
        var product = _context.Products.Find(model.Id);
        product.Title = model.Title;
        product.CategoryId = model.CategoryId;
        // product.CreateDate = DateTime.Now;
        product.Description = model.Description;
        product.Image = model.Image;
        product.LastUpdateDate = DateTime.Now;
        product.Price = model.Price;
        product.Quantity = model.Quantity;

        _context.Entry<Product>(product).State = EntityState.Modified;
        _context.SaveChanges();

        return new ResultViewModel
        {
            Success = true,
            Message = "Produto alterado com sucesso",
            Data = product
        };
    }
  }
}