Europa.Controllers.ClienteCadastro = {};
Europa.Controllers.Cliente = Europa.Controllers.Cliente || {};
Europa.Controllers.Cliente.UrlListarClientes = undefined;
Europa.Controllers.Cliente.TabelaContatos = undefined;
Europa.Controllers.Cliente.DataNascimentoDatePicker = undefined;

$(function () {
    $('#Uf').select2();
    $('#CpfCliente').mask('999.999.999-99');

    Europa.Controllers.Cliente.InicializarDatePicker();
});

Europa.Controllers.Cliente.InicializarDatePicker = function () {
    Europa.Components.DatePicker.AutoApply();

    Europa.Controllers.Cliente.DataNascimentoDatePicker = new Europa.Components.DatePicker()
        .WithTarget("#DataNascimento")
        .WithFormat("DD/MM/YYYY")
        .Configure();
};

Europa.Controllers.Cliente.CriarTabelaContatos = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.Cliente.TabelaContatos = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Controllers.Cliente.TabelaContatos
        .setIdAreaHeader("datatable-contato-header")
        .setColumns([
            DTColumnBuilder.newColumn('Tipo').withTitle(Europa.i18n.Messages.Tipo).withOption('type', 'enum-format-TipoContato').withOption('width', '31%'),
            DTColumnBuilder.newColumn('Descricao').withTitle(Europa.i18n.Messages.Descricao).withOption('width', '31%'),
            DTColumnBuilder.newColumn('Principal').withTitle(Europa.i18n.Messages.Principal).renderWith(Europa.String.FormatBoolean).withOption('width', '31%'),
        ])
        .setColActions(actionsHtml, '7%')
        .setActionSave(Europa.Controllers.Cliente.Salvar)
        .setOptionsMultiSelect('POST', Europa.Controllers.Cliente.UrlListarContatos, Europa.Controllers.Cliente.Filtro);

    function actionsHtml(data, type, full, meta) {
        var botoes = '<div>';
        if (data.Situacao !== 3) {
            botoes += botoes +
                $scope.renderButton(Europa.i18n.Messages.Editar, "fa fa-edit", "Editar(" + meta.row + ")");
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
        var objetoLinhaTabela = Europa.Controllers.Cliente.TabelaContatos.getRowData(row);

        Europa.Confirmacao.PreAcao(
            Europa.i18n.Messages.Excluir,
            objetoLinhaTabela.Descricao,
            function () {
                Europa.Controllers.Cliente.Excluir(objetoLinhaTabela.Id)
            }
        );
    };

    $scope.Editar = function (row) {
        var objetoLinhaTabela = Europa.Controllers.Cliente.TabelaContatos.getRowData(row);

        $("#modalContato").show();
        $('#TipoContato').val(objetoLinhaTabela.Tipo);
        $('#DescricaoContato').val(objetoLinhaTabela.Descricao);
        $('#PrincipalContato').prop('checked', objetoLinhaTabela.Principal === true || objetoLinhaTabela.Principal === "true");
        $('#idContato').val(objetoLinhaTabela.Id);
    };
}


DataTableApp.controller('Contatos',
    Europa.Controllers.Cliente.CriarTabelaContatos);

Europa.Controllers.Cliente.CriarCliente = function () {
    Europa.Controllers.Cliente.Tabela.createRowNewData();
    Europa.Controllers.Cliente.InicializarDatePicker();
};

Europa.Controllers.Cliente.SalvarCliente = function () {
    Europa.Controllers.Cliente.LimparErro();

    var objCliente = {
        Id: $('#Id').val(),
        Nome: $('#Nome').val(),
        Cpf: $('#CpfCliente').val(),
        DataNascimento: $('#DataNascimento').val(),
        Situacao: 1
    };

    var objEndereco = {
        Cep: $('#CepEndereco').val(),
        Logradouro: $('#Logradouro').val(),
        Numero: $('#NumeroEndereco').val(),
        Complemento: $('#Complemento').val(),
        Bairro: $('#Bairro').val(),
        Cidade: $('#Cidade').val(),
        Uf: $('#Uf').val(),
    }

    Europa.Confirmacao.PreAcao(
        Europa.i18n.Messages.Salvar,
        objCliente.Nome,
        function () {
            $.post(Europa.Controllers.Cliente.UrlSalvarCliente, { cliente: objCliente, endereco: objEndereco }, function (resposta) {
                Europa.Informacao.PosAcao(resposta);

                if (!resposta.Sucesso) {
                    Europa.Controllers.Cliente.AdicionarErro(resposta.Campos);
                }
            });
        }
    );
};

Europa.Controllers.Cliente.SalvarContato = function () {
    Europa.Controllers.Cliente.LimparErro();
    var clienteId = $('#Id').val();

    var objContato = {
        Tipo: $('#TipoContato').val(),
        Descricao: $('#DescricaoContato').val(),
        Principal: $('#PrincipalContato').is(':checked'),
        Id: $('#idContato').val()
    }

    Europa.Confirmacao.PreAcao(
        Europa.i18n.Messages.Salvar,
        objContato.Descricao,
        function () {
            $.post(Europa.Controllers.Cliente.UrlSalvarContato, { contato: objContato, clienteId: clienteId }, function (resposta) {
                Europa.Informacao.PosAcao(resposta);

                if (resposta.Sucesso) {
                    $('#modalContato').hide();
                    Europa.Controllers.Cliente.FecharModalContatos();
                } else {
                    Europa.Controllers.Cliente.AdicionarErro(resposta.Campos);
                }
            });
        }
    )
};

Europa.Controllers.Cliente.Excluir = function (id) {
    $.post(Europa.Controllers.Cliente.UrlExcluirContato, { id: id }, function (resposta) {
        Europa.Informacao.PosAcao(resposta);

        if (resposta.Sucesso) {
            Europa.Controllers.Cliente.TabelaContatos.reloadData();
        }
    });
};

Europa.Controllers.Cliente.FecharModalContatos = function (){
    location.reload();
};

Europa.Controllers.Cliente.Filtro = function () {
    var params = {
        filtro: $('#Id').val() 
    };

    return params;
};

Europa.Controllers.Cliente.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.Cliente.LimparErro = function () {
    $("#form-filtro").find(".has-error").removeClass("has-error");
};
