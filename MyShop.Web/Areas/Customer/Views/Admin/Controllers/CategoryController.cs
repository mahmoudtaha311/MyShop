//using Microsoft.AspNetCore.Mvc;
//using MyShop.DataAccess.Repository;
//using MyShop.Entities.Models;
//using MyShop.Entities.Services;

//namespace MyShop.Web.Areas.Admin.Controllers
//{
//    [Area("Admin")]
//    public class CategoryController : Controller
//    {

//        private readonly IUnitOfWork unitOfWork;

//        public CategoryController(IUnitOfWork unitOfWork)
//        {

//            this.unitOfWork = unitOfWork;
//        }
//        public IActionResult GetAll()
//        {
//            var categories = unitOfWork.Category.GeTAll();
//            return View(categories);
//            //var categories = categoryRepository.GetAllCategories();
//            //return View(categories);
//        }


//        public IActionResult GetCategoy(int id)
//        {
//            var category = unitOfWork.Category.GetFirstOrDefualt(x => x.Id == id);
//            if (category == null)
//            {
//                return NotFound();
//            }
//            return View(category);

//        }
//        [HttpGet]
//        public IActionResult Edit(int id)
//        {
//            var category = unitOfWork.Category.GetFirstOrDefualt(x => x.Id == id);

//            if (category == null)
//            {
//                return NotFound();
//            }
//            return View(category);
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult Edit(Category category)
//        {
//            if (ModelState.IsValid)
//            {
//                unitOfWork.Category.Update(category);
//                unitOfWork.Complete();
//                TempData["Update"] = " Data Has Updated Successfully ";

//                return RedirectToAction("GetAll");
//            }
//            return View(category);

//        }

//        [HttpGet]
//        public IActionResult Delete(int id)
//        {
//            var category = unitOfWork.Category.GetFirstOrDefualt(x => x.Id == id);

//            return View(category);
//        }

//        public IActionResult DeleteCatigory(Category category)
//        {
//            unitOfWork.Category.Delete(category);
//            unitOfWork.Complete();


//            TempData["Delete"] = " Data Has Deleted Successfully ";


//            return RedirectToAction("GetAll");
//        }
//        [HttpGet]
//        public IActionResult CreateCategory()
//        {
//            return View();
//        }
//        [HttpPost]
//        public IActionResult CreateCategory(Category category)
//        {
//            if (ModelState.IsValid)
//            {
//                unitOfWork.Category.Add(category);
//                unitOfWork.Complete();
//                TempData["Create"] = " Data Has Created Successfully ";

//                return RedirectToAction("GetAll");
//            }
//            return View(category);
//        }
//    }
//}
