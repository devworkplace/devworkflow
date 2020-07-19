var dvFormulariosFiltro;
var selFormulariosDualList;
var colunasTabelaPrincipal = [
    {
        render: renderColunaOpcoes
    }
];
metodoGet = 'Dashboard/Get';

function inicializarCampoData(id) {
    $('#' + id).datepicker({
        format: 'dd/mm/yyyy',
        todayBtn: 'linked',
        todayHighlight: true,
        autoclose: true,
        orientation: 'top right',
        language: 'pt-BR'
    })
        .on('changeDate', function (e) {
            ajustarPeriodoFiltro($(this), $('#dataInicialFiltro'), $('#dataFinalFiltro'), tabelaPrincipal, diferenciaDiasFiltro);
        });
}

function inicializarFiltro() {
    var dataInicialFiltro = new Date();
    dataInicialFiltro.setDate(dataInicialFiltro.getDate() - diferenciaDiasFiltro);
    var dataFinalFiltro = new Date();

    inicializarCampoData('dataInicialFiltro');
    $('#dataInicialFiltro').datepicker('setDate', dataInicialFiltro);
    $('#dataInicialFiltro').datepicker('update');
    $('#dataInicialFiltro').data('fezCargaInicial', 'S');

    inicializarCampoData('dataFinalFiltro');
    $('#dataFinalFiltro').datepicker('setDate', dataFinalFiltro);
    $('#dataFinalFiltro').datepicker('update');
    $('#dataFinalFiltro').data('fezCargaInicial', 'S');

    $('#idBuscaPreenchimento').val('');
}

function possuiFormularios() {
    return null != $('#selFormularios').val() && undefined != $('#selFormularios').val();
}

function atualizarListaFormulariosEscolhidos() {
    $('#listaFormularios').html('');
    if (possuiFormularios()) {
        var itensSelect = $($('#selFormularios')[0]).find('option:selected');
        if (undefined != itensSelect && itensSelect.length > 0) {
            $('#listaFormularios').html($(itensSelect[0]).html());
            for (var i = 1; i < itensSelect.length; i++) {
                $('#listaFormularios').html($('#listaFormularios').html() + ', ' + $(itensSelect[i]).html());
            }
        } else {
            $('#listaFormularios').html('Nenhum Formulário foi Informado');
        }
    } else {
        $('#listaFormularios').html('Nenhum Formulário foi Informado');
    }
}

function inicializarModalFormularios() {
    dvFormulariosFiltro = $('#dvFormulariosFiltro').dialog({
        resizable: false,
        height: 'auto',
        width: '80%',
        modal: true,
        autoOpen: false,
        draggable: false,
        show: {
            effect: "blind",
            duration: 400
        },
        hide: {
            effect: "blind",
            duration: 400
        },
        open: function (event, ui) {
            $(this).parent().find('.ui-dialog-titlebar-close').hide();
        },
        buttons: [
            {
                text: 'Confirmar',
                'class': 'btn btn-sm btn-success',
                'style': 'font-weight: bold; font-size: 12px; margin-right: 10px;',
                click: function () {
                    mostrarPopup();
                    atualizarListaFormulariosEscolhidos();
                    atualizarSelectFormularios();
                    fecharPopup();
                    $(this).dialog("close");
                }
            },
            {
                text: 'Cancelar',
                'class': 'btn btn-sm btn-danger',
                'style': 'font-weight: bold; font-size: 12px;',
                click: function () {
                    $(this).dialog("close");
                    reverterSelectFormularios();
                }
            }
        ],
    });
}

function desenharTabelaPrincipal() {
    var tabelaPrincipal = $('<table class="table table-striped table-bordered table-hover" id="tabelaPrincipal"></table>');
    var theadTabelaPrincipal = $('<thead></thead>');
    var linhaColunasDinamicas = $('<tr></tr>');
    var nomeColuna;
    var indiceLinhaTabela = 0;
    var indiceListaNomeColuna = 0;
    var listaNomeColuna = []
    linhaColunasDinamicas.append('<th style="text-align: center;">Ações</th>');
    theadTabelaPrincipal.append(linhaColunasDinamicas);
    tabelaPrincipal.append(theadTabelaPrincipal);
    //$('#dvPaiTabela').append(tabelaPrincipal);

    colunasTabelaPrincipal = [];
    colunasTabelaPrincipal[colunasTabelaPrincipal.length] = {
        render: renderColunaOpcoes,
        mRender: renderColunaOpcoes
    }

    guardarDadosFiltro();
    inicializarTabelaPrincipalColunasDinamicas($('#tabelaPrincipal'), colunasTabelaPrincipal, '', listaNomeColuna, $('#dataInicialFiltro').val(), $('#dataFinalFiltro').val());
    mostrarDvTabelaPrincipal($('#formCadastro'), $('#dvFormCadastro'), $('#dvTabelaPrincipal'));
}

function inicializarSelectFormularios() {
    selFormulariosDualList = $('#selFormularios').bootstrapDualListbox({
        nonSelectedListLabel: '<b>Dispon&iacute;veis</b>',
        selectedListLabel: '<b>Escolhidos</b>'
    });
}

function atualizarSelectFormularios() {
    var auxItensSelect = $($('#selFormularios')[0]).find('option');
    var itensSelect = [];
    var indice = 0;
    var valueOption;
    var textOption;
    var optionSelected;
    for (var i = 0; i < auxItensSelect.length; i++) {
        valueOption = $(auxItensSelect[i]).val();
        textOption = $(auxItensSelect[i]).html();
        optionSelected = $(auxItensSelect[i]).prop('selected');
        if (optionSelected == true) {
            itensSelect[indice++] = $('<option selected value="' + valueOption + '" >' + textOption + '</option>');
        } else {
            itensSelect[indice++] = $('<option value="' + valueOption + '" >' + textOption + '</option>');
        }
    }
    $('#selFormularios').data('totItensEscolhidos', $($('#selFormularios')[0]).find('option:selected').length);
    $('#selFormularios').data('itensSelect', itensSelect);
}

function reverterSelectFormularios() {
    if ('none' == $('#dvFormulariosFiltro').parent().css('display')) {
        var itensSelect = $('#selFormularios').data('itensSelect');
        if (undefined != itensSelect) {
            $('#selFormularios').empty();
            for (var i = 0; i < itensSelect.length; i++) {
                $('#selFormularios').append($(itensSelect[i]));
            }
            selFormulariosDualList.bootstrapDualListbox('refresh');
        }
        atualizarSelectFormularios();
    } else {
        setTimeout(function () {
            reverterSelectFormularios();
        }, 200);
    }
}

function obterFormularios() {
    mostrarPopup();
    $.ajax({
        url: 'Formularios/GetParaHome',
        type: 'POST',
        data: { somenteAtivos: 'S', filtrarStatus: 'S' },
        success: function (result) {
            var dados = result.data;

            if (undefined != dados && dados.length > 0) {
                if (undefined == selFormulariosDualList) {
                    inicializarSelectFormularios();
                }

                $('#selFormularios').empty();

                var auxOption;
                for (var i = 0; i < dados.length; i++) {
                    auxOption = $('<option value="' + dados[i].ID + '">' + dados[i].NOME + '</option>');
                    $('#selFormularios').append(auxOption);
                }

                atualizarSelectFormularios();
                selFormulariosDualList.bootstrapDualListbox('refresh');
                mostrarFormulariosFiltro();
                guardarFormulariosFiltro();
            } else {
                mostrarMsgErro('Nenhum formulário foi encontrado');
            }

            fecharPopup();
        },
        error: function (request, status, error) {
            mostrarMsgErro('Falha ao tentar obter os formulários, favor tente novamente');
            fecharPopup();
        }
    });
}

function mostrarFormulariosFiltro() {
    $('#selFormularios').parent().find('input.filter').each(function () {
        $(this).val('');
        $(this).change();
    });
    dvFormulariosFiltro.dialog('open');
    $('#dvFormulariosFiltro').css('height', 'auto');
}

function obterFormulariosFiltro(formulariosFiltro) {
    if (undefined != selFormulariosDualList && undefined != formulariosFiltro && formulariosFiltro.length > 0) {
        $('#selFormularios').empty();
        var idFormulario;
        var nomeFormulario;
        var selecionado;
        for (var i = 0; i < formulariosFiltro.length; i++) {
            idFormulario = formulariosFiltro[i].ID;
            nomeFormulario = formulariosFiltro[i].NOME;
            selecionado = formulariosFiltro[i].SELECTED;
            if ('S' == selecionado) {
                $('#selFormularios').append($('<option selected value="' + idFormulario + '">' + nomeFormulario + '</option>'));
            } else {
                $('#selFormularios').append($('<option value="' + idFormulario + '">' + nomeFormulario + '</option>'));
            }
        }
        atualizarListaFormulariosEscolhidos();
        atualizarSelectFormularios();
        selFormulariosDualList.bootstrapDualListbox('refresh');
    }
}

function guardarFormulariosFiltro() {
    var formulariosFiltro = [];
    var indiceFormulariosFiltro = 0;
    var itensSelect = $('#selFormularios').data('itensSelect');
    if (undefined != itensSelect) {
        for (var i = 0; i < itensSelect.length; i++) {
            formulariosFiltro[indiceFormulariosFiltro++] = {
                ID: $(itensSelect[i]).val(),
                NOME: $(itensSelect[i]).html(),
                SELECTED: undefined != $(itensSelect[i]).attr('selected') ? 'S' : 'N'
            };
        }
    }
    tabelaPrincipal.formulariosFiltro = formulariosFiltro;
}

var listasCoresGeradas = [];

function gerarCorAleatoria() {
    var limiteSuperior = 256;
    var limiteInferior = 127;
    var valorRed = (Math.random() * (limiteSuperior - limiteInferior) + limiteInferior).toFixed(0);
    var valorGreen = (Math.random() * (limiteSuperior - limiteInferior) + limiteInferior).toFixed(0);
    var valorBlue = (Math.random() * (limiteSuperior - limiteInferior) + limiteInferior).toFixed(0);
    var result = 'rgb(' + valorRed + ', ' + valorGreen + ', ' + valorBlue + ')';
    if (isNullOrEmpty(listasCoresGeradas[result])) {
        listasCoresGeradas[result] = 1;
        return result;
    }
    return gerarCorAleatoria();
}

function desenharGraficoBarras(idChart, dataChart, titleChart) {
    var canvas = document.getElementById(idChart).getContext('2d');
    return new Chart(canvas, {
        type: 'bar',
        data: dataChart,
        options: {
            title: {
                display: true,
                text: titleChart
            },
            tooltips: {
                mode: 'index',
                intersect: false
            },
            responsive: true,
            scales: {
                xAxes: [{
                    stacked: true,
                }],
                yAxes: [{
                    stacked: true
                }]
            }
        }
    });
}

var chartTotLancamentos;
var chartTotLancamentosStatus;
var chartMediaAtendimento;
var chartMediaAtendimentoStatus;

var tituloTotLancamentos = 'Total de Lançamentos';
var tituloTotLancamentosStatus = 'Total de Lançamentos por Status';
var tituloMediaAtendimento = 'Tempo Médio de Atendimento';
var tituloMediaAtendimentoStatus = 'Tempo Médio de Atendimento por Status';

var dadosTotLancamentos = {
    labels: undefined,
    datasets: [{
        label: tituloTotLancamentos,
        data: []
    }]
};

var dadosTotLancamentosStatus = {
    labels: undefined,
    datasets: undefined
};

var dadosMediaAtendimento = {
    labels: undefined,
    datasets: [{
        label: tituloMediaAtendimento,
        data: []
    }]
};
var dadosMediaAtendimentoStatus = {
    labels: undefined,
    datasets: undefined
};

function atualizarGraficoTotLancamentos(dadosGrafico) {
    dadosTotLancamentos.datasets.forEach(function (dataset) {
        dadosTotLancamentos.labels = [];
        for (var i = 0; i < dadosGrafico.length; i++) {
            dadosTotLancamentos.labels[i] = dadosGrafico[i].NOME_FORMULARIO;
            dataset.data[i] = dadosGrafico[i].TOTAL_LANCAMENTOS;
        }
        dataset.backgroundColor = gerarCorAleatoria();
    });
    chartTotLancamentos.update();
}

function atualizarGraficoTotLancamentosStatus(dadosGrafico) {
    dadosTotLancamentosStatus.labels = [];

    var indiceFormulario = 0;
    var indiceStatus = 0;
    var listaIndicesFormularios = [];
    var listaIndicesStatus = [];
    var listaStatus = [];

    for (var i = 0; i < dadosGrafico.length; i++) {
        if (isNullOrEmpty(listaIndicesFormularios[dadosGrafico[i].NOME_FORMULARIO])) {
            dadosTotLancamentosStatus.labels[indiceFormulario] = dadosGrafico[i].NOME_FORMULARIO;
            listaIndicesFormularios[dadosGrafico[i].NOME_FORMULARIO] = indiceFormulario++;
        }
    }

    for (var i = 0; i < dadosGrafico.length; i++) {
        if (isNullOrEmpty(listaIndicesStatus[dadosGrafico[i].NOME_STATUS])) {
            listaIndicesStatus[dadosGrafico[i].NOME_STATUS] = indiceStatus;
            listaStatus[indiceStatus] = {
                label: dadosGrafico[i].NOME_STATUS,
                backgroundColor: gerarCorAleatoria(),
                data: Array(indiceFormulario).fill(0)
            };
            indiceStatus++;
        }
        listaStatus[listaIndicesStatus[dadosGrafico[i].NOME_STATUS]].data[listaIndicesFormularios[dadosGrafico[i].NOME_FORMULARIO]] = dadosGrafico[i].TOTAL_LANCAMENTOS;
    }

    dadosTotLancamentosStatus.datasets = listaStatus;
    chartTotLancamentosStatus.update();
}

function atualizarGraficoMediaAtendimento(dadosGrafico) {
    dadosMediaAtendimento.datasets.forEach(function (dataset) {
        dadosMediaAtendimento.labels = [];
        for (var i = 0; i < dadosGrafico.length; i++) {
            dadosMediaAtendimento.labels[i] = dadosGrafico[i].NOME_FORMULARIO;
            dataset.data[i] = dadosGrafico[i].MEDIA_ATENDIMENTO;
        }
        dataset.backgroundColor = gerarCorAleatoria();
    });
    chartMediaAtendimento.update();
}

function atualizarGraficoMediaAtendimentoStatus(dadosGrafico) {
    dadosMediaAtendimentoStatus.labels = [];

    var indiceFormulario = 0;
    var indiceStatus = 0;
    var listaIndicesFormularios = [];
    var listaIndicesStatus = [];
    var listaStatus = [];

    for (var i = 0; i < dadosGrafico.length; i++) {
        if (isNullOrEmpty(listaIndicesFormularios[dadosGrafico[i].NOME_FORMULARIO])) {
            dadosMediaAtendimentoStatus.labels[indiceFormulario] = dadosGrafico[i].NOME_FORMULARIO;
            listaIndicesFormularios[dadosGrafico[i].NOME_FORMULARIO] = indiceFormulario++;
        }
    }

    for (var i = 0; i < dadosGrafico.length; i++) {
        if (isNullOrEmpty(listaIndicesStatus[dadosGrafico[i].NOME_STATUS_ORIGEM + ' - ' + dadosGrafico[i].NOME_STATUS_DESTINO])) {
            listaIndicesStatus[dadosGrafico[i].NOME_STATUS_ORIGEM + ' - ' + dadosGrafico[i].NOME_STATUS_DESTINO] = indiceStatus;
            listaStatus[indiceStatus] = {
                label: dadosGrafico[i].NOME_STATUS_ORIGEM + ' - ' + dadosGrafico[i].NOME_STATUS_DESTINO,
                backgroundColor: gerarCorAleatoria(),
                data: Array(indiceFormulario).fill(0)
            };
            indiceStatus++;
        }
        listaStatus[listaIndicesStatus[dadosGrafico[i].NOME_STATUS_ORIGEM + ' - ' + dadosGrafico[i].NOME_STATUS_DESTINO]].data[listaIndicesFormularios[dadosGrafico[i].NOME_FORMULARIO]] = dadosGrafico[i].MEDIA_ATENDIMENTO;
    }

    dadosMediaAtendimentoStatus.datasets = listaStatus;
    chartMediaAtendimentoStatus.update();
}

function obterDadosDashboards(listaIdsFormulariosFiltro) {
    mostrarPopup();
    $.ajax({
        url: metodoGet,
        type: 'POST',
        data: { dataInicialFiltro: tabelaPrincipal.dataInicialFiltro, dataFinalFiltro: tabelaPrincipal.dataFinalFiltro, formulariosFiltro: listaIdsFormulariosFiltro },
        success: function (result) {
            atualizarGraficoTotLancamentos(result.totLancamentosFormulario);
            atualizarGraficoTotLancamentosStatus(result.totLancamentosStatusFormulario);
            atualizarGraficoMediaAtendimento(result.formulariosTempoMedioAtendimento);
            atualizarGraficoMediaAtendimentoStatus(result.formulariosTempoMedioAtendimentoStatus);
            fecharPopup();
        },
        error: function (request, status, error) {
            mostrarMsgErro('Falha ao tentar obter os dados, favor tente novamente');
            fecharPopup();
        }
    });
}

$(document).ready(function () {
    diferenciaDiasFiltro = 180;

    inicializarFiltro();
    inicializarModalFormularios();
    desenharTabelaPrincipal();

    chartTotLancamentos = desenharGraficoBarras('chartTotLancamentos', dadosTotLancamentos, tituloTotLancamentos);
    chartTotLancamentosStatus = desenharGraficoBarras('chartTotLancamentosStatus', dadosTotLancamentosStatus, tituloTotLancamentosStatus);
    chartMediaAtendimento = desenharGraficoBarras('chartMediaAtendimento', dadosMediaAtendimento, tituloMediaAtendimento);
    chartMediaAtendimentoStatus = desenharGraficoBarras('chartMediaAtendimentoStatus', dadosMediaAtendimentoStatus, tituloMediaAtendimentoStatus);

    $('#btnEscolherForms').click(function () {
        if (undefined == selFormulariosDualList) {
            obterFormularios();
        } else {
            mostrarFormulariosFiltro();
        }
    });

    $('#btnFiltrar').click(function () {
        if (undefined != tabelaPrincipal &&
            ajustarPeriodoFiltro($('#dataInicialFiltro'),
                $('#dataInicialFiltro'),
                $('#dataFinalFiltro'),
                tabelaPrincipal,
                diferenciaDiasFiltro)) {
            guardarDadosFiltro();
            var listaIdsFormulariosFiltro = obterIdsFormulariosFiltro(tabelaPrincipal.formulariosFiltro);
            if (listaIdsFormulariosFiltro.length == 0) {
                mostrarMsgErro("Favor informar pelo menos 1 formulário");
            } else {
                $('#dvPaiTabela').css('display', '');
                obterDadosDashboards(listaIdsFormulariosFiltro);
            }
        }
    });

    $('#btnLimparFiltro').click(function () {
        limparFiltro();
    });
});