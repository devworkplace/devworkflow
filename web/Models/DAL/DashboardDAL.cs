using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace web.Models.DAL
{
    public class DashboardDAL
    {
        public static IList<object> GetTotLancamentosFormulario(string dataInicialFiltro, string dataFinalFiltro, int[] formularios)
        {
            IList<object> objs = new List<object>();

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;

            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;

                string dataInicio = string.Empty;
                string dataFim = string.Empty;
                if (!string.IsNullOrEmpty(dataInicialFiltro))
                {
                    string[] auxDataInicialFiltro = dataInicialFiltro.Split('/');
                    if (auxDataInicialFiltro.Length == 3)
                    {
                        dataInicio = Util.GetDataHoraParaFiltro("00:00:00", auxDataInicialFiltro);
                    }
                }
                if (!string.IsNullOrEmpty(dataFinalFiltro))
                {
                    string[] auxDataFinalFiltro = dataFinalFiltro.Split('/');
                    if (auxDataFinalFiltro.Length == 3)
                    {
                        dataFim = Util.GetDataHoraParaFiltro("23:59:59", auxDataFinalFiltro);
                    }
                }

                StringBuilder filtroFormularios = new StringBuilder();
                if (formularios != null && formularios.Length > 0)
                {
                    Int32 idFormulario = formularios[0];
                    filtroFormularios.Append(idFormulario);
                    for (int i = 1; i < formularios.Length; i++)
                    {
                        idFormulario = formularios[i];
                        filtroFormularios.Append(",").Append(idFormulario);
                    }
                }
                else
                {
                    filtroFormularios.Append("''");
                }

                StringBuilder queryGet = new StringBuilder(@"
                SELECT 
                form.NOME AS 'NOME_FORMULARIO',
                COUNT(pre.ID) AS 'TOTAL_LANCAMENTOS'
                FROM TB_FORMULARIO form
                LEFT JOIN TB_PREENCHIMENTO_FORMULARIO pre ON (pre.ID_FORMULARIO = form.ID AND pre.DATA_HORA BETWEEN @dataInicio AND @dataFim)
                WHERE form.ID IN (").Append(filtroFormularios.ToString()).Append(@")
                GROUP BY form.NOME
                ORDER BY form.NOME");

                comm.CommandText = queryGet.ToString();

                comm.Parameters.Add(new SqlParameter("dataInicio", dataInicio));
                comm.Parameters.Add(new SqlParameter("dataFim", dataFim));

                con.Open();

                SqlDataReader rd = comm.ExecuteReader();

                object obj;

                while (rd.Read())
                {
                    obj = new
                    {
                        NOME_FORMULARIO = rd.GetString(0),
                        TOTAL_LANCAMENTOS = rd.GetInt32(1)
                    };
                    objs.Add(obj);
                }
                rd.Close();
            }
            catch (Exception ex)
            {
                objs.Clear();
            }
            finally
            {
                con.Close();
            }

            return objs;
        }

        public static IList<object> GetTotLancamentosStatusFormulario(string dataInicialFiltro, string dataFinalFiltro, int[] formularios)
        {
            IList<object> objs = new List<object>();

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;

            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;

                string dataInicio = string.Empty;
                string dataFim = string.Empty;
                if (!string.IsNullOrEmpty(dataInicialFiltro))
                {
                    string[] auxDataInicialFiltro = dataInicialFiltro.Split('/');
                    if (auxDataInicialFiltro.Length == 3)
                    {
                        dataInicio = Util.GetDataHoraParaFiltro("00:00:00", auxDataInicialFiltro);
                    }
                }
                if (!string.IsNullOrEmpty(dataFinalFiltro))
                {
                    string[] auxDataFinalFiltro = dataFinalFiltro.Split('/');
                    if (auxDataFinalFiltro.Length == 3)
                    {
                        dataFim = Util.GetDataHoraParaFiltro("23:59:59", auxDataFinalFiltro);
                    }
                }

                StringBuilder filtroFormularios = new StringBuilder();
                if (formularios != null && formularios.Length > 0)
                {
                    Int32 idFormulario = formularios[0];
                    filtroFormularios.Append(idFormulario);
                    for (int i = 1; i < formularios.Length; i++)
                    {
                        idFormulario = formularios[i];
                        filtroFormularios.Append(",").Append(idFormulario);
                    }
                }
                else
                {
                    filtroFormularios.Append("''");
                }

                StringBuilder queryGet = new StringBuilder(@"
                SELECT 
                form.NOME AS 'NOME_FORMULARIO',
                statusForm.NOME AS 'NOME_STATUS',
                COUNT(tra.ID) AS 'TOTAL_LANCAMENTOS'
                FROM TB_FORMULARIO form
                JOIN TB_STATUS_FORMULARIO statusForm ON statusForm.ID_FORMULARIO = form.ID
                LEFT JOIN TB_TRAMITACAO tra ON tra.ID_STATUS_DESTINO = statusForm.ID AND tra.ID IN 
	            (
		            SELECT MAX(maxTra.ID)
			            FROM TB_TRAMITACAO maxTra
                        JOIN TB_PREENCHIMENTO_FORMULARIO pre ON pre.ID = maxTra.ID_PREENCHIMENTO_FORMULARIO
                            WHERE pre.DATA_HORA BETWEEN @dataInicio AND @dataFim 
				                GROUP BY maxTra.ID_PREENCHIMENTO_FORMULARIO
	            )
                WHERE form.ID IN (").Append(filtroFormularios.ToString()).Append(@")
                GROUP BY form.NOME, statusForm.NOME
                ORDER BY form.NOME, statusForm.NOME");

                comm.CommandText = queryGet.ToString();

                comm.Parameters.Add(new SqlParameter("dataInicio", dataInicio));
                comm.Parameters.Add(new SqlParameter("dataFim", dataFim));

                con.Open();

                SqlDataReader rd = comm.ExecuteReader();

                object obj;

                while (rd.Read())
                {
                    obj = new
                    {
                        NOME_FORMULARIO = rd.GetString(0),
                        NOME_STATUS = rd.GetString(1),
                        TOTAL_LANCAMENTOS = rd.GetInt32(2)
                    };
                    objs.Add(obj);
                }
                rd.Close();
            }
            catch (Exception ex)
            {
                objs.Clear();
            }
            finally
            {
                con.Close();
            }

            return objs;
        }

        public static IList<object> GetFormulariosTempoMedioAtendimento(string dataInicialFiltro, string dataFinalFiltro, int[] formularios)
        {
            IList<object> objs = new List<object>();

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;

            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;

                string dataInicio = string.Empty;
                string dataFim = string.Empty;
                if (!string.IsNullOrEmpty(dataInicialFiltro))
                {
                    string[] auxDataInicialFiltro = dataInicialFiltro.Split('/');
                    if (auxDataInicialFiltro.Length == 3)
                    {
                        dataInicio = Util.GetDataHoraParaFiltro("00:00:00", auxDataInicialFiltro);
                    }
                }
                if (!string.IsNullOrEmpty(dataFinalFiltro))
                {
                    string[] auxDataFinalFiltro = dataFinalFiltro.Split('/');
                    if (auxDataFinalFiltro.Length == 3)
                    {
                        dataFim = Util.GetDataHoraParaFiltro("23:59:59", auxDataFinalFiltro);
                    }
                }

                StringBuilder filtroFormularios = new StringBuilder();
                if (formularios != null && formularios.Length > 0)
                {
                    Int32 idFormulario = formularios[0];
                    filtroFormularios.Append(idFormulario);
                    for (int i = 1; i < formularios.Length; i++)
                    {
                        idFormulario = formularios[i];
                        filtroFormularios.Append(",").Append(idFormulario);
                    }
                }
                else
                {
                    filtroFormularios.Append("''");
                }

                StringBuilder queryGet = new StringBuilder(@"
                SELECT 
                NOME_FORMULARIO = formulario.NOME,
                MEDIA_ATENDIMENTO = ISNULL(AVG(TEMPO_ATENDIMENTO.TEMPO_ATENDIMENTO), 0)
                FROM 
                (
	                SELECT
	                preenchimento.ID_FORMULARIO,
	                TEMPO_ATENDIMENTO = DATEDIFF(MINUTE, MIN(tramitacao.DATA_HORA), MAX(tramitacao.DATA_HORA))
	                FROM 
	                (
		                SELECT
		                tramitacaoInicial.ID_PREENCHIMENTO_FORMULARIO,
		                tramitacaoInicial.DATA_HORA
		                FROM TB_TRAMITACAO tramitacaoInicial
		                JOIN TB_STATUS_FORMULARIO statusInicial 
		                ON statusInicial.INICIAL = 1 
		                AND statusInicial.ID = tramitacaoInicial.ID_STATUS_DESTINO 
		                AND tramitacaoInicial.ID_STATUS_ORIGEM IS NULL
		                UNION ALL
		                SELECT
		                tramitacaoFinal.ID_PREENCHIMENTO_FORMULARIO,
		                tramitacaoFinal.DATA_HORA
		                FROM TB_TRAMITACAO tramitacaoFinal
		                JOIN TB_STATUS_FORMULARIO statusFinal ON statusFinal.ID = tramitacaoFinal.ID_STATUS_DESTINO
		                WHERE NOT EXISTS (SELECT 1 FROM TB_FLUXO_STATUS WHERE ID_STATUS_ORIGEM = statusFinal.ID)
		                AND statusFinal.INICIAL = 0 
		                AND statusFinal.RETORNO = 0
	                ) tramitacao
	                JOIN TB_PREENCHIMENTO_FORMULARIO preenchimento ON (preenchimento.ID = tramitacao.ID_PREENCHIMENTO_FORMULARIO AND preenchimento.DATA_HORA BETWEEN @dataInicio AND @dataFim)
	                GROUP BY preenchimento.ID_FORMULARIO, preenchimento.ID
	                HAVING COUNT(preenchimento.ID) = 2
                ) TEMPO_ATENDIMENTO
                RIGHT JOIN TB_FORMULARIO formulario ON formulario.ID = TEMPO_ATENDIMENTO.ID_FORMULARIO
                WHERE formulario.ID IN (").Append(filtroFormularios.ToString()).Append(@")
                GROUP BY formulario.NOME
                ORDER BY formulario.NOME");

                comm.CommandText = queryGet.ToString();

                comm.Parameters.Add(new SqlParameter("dataInicio", dataInicio));
                comm.Parameters.Add(new SqlParameter("dataFim", dataFim));

                con.Open();

                SqlDataReader rd = comm.ExecuteReader();

                object obj;

                while (rd.Read())
                {
                    obj = new
                    {
                        NOME_FORMULARIO = rd.GetString(0),
                        MEDIA_ATENDIMENTO = rd.GetInt32(1)
                    };
                    objs.Add(obj);
                }
                rd.Close();
            }
            catch (Exception ex)
            {
                objs.Clear();
            }
            finally
            {
                con.Close();
            }

            return objs;
        }

        public static IList<object> GetFormulariosTempoMedioAtendimentoStatus(string dataInicialFiltro, string dataFinalFiltro, int[] formularios)
        {
            IList<object> objs = new List<object>();

            SqlConnection con = new SqlConnection();
            con.ConnectionString = Util.CONNECTION_STRING;

            try
            {
                SqlCommand comm = new SqlCommand();
                comm.Connection = con;

                string dataInicio = string.Empty;
                string dataFim = string.Empty;
                if (!string.IsNullOrEmpty(dataInicialFiltro))
                {
                    string[] auxDataInicialFiltro = dataInicialFiltro.Split('/');
                    if (auxDataInicialFiltro.Length == 3)
                    {
                        dataInicio = Util.GetDataHoraParaFiltro("00:00:00", auxDataInicialFiltro);
                    }
                }
                if (!string.IsNullOrEmpty(dataFinalFiltro))
                {
                    string[] auxDataFinalFiltro = dataFinalFiltro.Split('/');
                    if (auxDataFinalFiltro.Length == 3)
                    {
                        dataFim = Util.GetDataHoraParaFiltro("23:59:59", auxDataFinalFiltro);
                    }
                }

                StringBuilder filtroFormularios = new StringBuilder();
                if (formularios != null && formularios.Length > 0)
                {
                    Int32 idFormulario = formularios[0];
                    filtroFormularios.Append(idFormulario);
                    for (int i = 1; i < formularios.Length; i++)
                    {
                        idFormulario = formularios[i];
                        filtroFormularios.Append(",").Append(idFormulario);
                    }
                }
                else
                {
                    filtroFormularios.Append("''");
                }

                StringBuilder queryGet = new StringBuilder(@"
                SELECT
                NOME_FORMULARIO = formulario.NOME,
                NOME_STATUS_ORIGEM = statusOrigem.NOME,
                NOME_STATUS_DESTINO = statusDestino.NOME,
                MEDIA_ATENDIMENTO = AVG(DATEDIFF(MINUTE, tramitacaoOrigem.DATA_HORA, tramitacaoDestino.DATA_HORA))
                FROM (SELECT
                ID_TRAMITACAO_ORIGEM = tramitacaoOrigem.ID,
                ID_TRAMITACAO_DESTINO = (
	                SELECT TOP 1 ID 
	                FROM TB_TRAMITACAO
	                WHERE ID_PREENCHIMENTO_FORMULARIO = tramitacaoOrigem.ID_PREENCHIMENTO_FORMULARIO
	                AND ID_STATUS_ORIGEM = tramitacaoOrigem.ID_STATUS_DESTINO
	                AND DATA_HORA > tramitacaoOrigem.DATA_HORA
	                ORDER BY DATA_HORA
                ),
                tramitacaoOrigem.ID_STATUS_DESTINO,
                tramitacaoOrigem.DATA_HORA
                FROM TB_TRAMITACAO tramitacaoOrigem) tramitacaoOrigem
                JOIN TB_TRAMITACAO tramitacaoDestino ON tramitacaoDestino.ID = tramitacaoOrigem.ID_TRAMITACAO_DESTINO
                JOIN TB_PREENCHIMENTO_FORMULARIO pre ON pre.ID = tramitacaoDestino.ID_PREENCHIMENTO_FORMULARIO
                JOIN TB_STATUS_FORMULARIO statusOrigem ON statusOrigem.ID = tramitacaoOrigem.ID_STATUS_DESTINO
                JOIN TB_STATUS_FORMULARIO statusDestino ON statusDestino.ID = tramitacaoDestino.ID_STATUS_DESTINO
                JOIN TB_FORMULARIO formulario ON formulario.ID = statusDestino.ID_FORMULARIO
                WHERE formulario.ID IN (").Append(filtroFormularios.ToString()).Append(@")
                AND pre.DATA_HORA BETWEEN @dataInicio AND @dataFim 
                GROUP BY formulario.NOME, statusOrigem.NOME, statusDestino.NOME
                ORDER BY formulario.NOME, statusOrigem.NOME, statusDestino.NOME");

                comm.CommandText = queryGet.ToString();

                comm.Parameters.Add(new SqlParameter("dataInicio", dataInicio));
                comm.Parameters.Add(new SqlParameter("dataFim", dataFim));

                con.Open();

                SqlDataReader rd = comm.ExecuteReader();

                object obj;

                while (rd.Read())
                {
                    obj = new
                    {
                        NOME_FORMULARIO = rd.GetString(0),
                        NOME_STATUS_ORIGEM = rd.GetString(1),
                        NOME_STATUS_DESTINO = rd.GetString(2),
                        MEDIA_ATENDIMENTO = rd.GetInt32(3)
                    };
                    objs.Add(obj);
                }
                rd.Close();
            }
            catch (Exception ex)
            {
                objs.Clear();
            }
            finally
            {
                con.Close();
            }

            return objs;
        }
    }
}