using web.Models.DAL;
using web.Models.TO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class MascaraComponenteController : SegurancaController
    {
        // GET: MascaraComponente
        [HttpPost]
        public JsonResult GetParaComponente()
        {
            IList<MascaraComponente> dados = MascaraComponenteDAL.GetParaComponente();

            return Json(new { data = dados }, JsonRequestBehavior.AllowGet);
        }
    }
}