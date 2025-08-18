Europa.Controllers.Cliente = {};
Europa.Controllers.Cliente.UrlListarClientes = undefined;
Europa.Controllers.Cliente.Tabela = undefined;
Europa.Controllers.Cliente.UrlDetalhesCliente = undefined;

$(function () {
    $("#SituacaoFiltro").select2({
        tags: true,
        placeholder: "Situação",
        multiple: true,
    });

    $("#SituacaoFiltro").val(1).trigger("change");
});

Europa.Controllers.Cliente.CriarTabela = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder){
    Europa.Controllers.Cliente.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Controllers.Cliente.Tabela
        .setIdAreaHeader("datatable-clientes-header")
        .setColumns([
            DTColumnBuilder.newColumn('Nome').withTitle(Europa.i18n.Messages.Nome).withOption('width', '20%'),
            DTColumnBuilder.newColumn('CpfCliente').withTitle(Europa.i18n.Messages.Cpf).withOption('width', '10%'),
            DTColumnBuilder.newColumn('DataNascimento').withTitle(Europa.i18n.Messages.DataNascimento).withOption("type", "date-format-DD/MM/YYYY").withOption('width', '5%'),
            DTColumnBuilder.newColumn('Cidade').withTitle(Europa.i18n.Messages.Cidade).withOption('width', '10%'),    
            DTColumnBuilder.newColumn('Uf').withTitle(Europa.i18n.Messages.Estado).withOption('width', '5%'),
            DTColumnBuilder.newColumn('Telefone').withTitle(Europa.i18n.Messages.Telefone).withOption('width', '10%'),
            DTColumnBuilder.newColumn('Celular').withTitle(Europa.i18n.Messages.Celular).withOption('width', '10%'),
            DTColumnBuilder.newColumn('Email').withTitle(Europa.i18n.Messages.Email).withOption('width', '10%'),
            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao).withOption('type', 'enum-format-Situacao').withOption('width', '10%')
        ])
        .setColActions(actionsHtml, '60px')
        .setOptionsMultiSelect('POST', Europa.Controllers.Cliente.UrlListarClientes, Europa.Controllers.Cliente.Filtro);

    function actionsHtml(data, type, full, meta) {
        var botoes = '<div>';
        if (data.Situacao !== 3) {
            botoes += '<a href="' + Europa.Controllers.Cliente.UrlDetalhesCliente + '?id=' + data.Id + '" class="btn btn-default" title="' + Europa.i18n.Messages.Editar + '"><i class="fa fa-edit"></i></a>';
        }

        botoes += $scope.renderButton(Europa.i18n.Messages.Excluir, "fa fa-trash", "Excluir(" + meta.row + ")") + '</div>';

        return botoes;
    }

    $scope.renderButton = function (title, icon, onClick) {
        icon = $('<i/>').addClass(icon);

        var button = $('<a />')
            .addClass('btn btn-default')
            .attr('title', title)
            .attr('ng-click', onClick)
            .append(icon);

        return button.prop('outerHTML');
    };


    $scope.Excluir = function (row) {
        var objetoLinhaTabela = Europa.Controllers.Cliente.Tabela.getRowData(row);

        Europa.Controllers.Cliente.Excluir(objetoLinhaTabela.Id);
    };

    $scope.Editar = function (row) {
        var objetoLinhaTabela = Europa.Controllers.Cliente.Tabela.getRowData(row);

        $scope.rowEdit(row);

        Europa.Controllers.Cliente.AplicarMascaras();

        $("#DataNascimento").val(Europa.Date.Format(Europa.Date.Parse(objetoLinhaTabela.DataNascimento), 'DD/MM/YYYY'));
    };
}

DataTableApp.controller('Clientes', 
    Europa.Controllers.Cliente.CriarTabela);

Europa.Controllers.Cliente.Filtrar = function () {
    Europa.Controllers.Cliente.Tabela.reloadData();
};

Europa.Controllers.Cliente.LimparFiltro = function () {
    $("#NomeFiltro").val(""),
    $("#EnderecoFiltro").val(""),
    $("#SituacaoFiltro").val(1).trigger("change")
};

Europa.Controllers.Cliente.Filtro = function () {
    var params = {
        Nome: $("#NomeFiltro").val(),
        Endereco: $("#EnderecoFiltro").val(),
        Situacao: $("#SituacaoFiltro").val()
    };

    return params;
};

Europa.Controllers.Cliente.Excluir = function (id) {
    $.post(Europa.Controllers.Cliente.UrlExcluirCliente, { id: id }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);

        if (resposta.Sucesso) {
            Europa.Controllers.Cliente.TabelaContatos.reloadData();
        }     
    });
};

Europa.Controllers.Cliente.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.Cliente.LimparErro = function () {
    $("[name='Nome']").parent().removeClass("has-error");
    $("[name='Endereco']").parent().removeClass("has-error");
    $("[name='Situacao']").parent().removeClass("has-error");
};

Europa.Controllers.Cliente.AplicarMascaras = function () {
    $('#CpfCliente').mask('999.999.999-99');
    $('#Telefone').mask('(99) 9999-9999');
    $('#Celular').mask('(99) 99999-9999');
};

Europa.Controllers.Cliente.ReativarEmLote = function () {
    Europa.Confirmacao.PreAcaoMulti(Europa.i18n.Messages.Reativar, function () {
        Europa.Controllers.Cliente.AlterarSituacaoEmLote(Europa.Controllers.Cliente.UrlReativarClientesEmLote);
    });
};

Europa.Controllers.Cliente.SuspenderEmLote = function () {
    Europa.Confirmacao.PreAcaoMulti(Europa.i18n.Messages.Suspender, function () {
        Europa.Controllers.Cliente.AlterarSituacaoEmLote(Europa.Controllers.Cliente.UrlSuspenderClientesEmLote);
    });
};

Europa.Controllers.Cliente.CancelarEmLote = function () {
    Europa.Confirmacao.PreAcaoMulti(Europa.i18n.Messages.Cancelar, function () {
        Europa.Controllers.Cliente.AlterarSituacaoEmLote(Europa.Controllers.Cliente.UrlCancelarClientesEmLote);
    });
};

Europa.Controllers.Cliente.AlterarSituacaoEmLote = function (novaSituacao) {
    var selecionados = Europa.Controllers.Cliente.Tabela.getRowsSelect().map(x => x.Id);

    if (selecionados.length === 0) return;

    $.post(novaSituacao, { ids: selecionados }, function (resposta) {
        Europa.Controllers.Cliente.Tabela.reloadData();
        Europa.Informacao.PosAcao(resposta);
    });
};

Europa.Controllers.Cliente.ExportarPagina = function () {
    var params = Europa.Controllers.Cliente.Filtro();
    params.order = Europa.Controllers.Cliente.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.Cliente.Tabela.lastRequestParams.draw;
    params.pageSize = Europa.Controllers.Cliente.Tabela.lastRequestParams.pageSize;
    params.start = Europa.Controllers.Cliente.Tabela.lastRequestParams.start;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.Cliente.UrlExportarPagina);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};

Europa.Controllers.Cliente.ExportarTodos = function () {
    var params = Europa.Controllers.Cliente.Filtro();
    params.order = Europa.Controllers.Cliente.Tabela.lastRequestParams.order;
    params.draw = Europa.Controllers.Cliente.Tabela.lastRequestParams.draw;
    var formExportar = $("#Exportar");
    $("#Exportar").find("input").remove();
    formExportar.attr("method", "post").attr("action", Europa.Controllers.Cliente.UrlExportarTodos);
    formExportar.addHiddenInputData(params);
    formExportar.submit();
};