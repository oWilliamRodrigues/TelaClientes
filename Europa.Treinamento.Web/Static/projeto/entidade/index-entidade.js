/*
 * Define namespace, objetos e variáveis globais
 */
Europa.Controllers.Entidade = {};
Europa.Controllers.Entidade.UrlListarEntidades = undefined;
Europa.Controllers.Entidade.Tabela = undefined;
Europa.Controllers.Entidade.DataNascimentoDeDatePicker = undefined;
Europa.Controllers.Entidade.DataNascimentoAteDatePicker = undefined;
Europa.Controllers.Entidade.AutoCompleteUnidade = undefined;
Europa.Controllers.Entidade.AutoCompleteUnidadeFiltro = undefined;

$(function () {
    $("#SituacaoFiltro").select2({
        trags: true,
        placeholder: "Situação"
    });
 
    $("#SituacaoFiltro").val(1).trigger("change");
    Europa.Controllers.Entidade.InicializarDatePicker();
    Europa.Controllers.Entidade.InicializarAutocompletes();
});

Europa.Controllers.Entidade.InicializarDatePicker = function () {
    Europa.Components.DatePicker.AutoApply();

    Europa.Controllers.Entidade.DataNascimentoDeDatePicker = new Europa.Components.DatePicker()
        .WithTarget("#DataNascimentoDe")
        .WithFormat("DD/MM/YYYY")
        .Configure();

    Europa.Controllers.Entidade.DataNascimentoAteDatePicker = new Europa.Components.DatePicker()
        .WithTarget("#DataNascimentoAte")
        .WithFormat("DD/MM/YYYY")
        .Configure();

    Europa.Controllers.Entidade.DataNascimentoDatePicker = new Europa.Components.DatePicker()
        .WithTarget("#DataNascimento")
        .WithFormat("DD/MM/YYYY")
        .Configure();
};

Europa.Controllers.Entidade.InicializarAutocompletes = function () {
    Europa.Controllers.Entidade.AutoCompleteUnidadeFiltro = new Europa.Components.AutoCompleteUnidade()
        .WithTargetSuffix("unidade_filtro")
        .Configure();

    Europa.Controllers.Entidade.AutoCompleteUnidade = new Europa.Components.AutoCompleteUnidade()
        .WithTargetSuffix("unidade")
        .Configure();
};

/*
 * Define função que instancia um novo datatable
 * A referência do Datatable é guardada em Europa.Controllers.Entidade.Tabela
 */
Europa.Controllers.Entidade.CriarTabela = function ($scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder) {
    Europa.Controllers.Entidade.Tabela = new DataTableWrapper(this, $scope, $compile, DTOptionsBuilder, DTColumnBuilder, DTColumnDefBuilder);
    Europa.Controllers.Entidade.Tabela
        // Define o id da div do cabeçalho do Datatable
        .setIdAreaHeader("datatable-entidades-header")
        // Define o template de edição dos campos
        .setTemplateEdit([
            '<input type="text" class="form-control" name="Nome" id="Nome" maxlength="254">',
            '<input type="text" class="form-control" name="Sobrenome" id="Sobrenome" maxlength="254">',
            '<input type="text" class="form-control" name="Idade" id="Idade" maxlength="5">',
            '<input type="text" class="form-control" name="DataNascimento" id="DataNascimento" maxlength="10">',
            '<select class="form-control select2-container" name="Unidade.Id" id="autocomplete_unidade"></select>',
            '<select class="form-control" name="Situacao" id="Situacao" disabled="disabled">' +
            '<option value="1">' + Europa.i18n.Messages.Situacao_Ativo + '</option>' +
            '<option value="2">' + Europa.i18n.Messages.Situacao_Suspenso + '</option>' +
            '<option value="3">' + Europa.i18n.Messages.Situacao_Cancelado + '</option>' +
            '</select>'
        ])
        // Define quais as colunas que serão mostradas no Datatable
        .setColumns([
            DTColumnBuilder.newColumn('Nome') // Define qual é a propriedade da classe que esta coluna exibirá
                .withTitle(Europa.i18n.Messages.Nome) // Define qual é o título da coluna no cabeçalho da tabela
                .withOption('width', '30%'), // Define a largura em porcentagem da coluna, podemos usar pixels também
            DTColumnBuilder.newColumn('Sobrenome').withTitle(Europa.i18n.Messages.Sobrenome).withOption('width', '30%'),
            DTColumnBuilder.newColumn('Idade').withTitle(Europa.i18n.Messages.Idade).withOption('width', '5%'),
            DTColumnBuilder.newColumn('DataNascimento')
                .withTitle(Europa.i18n.Messages.DataNascimento)
                .withOption("type", "date-format-DD/MM/YYYY") // Use a opção 'type' com 'date-format-{formato-de-data}' para obter uma data formatada
                .withOption('width', '7%'),
            DTColumnBuilder.newColumn('Unidade.Nome').withTitle(Europa.i18n.Messages.Unidade).withOption('width', '13%'),
            DTColumnBuilder.newColumn('Situacao').withTitle(Europa.i18n.Messages.Situacao)
                .withOption('type', 'enum-format-Situacao') // Use a opção 'type' com 'enum-format-{nome-do-enum}' para obter o nome da propriedade do Enum
                .withOption('width', '10%')
        ])
        .setColActions(actionsHtml, '60px')
        .setActionSave(Europa.Controllers.Entidade.Salvar)
        .setOptionsMultiSelect // Aqui configuramos o nosso datatable para seleção múltipla
        ('POST', // Método de requisição do método de listagem, utilizamos sempre POST
            Europa.Controllers.Entidade.UrlListarEntidades, // Url do método de listagem do Controller
            Europa.Controllers.Entidade.Filtro); // Callback da função de filtro que retorna um objeto JSON com chave e valor de filtro

    function actionsHtml(data, type, full, meta) {
        // Inicia a DIV com os botões de ação
        var botoes = '<div>';
        /*
         * Não se pode editar um registro cancelado (situação 3)
         */
        if (data.Situacao !== 3) {
            // Renderiza botão de editar passando a linha como parâmetro
            botoes = botoes +
                $scope.renderButton(Europa.i18n.Messages.Editar, "fa fa-edit", "Editar(" + meta.row + ")");
        }

        // Renderiza botão de excluir passando a linha como parâmetro
        botoes = botoes +
            $scope.renderButton(Europa.i18n.Messages.Excluir, "fa fa-trash", "Excluir(" + meta.row + ")") +
            // Fecha a DIV
            '</div>';

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
        // Obtém os dados da linha selecionada
        var objetoLinhaTabela = Europa.Controllers.Entidade.Tabela.getRowData(row);

        // Chama a função de excluir um registro
        Europa.Controllers.Entidade.Excluir(objetoLinhaTabela.Id);
    };

    $scope.Editar = function (row) {
        // Obtém os dados da linha selecionada
        var objetoLinhaTabela = Europa.Controllers.Entidade.Tabela.getRowData(row);

        // Carrega os dados e abre o modo edição
        $scope.rowEdit(row);

        //Inicializa os DatePickers novamente
        Europa.Controllers.Entidade.InicializarDatePicker();
        // Inicializa os Autocompletes novamente
        Europa.Controllers.Entidade.InicializarAutocompletes();

        Europa.Controllers.Entidade.AutoCompleteUnidade.SetValue(objetoLinhaTabela.Unidade.Id, objetoLinhaTabela.Unidade.Nome);

        // Aqui fazemos um parse e formatamos a data, depois inserimos a data no formato correto no campos de edição de Data 'Nascimento'
        $("#DataNascimento").val(Europa.Date.Format(Europa.Date.Parse(objetoLinhaTabela.DataNascimento), 'DD/MM/YYYY'));
    };
};

/*
 * Função que atribui o Datatable em HTML a seu código JavaScript
 */
DataTableApp.controller('Entidades', // Nome da lista de elementos a ser lida dentro do código HTML do Datatable
    Europa.Controllers.Entidade.CriarTabela); // Callback da função que cria a tabela

/*
 * Função que faz o filtro da tabela
 * Ela simplesmente invoca a função reloadData() do Datatable e recarrega o mesmo
 */
Europa.Controllers.Entidade.Filtrar = function () {
    Europa.Controllers.Entidade.Tabela.reloadData();
};

/*
 * Função que reseta os dados do formulário de filtro
 */
Europa.Controllers.Entidade.LimparFiltro = function () {
    $("#NomeFiltro").val("");
    $("#SobrenomeFiltro").val("");
    $("#DataNascimentoDe").val("");
    $("#DataNascimentoAte").val("");
    $("#IdadeFiltro").val("");
    $("#SituacaoFiltro").val(1).trigger("change");
    Europa.Controllers.Entidade.AutoCompleteUnidadeFiltro.SetValue("");
};

/*
 * Função que retorna o objeto JSON contendo os campos de filtro e seus valores
 * No momento ele não retorna filtro
 */
Europa.Controllers.Entidade.Filtro = function () {
    var params = {
        Nome: $("#NomeFiltro").val(),
        Sobrenome: $("#SobrenomeFiltro").val(),
        DataNascimentoDe: $("#DataNascimentoDe").val(),
        DataNascimentoAte: $("#DataNascimentoAte").val(),
        Situacao: $("#SituacaoFiltro").val(),
        Idade: $("#IdadeFiltro").val(),
        IdUnidade: $("#autocomplete_unidade_filtro").val()
    };

    return params;
};

/*
 * Função que salva uma entidade
 */
Europa.Controllers.Entidade.Salvar = function () {
    // Obtém os dados da linha
    var obj = Europa.Controllers.Entidade.Tabela.getDataRowEdit();

    $.post(Europa.Controllers.Entidade.UrlSalvarEntidade, { entidade: obj }, function (resposta) {
        // Esta função pega a mensagem de erro ou sucesso, coloca em um modal e mostra este
        // Se não houver sucesso ao salvar uma nova entidade, vamos mostrar as mensagens de erro
        Europa.Informacao.PosAcao(resposta);

        if (resposta.Sucesso) {
            // Fecha edição de tabela
            Europa.Controllers.Entidade.Tabela.closeEdition();
            // Recarregando tabela para mostrar entidade atualizada...
            Europa.Controllers.Entidade.Tabela.reloadData();
            // Limpa a borda vermelha dos campos do formulário de edição
            Europa.Controllers.Entidade.LimparErro();
        } else {
            // Adiciona borda vermelha aos campos com erro
            Europa.Controllers.Entidade.AdicionarErro(resposta.Campos);
        }
    });
};


Europa.Controllers.Entidade.Excluir = function (id) {
    $.post(Europa.Controllers.Entidade.UrlExcluirEntidade, { id: id }, function (resposta) {
        // Esta função pega a mensagem de erro ou sucesso, coloca em um modal e mostra este
        // Se não houver sucesso ao salvar uma nova entidade, vamos mostrar as mensagens de erro
        Europa.Informacao.PosAcao(resposta);

        if (resposta.Sucesso) {
            // Recarregando tabela para mostrar entidade atualizada...
            Europa.Controllers.Entidade.Tabela.reloadData();
        }
    });
};

/*
 * Função que abre edição na tabela para criar uma nova entidade
 */
Europa.Controllers.Entidade.CriarEntidade = function () {
    Europa.Controllers.Entidade.Tabela.createRowNewData();
    //Inicializa os DatePickers novamente
    Europa.Controllers.Entidade.InicializarDatePicker();
    // Inicializa os Autocompletes novamente
    Europa.Controllers.Entidade.InicializarAutocompletes();
};

Europa.Controllers.Entidade.AdicionarErro = function (campos) {
    campos.forEach(function (chave) {
        $("[name='" + chave + "']").parent().addClass("has-error");
    });
};

Europa.Controllers.Entidade.LimparErro = function () {
    $("[name='Nome']").parent().removeClass("has-error");
    $("[name='Sobrenome']").parent().removeClass("has-error");
    $("[name='Idade']").parent().removeClass("has-error");
    $("[name='DataNascimento']").parent().removeClass("has-error");
    $("[name='Situacao']").parent().removeClass("has-error");
    $("[name='Unidade.Id']").parent().removeClass("has-error");
};

/*
 * Função que altera a situação dos registros selecionados na tabela para 'Ativado'
 */
Europa.Controllers.Entidade.ReativarEmLote = function () {
    if (Europa.Controllers.Entidade.Tabela.getRowsSelect().length !== 0) {
        Europa.Confirmacao.PreAcaoMulti(Europa.i18n.Messages.Reativar, function () {
            Europa.Controllers.Entidade.AlterarSituacao(Europa.Controllers.Entidade.UrlReativarEntidadesEmLote);
        });
    }
};

/*
 * Função que altera a situação dos registros selecionados na tabela para 'Suspenso'
 */
Europa.Controllers.Entidade.SuspenderEmLote = function () {
    if (Europa.Controllers.Entidade.Tabela.getRowsSelect().length !== 0) {
        Europa.Confirmacao.PreAcaoMulti(Europa.i18n.Messages.Suspender, function () {
            Europa.Controllers.Entidade.AlterarSituacao(Europa.Controllers.Entidade.UrlSuspenderEntidadesEmLote);
        });
    }
};

/*
 * Função que altera a situação dos registros selecionados na tabela para 'Cancelado'
 */
Europa.Controllers.Entidade.CancelarEmLote = function () {
    if (Europa.Controllers.Entidade.Tabela.getRowsSelect().length !== 0) {
        Europa.Confirmacao.PreAcaoMulti(Europa.i18n.Messages.Cancelar, function () {
            Europa.Controllers.Entidade.AlterarSituacao(Europa.Controllers.Entidade.UrlCancelarEntidadesEmLote);
        });
    }
};

/*
 * Função que altera a situação dos registros selecionados na tabela
 */
Europa.Controllers.Entidade.AlterarSituacao = function (url) {
    var objetosSelecionados = Europa.Controllers.Entidade.Tabela.getRowsSelect();
    var ids = [];

    objetosSelecionados.map(function (x) {
        ids.push(x.Id);
    });

    $.post(url, { ids: ids }, function (resposta) {
        Europa.Controllers.Entidade.Tabela.reloadData();
        Europa.Informacao.PosAcao(resposta);
    });

};