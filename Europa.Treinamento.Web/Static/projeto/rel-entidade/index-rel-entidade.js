Europa.Controllers.RelatorioEntidade = {};
Europa.Controllers.RelatorioEntidade.UrlListar = undefined;
Europa.Controllers.RelatorioEntidade.Tabela = undefined;

Europa.Controllers.RelatorioEntidade.CriarTabela = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.RelatorioEntidade.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Controllers.RelatorioEntidade.Tabela
        .setIdAreaHeader("datatable-rel-entidade-header")
        .setColumns([
            DTColumnBuilder.newColumn('NomeEntidade').withTitle(Europa.i18n.Messages.Nome).withOption('width', '30%'),
            DTColumnBuilder.newColumn('SobrenomeEntidade').withTitle(Europa.i18n.Messages.Sobrenome).withOption('width', '30%'),
            DTColumnBuilder.newColumn('Idade').withTitle(Europa.i18n.Messages.Idade).withOption('width', '5%'),
            DTColumnBuilder.newColumn('DataNascimento').withTitle(Europa.i18n.Messages.DataNascimento).withOption("type", "date-format-DD/MM/YYYY").withOption('width', '7%'),
            DTColumnBuilder.newColumn('NomeUnidade').withTitle(Europa.i18n.Messages.Unidade).withOption('width', '13%'),
            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('type', 'enum-format-Situacao').withOption('width', '10%')
        ])
        .setDefaultOptions('POST', Europa.Controllers.RelatorioEntidade.UrlListar, Europa.Controllers.RelatorioEntidade.Filtro);
};

DataTableApp.controller('Entidades', Europa.Controllers.RelatorioEntidade.CriarTabela);

Europa.Controllers.RelatorioEntidade.Filtro = function () {
    return {};
};

Europa.Controllers.RelatorioEntidade.ExportarPagina = function () {
    var params = Europa.Controllers.RelatorioEntidade.Filtro();
    params.order = Europa.Controllers.RelatorioEntidade.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.RelatorioEntidade.Tabela.lastRequestParams.draw;
    params.pageSize = Europa.Controllers.RelatorioEntidade.Tabela.lastRequestParams.pageSize;
    params.start = Europa.Controllers.RelatorioEntidade.Tabela.lastRequestParams.start;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.RelatorioEntidade.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.RelatorioEntidade.ExportarTodos = function () {
    var params = Europa.Controllers.RelatorioEntidade.Filtro();
    params.order = Europa.Controllers.RelatorioEntidade.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.RelatorioEntidade.Tabela.lastRequestParams.draw;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.RelatorioEntidade.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};