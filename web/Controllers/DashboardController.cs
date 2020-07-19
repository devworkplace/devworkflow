using web.Models.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Get(string dataInicialFiltro, string dataFinalFiltro, int[] formulariosFiltro)
        {
            IList<object> totLancamentosFormulario = DashboardDAL.GetTotLancamentosFormulario(dataInicialFiltro, dataFinalFiltro, formulariosFiltro);
            IList<object> totLancamentosStatusFormulario = DashboardDAL.GetTotLancamentosStatusFormulario(dataInicialFiltro, dataFinalFiltro, formulariosFiltro);
            IList<object> formulariosTempoMedioAtendimento = DashboardDAL.GetFormulariosTempoMedioAtendimento(dataInicialFiltro, dataFinalFiltro, formulariosFiltro);
            IList<object> formulariosTempoMedioAtendimentoStatus = DashboardDAL.GetFormulariosTempoMedioAtendimentoStatus(dataInicialFiltro, dataFinalFiltro, formulariosFiltro);

            return Json(new
            {
                totLancamentosFormulario = totLancamentosFormulario,
                totLancamentosStatusFormulario = totLancamentosStatusFormulario,
                formulariosTempoMedioAtendimento = formulariosTempoMedioAtendimento,
                formulariosTempoMedioAtendimentoStatus = formulariosTempoMedioAtendimentoStatus
            }, JsonRequestBehavior.AllowGet);
        }
    }
}