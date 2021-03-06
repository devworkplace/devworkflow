﻿using web.Models.DAL;
using web.Models.TO;
using web.Models.TO.Excel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class PermissaoExibicaoController : SegurancaController
    {
        // GET: PermissaoExibicao
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult Get()
        {
            int draw = Convert.ToInt32(Request.Form["draw"]);
            int start = Convert.ToInt32(Request.Form["start"]);
            int length = Convert.ToInt32(Request.Form["length"]);
            string textoFiltro = Request.Form["search[value]"];
            string sortColumn = Request.Form[string.Format("columns[{0}][name]", Request.Form["order[0][column]"])];
            string sortColumnDir = Request.Form["order[0][dir]"];

            int totRegistros = 0;
            int totRegistrosFiltro = 0;
            IList<PermissaoExibicao> dados = PermissaoExibicaoDAL.Get(start, length, ref totRegistros, textoFiltro, ref totRegistrosFiltro, sortColumn, sortColumnDir);
            if (start > 0 && dados.Count == 0)
            {
                start -= length;
                dados = PermissaoExibicaoDAL.Get(start, length, ref totRegistros, textoFiltro, ref totRegistrosFiltro, sortColumn, sortColumnDir);
                return Json(new { draw = draw, recordsFiltered = totRegistrosFiltro, recordsTotal = totRegistros, data = dados, voltarPagina = 'S' }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { draw = draw, recordsFiltered = totRegistrosFiltro, recordsTotal = totRegistros, data = dados }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetParaExcel()
        {
            return VerificarDadosExcel<PermissaoExibicaoExcel>(PermissaoExibicaoDAL.GetParaExcel(), "as permissões", "Nenhuma permissão foi encontrada");
        }

        [HttpPost]
        public FileResult ExportarExcel(string dados, string nomeTabela)
        {
            DataTable auxDados = JsonConvert.DeserializeObject<DataTable>(dados);
            auxDados.TableName = nomeTabela;
            return DownloadExcel(auxDados, "Permissoes-Exibicoes.xls");
        }

        [HttpPost]
        public JsonResult Insert(String NOME_GRUPO, Int32 ID_FORMULARIO)
        {
            string auxMsgErro = string.Empty;
            string auxMsgSucesso = string.Empty;

            PermissaoExibicao obj = new PermissaoExibicao
            {
                grupo = new Grupo
                {
                    NOME = NOME_GRUPO
                },
                formulario = new Formulario
                {
                    ID = ID_FORMULARIO
                }
            };

            if (PermissaoExibicaoDAL.Insert(obj) == null)
            {
                auxMsgErro = "Falha ao tentar inserir a permissão, favor tente novamente";
            }
            else
            {
                auxMsgSucesso = "Permissão inserida com sucesso";
            }

            return Json(new { msgErro = auxMsgErro, msgSucesso = auxMsgSucesso });
        }

        [HttpPost]
        public JsonResult Delete(String NOME_GRUPO, Int32 ID_FORMULARIO)
        {
            string auxMsgErro = string.Empty;
            string auxMsgSucesso = string.Empty;

            PermissaoExibicao obj = new PermissaoExibicao
            {
                grupo = new Grupo
                {
                    NOME = NOME_GRUPO
                },
                formulario = new Formulario
                {
                    ID = ID_FORMULARIO
                }
            };

            if (PermissaoExibicaoDAL.Delete(obj) == null)
            {
                auxMsgErro = "Falha ao tentar excluir a permissão, favor tente novamente";
            }
            else
            {
                auxMsgSucesso = "Permissão excluída com sucesso";
            }

            return Json(new { msgErro = auxMsgErro, msgSucesso = auxMsgSucesso });
        }
    }
}