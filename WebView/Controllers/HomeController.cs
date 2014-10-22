using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.DomainModel;
using Core.Interface.Service;
using Data.Repository;
using Service.Service;
using Validation.Validation;

namespace WebView.Controllers
{
    public class HomeController : Controller
    {
        
        public HomeController()
        {
            
        }

        //
        // GET: /Home/
   
        public ActionResult Index()
        {
            return View();
        }



    
       
    }
}
