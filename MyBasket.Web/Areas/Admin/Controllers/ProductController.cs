using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MyBasket.Domain;
using MyBasket.Domain.Models;
using MyBasket.Domain.Repository;
using MyBasket.Domain.ViewModels;
using MyBasket.Infrastructure.Implementation;
using MyBasket.Web.Migrations;


namespace MyBasket.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork , IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult GetData()
        {
            var products = _unitOfWork.Product.GetAll(Includeword:"Category");
            return Json(new {data = products});
        }

        /******************************************* Create ****************************************/

        [HttpGet]
        public IActionResult Create()
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ProductVM productVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if(file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(RootPath, @"Images\Products");
                    var ext = Path.GetExtension(file.FileName);
                    using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productVM.Product.Img = @"Images\Products\" + filename + ext;
                }
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Complete();
                TempData["Create"] = "Item Has Created Successfully";
                return RedirectToAction("Index");
            }
            return View(productVM.Product);
        }

        /******************************************* EDIT ****************************************/

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null | id == 0)
            {
                NotFound();
            }

            ProductVM productVM = new ProductVM()
            {
                Product = _unitOfWork.Product.GetFirstorDefault(x => x.Id == id),
                CategoryList = _unitOfWork.Category.GetAll().Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.Id.ToString()
                })
            };
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string RootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string filename = Guid.NewGuid().ToString();
                    var Upload = Path.Combine(RootPath, @"Images\Products");
                    var ext = Path.GetExtension(file.FileName);

                    if(productVM.Product.Img != null)
                    {
                        var oldimg = Path.Combine(RootPath, productVM.Product.Img.TrimStart('\\'));
                        if (System.IO.File.Exists(oldimg))
                        {
                            System.IO.File.Delete(oldimg);
                        }
                    }

                    using (var filestream = new FileStream(Path.Combine(Upload, filename + ext), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }
                    productVM.Product.Img = @"Images\Products\" + filename + ext;
                }

                _unitOfWork.Product.Update(productVM.Product);
                _unitOfWork.Complete();
                TempData["Update"] = "Data Has Updated Successfully";
                return RedirectToAction("Index");
            }
            return View(productVM.Product);
        }


        /******************************************* Delete ****************************************/


        [HttpDelete]
        public IActionResult DeleteProduct(int? id)
        {
            var productInDb = _unitOfWork.Product.GetFirstorDefault(x => x.Id == id);
            if (productInDb == null)
            {
                return Json(new { success = false, message = "Error while deleting" });
            }   
                var oldImgPath = Path.Combine(_webHostEnvironment.WebRootPath, productInDb.Img.TrimStart('\\'));
                if (System.IO.File.Exists(oldImgPath))
                {
                    System.IO.File.Delete(oldImgPath);
                }

            _unitOfWork.Product.Remove(productInDb);
            _unitOfWork.Complete();
            return Json(new { success = true, message = "Product has been deleted" });
        }



    }
    
}
