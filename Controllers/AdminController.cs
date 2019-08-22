using Newtonsoft.Json;
using OnlineShoppingStore.DAL;
using OnlineShoppingStore.Models;
using OnlineShoppingStore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShoppingStore.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();

        public List<SelectListItem> GetCategory()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            var cat = _unitOfWork.GetRepositoryInstance<Tbl_Catrgory>().GetAllRecords();
            foreach(var item in cat)
            {
                list.Add(new SelectListItem { Value = item.CategoryId.ToString(), Text = item.CategoryName });
            }
            return list;
        }

        public ActionResult Dashboard()
        {
            return View();
        }

        public ActionResult Categories()
        {
            List<Tbl_Catrgory> allcategories = _unitOfWork.GetRepositoryInstance<Tbl_Catrgory>().GetAllRecordsIQueryable().Where(i=>i.IsDelete==false).ToList();
            return View(allcategories);
        }
        public ActionResult AddCategory()
        {
            return UpdateCategory(0);
        }

        public ActionResult UpdateCategory(int categoryId)
        {
            CategoryDetail cd;
                if (categoryId != null) 
                {
                    cd = JsonConvert.DeserializeObject<CategoryDetail>(JsonConvert.SerializeObject(_unitOfWork.GetRepositoryInstance<Tbl_Catrgory>().GetFirstorDefault(categoryId)));
                }
                else
                {
                    cd = new CategoryDetail();
                }
            return View("UpdateCategory", cd);
        
        }
        public ActionResult CategoryEdit(int catId)
        {
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Catrgory>().GetFirstorDefault(catId));
        }
        [HttpPost]
        public ActionResult CategoryEdit(Tbl_Catrgory tbl)
        { 
            _unitOfWork.GetRepositoryInstance<Tbl_Catrgory>().Update(tbl);
            return RedirectToAction("Categories");
        }
        public ActionResult Book()
        {
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Book>().GetBook());
        }

        public ActionResult BookEdit(int bookId)
        {
            ViewBag.CategoryList = GetCategory();
            return View(_unitOfWork.GetRepositoryInstance<Tbl_Book>().GetFirstorDefault(bookId));
        }
        [HttpPost]
        public ActionResult BookEdit(Tbl_Book tbl, HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);

                file.SaveAs(path);
            }
            tbl.ProductImage = file != null ? pic : tbl.ProductImage;
            tbl.ModifiedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Tbl_Book>().Update(tbl);
            return RedirectToAction("Book");
        }
        public ActionResult BookAdd()
        {
            ViewBag.CategoryList = GetCategory();  
            return View();
        }
        [HttpPost]
        public ActionResult BookAdd(Tbl_Book tbl, HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                 pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);

                file.SaveAs(path);
            }
            tbl.ProductImage = pic;
            tbl.CreatedDate = DateTime.Now;
            _unitOfWork.GetRepositoryInstance<Tbl_Book>().Add(tbl);
            return RedirectToAction("Book");
        }
    }
}