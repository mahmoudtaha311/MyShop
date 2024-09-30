using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyShop.DataAccess.Repository;
using MyShop.Entities.Models;
using MyShop.Entities.Services;
using MyShop.Entities.ViewModels;

namespace MyShop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {

            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }
        public IActionResult GetAll()
        {
            
            return View();
            
        }

        //public IActionResult GetData()
        //{

        //    var products = unitOfWork.Product.GeTAll(IncludeWord : "category");

        //    return Json(new {data = products });

        //}

        public IActionResult GetData()
        {
            var products = unitOfWork.Product.GeTAll(IncludeWord: "Category")
                .Select(p => new ProductDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Img = p.Img,
                    Price = p.Price,
                    CategoryName = p.Category.Name // فقط اسم الفئة
                })
                .ToList();

            return Json(new { data = products });
        }




        public IActionResult GetProduct(int id)
        {
            var product = unitOfWork.Product.GetFirstOrDefualt(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);

        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            
            ProductVM productVM = new ProductVM()
            {
                Product = unitOfWork.Product.GetFirstOrDefualt(x => x.Id == id),
                CategoryList = unitOfWork.Category.GeTAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })

            };

          
            return View(productVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM productVM , IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var Uploud = Path.Combine(RootPath, @"Images\Products");
                    var ext = Path.GetExtension(file.FileName);
                    if(productVM.Product.Img != null)
                    {
                        var oldimg = Path.Combine(RootPath,productVM.Product.Img.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimg))
                        {
                            System.IO.File.Delete(oldimg);
                        }
                    }
                    using (var filestream = new FileStream(Path.Combine(Uploud, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productVM.Product.Img = @"Images\Products\" + filename + ext;
                }
                unitOfWork.Product.Update(productVM.Product);
                unitOfWork.Complete();
                TempData["Update"] = " Data Has Updated Successfully ";

                return RedirectToAction("GetAll");
            }
            return View(productVM.Product);

        }


        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            Product product = unitOfWork.Product.GetFirstOrDefualt(x => x.Id == id);
            if (product == null)
            {
                return Json(new { success = false , message ="Error While Deleting" });
               
            }
           
                var oldimg = Path.Combine(webHostEnvironment.WebRootPath , product.Img.TrimStart('\\'));
                if (System.IO.File.Exists(oldimg))
                {
                    System.IO.File.Delete(oldimg);
                }
            
            unitOfWork.Product.Delete(product);
            unitOfWork.Complete();
            return Json(new { success = true, message = "File Hase be Deleting" });



        }
        [HttpGet]
        public IActionResult CreateProduct()
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = unitOfWork.Category.GeTAll().Select(x => new SelectListItem { Text = x.Name , Value = x.Id.ToString()})
                
            };
                
            return View(productVM);
        }
        [HttpPost]
        public IActionResult CreateProduct(ProductVM productVM,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = webHostEnvironment.WebRootPath;
                if (file != null) 
                {
                    string filename = Guid.NewGuid().ToString();
                    var Uploud = Path.Combine(RootPath, @"Images\Products");
                    var ext = Path.GetExtension(file.FileName);
                    using (var filestream = new FileStream(Path.Combine(Uploud,filename+ext) ,FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productVM.Product.Img = @"Images\Products\" + filename + ext;
                }
                unitOfWork.Product.Add(productVM.Product);
                unitOfWork.Complete();
                TempData["Create"] = " Data Has Created Successfully ";

                return RedirectToAction("GetAll");
            }
            return View(productVM.Product);
        }
    }
}
