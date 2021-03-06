﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository repository;
        internal int PageSize = 4;

        public ProductController(IProductRepository productRepository)
        {
            repository = productRepository;
        }


        public ViewResult List(string category,int page = 1)
        {
            ProductsListViewModel model = new ProductsListViewModel()
            {
                Products = repository.Products
                .Where(product => category == null || product.Category == category)
                .OrderBy(product => product.ProductID).Skip((page - 1) * PageSize).Take(PageSize),
                PagingInfo = new PagingInfo() { CurrentPage =  page, 
                    ItemsPerPage = PageSize, 
                    TotalItems = category == null ? repository.Products.Count() : repository.Products.Count(product => product.Category == category)},
                CurrentCategory = category
            };

            return View(model);
        }


        public FileContentResult GetImage(int productid)
        {
            FileContentResult result;
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productid);
            if (product != null)
            {
                result = File(product.ImageData, product.ImageMimeType);
            }
            else
            {
                result = null;
            }

            return result;
        }
    }
}