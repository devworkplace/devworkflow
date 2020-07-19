using web.Models.DAL;
using web.Models.TO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class TiposValoresComponentesController : SegurancaController
    {
        // GET: TiposValoresComponentesController
        [HttpPost]
        public JsonResult GetParaComponente()
        {
            IList<TipoValorComponente> dados = TipoValorComponenteDAL.GetParaComponente();

            return Json(new { data = dados }, JsonRequestBehavior.AllowGet);
        }
    }
}