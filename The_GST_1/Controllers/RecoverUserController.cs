﻿using Data_Access_Layer.Db_Context;
using Microsoft.AspNetCore.Mvc;
using Repository_Logic.QueryRepository.Interface;
using Repository_Logic.RecoverUser.Interface;

namespace The_GST_1.Controllers
{
    public class RecoverUserController : Controller
    {
        private readonly IRecoverUser _recover;
        private readonly Application_Db_Context _context;
        public RecoverUserController(IRecoverUser recover, Application_Db_Context context)
        {
            _context = context;
            _recover = recover;
        }

        public IActionResult RecoverTheUser(string id)
        {
            
            _recover.recover(id);
            TempData["User Recovered"] = "User Recover Successfully!!";
            return RedirectToAction("DeleteLogsView", "Log_Information");
        }
    }
}