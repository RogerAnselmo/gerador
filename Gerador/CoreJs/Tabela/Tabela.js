/*
Autor   : Pedro Carvalho
Data    : 05/08/2017
Objetivo: Todas as informações dentro da função abaixo serão executadas pelo sistema assim que o formulário Parametros do Sistema for carregado
*/
$(document).ready(function () {
    $('#Nome').val('');
    $('#Tabelas').html('');
    $('#btGerarClasse').hide();

    $('#NameSpace').bind("blur", function (e) {
        if ($('#NameSpace').val() != '') {
            CriaDiretorios();
        }
    });

    $('#Nome').change(function () {
        if ($('#Nome').val() != '') {
            CarregaLista('#Tabelas', '../Home/Propriedade', { NomeTabela: $('#Nome').val() });
            $('#btGerarClasse').show();
        }
        else {
            $('#Tabelas').html('');
            $('#btGerarClasse').hide();
        }
    });

    $('#btGerarClasse').click(function () {
        GerarClasse();
    })
});


function GerarClasse() {
    var params = new Object();
    params.NomeTabela = $('#Nome').val();
    params.NameSpace = $('#NameSpace').val();
    params.NameSchema = $('#NameSchema').val();

    $.ajax({
        type: "POST",
        url: '../Home/Salvar/',
        data: JSON.stringify(params),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (resultado) {
            $('#ClasseGerada').html(resultado);
        },
        error: function (e, exc) {
            alert(e);
        }
    });
}

function CriaDiretorios() {
    var params = new Object();
    params.NameSpace = $('#NameSpace').val();

    $.ajax({
        type: "POST",
        url: '../Home/CriaDiretorios/',
        data: JSON.stringify(params),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (resultado) {
           $('#SubDiretorios').html(resultado);
        },
        error: function (e, exc) {
            console.log(e);
        }
    });
}