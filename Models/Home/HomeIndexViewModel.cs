﻿using OnlineShoppingStore.DAL;
using OnlineShoppingStore.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OnlineShoppingStore.Models.Home
{
    public class HomeIndexViewModel
    {
        public GenericUnitOfWork _unitOfWork = new GenericUnitOfWork();
        public IEnumerable<Tbl_Book> ListOfBooks { get; set; }
        public HomeIndexViewModel CreateModel()
        {

            return new HomeIndexViewModel()
            {
                ListOfBooks = _unitOfWork.GetRepositoryInstance<Tbl_Book>().GetAllRecords()
            };
        }
    }
}