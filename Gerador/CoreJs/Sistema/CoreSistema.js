
function configuraFormulario(_nameform, _success, _error) {
    $(_nameform).on("submit", function (event) {
        event.preventDefault();
        var url = $(this).attr("action");
        var formData = $(this).serialize();
        $.ajax({
            url: this.action,
            type: this.method,
            data: $(this).serialize(),
            dataType: "json",
            success: _success,
            error: _error
        })
    });
}

CarregaModulo = function (target, url, parametro) {
    $.ajax({
        type: "POST",
        url: url,
        data: parametro,
        success: function (resposta) {
            var div = document.createElement("div");
            div.setAttribute("id", target);
            document.body.appendChild(div);
            $(target).html(resposta);
        }
    });
};

CarregaLista = function (target, url, parametro) {
    $.ajax({
        type: "POST",
        url: url,
        data: JSON.stringify(parametro),
        contentType: "application/json; charset=utf-8",
        dataType: "html",
        success: function (resposta) {
            var div = document.createElement("div");
            div.setAttribute("id", target);
            document.body.appendChild(div);
            $(target).html(resposta);
        }
    });
};

/*
 Auto      : Pedro Carvalho
 Data      : 21/07/2017
 Objetivo  : Configura o tamanho dos TextBox do Razor
*/
function configuraTextBox(target, width, title, maxlength) {
    $(target).css({
        width: width,
    }).attr({
        title: title,
        maxlength: maxlength
    });
}

/*
 Autor   : Pedro Carvalho
 Data      : 21/07/2017
 Objetivo: Seta data corrente
*/
function setaDataCorrente(target) {
    var $datepicker = $(target);
    $datepicker.datepicker();
    $datepicker.datepicker('setDate', new Date());
}

function setDataCorrenteSemCalendario(target) {

    var dateObject = new Date();

    var dia = dateObject.getDate();
    var mes = dateObject.getMonth() + 1;
    var ano = dateObject.getFullYear();

    var diaaux = '';
    var mesaux = '';
    var datacompleta = '';


    //1/2/2017
    if (parseInt(dia) < 10)
        diaaux = '0' + dia;
    else
        diaaux = dia;

    if (parseInt(mes) < 10)
        mesaux = '0' + mes;
    else
        mesaux = mes;

    datacompleta = diaaux + '/' + mesaux + '/' + ano;

    $(target).val(datacompleta);
}

/*
Autor   : Pedro Carvalho
Data    : 21/07/2017
Objetivo: Configura Calendário
*/
function configuraDapaPicker(_target) {
    $(_target).datepicker({
        dateFormat: 'dd/mm/yy',
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: 'Próximo',
        prevText: 'Anterior'
    });
}

function retornaData() {
    var now = new Date();
    var day = ("0" + now.getDate()).slice(-2);
    var month = ("0" + (now.getMonth() + 1)).slice(-2);
    var today = (day) + "/" + (month) + "/" + now.getFullYear();
    return today;
}

/*
Autor   : Pedro Carvalho
Data    : 21/07/2017
Objetivo: Seta mascara para campos numéricos
*/
function setaMascaraNumerica(target) {
    $(target).setMask({ mask: '99,999.999.999.999', type: 'reverse' });
}

/*
Auto      : Pedro Carvalho
Data: 21 / 07 / 2017
Objetivo  : Função que povoa um objeto comobox
*/
function povoarComboBox(res, _target, _fieldvalue, _textfield) {
    $.each(res, function (i, res) {
        $(_target).append('<option value="' + res[_fieldvalue] + '">' + res[_textfield] + '</option>');
    });
}

function criaTabela(_Target, _Titulo, _Colunas, _Campos, _Botoes, _Funcoes) {
    var _TabelaInicio = '<table class=\"table table-striped\">';
    var _TabelaFim = '</table>';
    var _trinicio = '<tr>';
    var _trfim = '</tr>'
    var _thinicio = '<th>';
    //var _thfim = '</th>';
    //var _tdinicio = '<td>';
    var _MontaTabela = null;

    $.each(_Titulo, function (i, val) {
        var _value = '<th>' + val + '</th>';
        _trinicio += _value;
    });

    _trinicio += _trfim;
    _trinicio += '<tr>';

    var quebra = false;
    var id = 0;

    var _NomeBotoes = ' ';
    var _NomeFuncoes = [];
   
    _NomeFuncoes = _Funcoes.toString().split('#');

    $.each(_Colunas, function (i, val) {
        $.each(_Campos, function (j, field) {
            if (field != 'linha') {
                if (!quebra) {
                    var _value = '<td>' + val[field] + '</td>';
                    _trinicio += _value;

                    if (j == 0) {
                        id = val[field]                       
                    };
                }
                else {
                    quebra = false;
                    _NomeBotoes = '';

                    $.each(_Botoes, function (i, botoes) {                       
                        _NomeBotoes += '<a href=\'#\' id=' + _NomeFuncoes[i] + ' onclick=' + _NomeFuncoes[i] + '("' + id + '")>' + RetornaIconeBotao(botoes) + '</a > ';
                    });                  

                    _trinicio += '<td>' + _NomeBotoes + '</td></tr>';

                    var _value = '<tr><td>' + val[field] + '</td>';
                    _trinicio += _value;
                    id = val[field];
                }
            }
            else {
                quebra = true;
            }
        });
    });

    _NomeBotoes = '';
    $.each(_Botoes, function (i, botoes) {
        _NomeBotoes += '<a href=\'#\' id=' + _NomeFuncoes[i] + ' onclick=' + _NomeFuncoes[i] + '("' + id + '")>' + RetornaIconeBotao(botoes) + '</a > ';
    });
    _trinicio += '<td>' + _NomeBotoes + '</td></tr>';

    _MontaTabela = _TabelaInicio;
    _MontaTabela += _trinicio;
    _MontaTabela += _thinicio;
    _MontaTabela += _trfim;
    _MontaTabela += _TabelaFim;

    console.log(_MontaTabela);

    $('#' + _Target).html(_MontaTabela);//.addClass(_css);
}


function RetornaIconeBotao(_Nome) {
    var icone = '';
    switch (_Nome) {
        case 'Editar':
            icone = '<i class="fa fa-pencil fa-lg" aria-hidden="true"></i>';
            break;
        case 'Excluir':
            icone = '<i class="fa fa-trash-o fa-lg" aria-hidden="true"></i>';
            break;

        case 'Imprimir':
            icone = '<i class="fa fa-print fa-lg" aria-hidden="true"></i>';
            break;

    }

    return icone;
}

function ConfiguraUrl(_url) {
    var NovoUrl = $('#tbUrl').val() + "/Default/Default#" + _url;
    window.history.pushState("", "Title", "/" + NovoUrl);
}

//function ConfiguraUrl(_root, _url) {
//    var NovoUrl = _root == null ? "/Default/Default#" + _url : _root + "/Default/Default#" + _url;
//    window.history.pushState("", "Title", "/" + NovoUrl);
//}

/*
* Script para configurações globais da aplicação. 
* Inserir em todas as páginas!
*/

// Configura o formatador de número/moedas para PT-BR
numeral.language('pt-br');
//$.mask.masks.telefone = { mask: "(99) 99999-9999"};

//Configura animação Ajax Loading 
$(document).ajaxSend(function () {
    // Espera 500ms antes de exibir o Ajax Loading
    setTimeout(function () {
        if ($.active > 0) {
            $(document.body).waitMe({ effect: "roundBounce", color: "#FF0000", text: 'Carregando...' });
        }
    }, 100);
});

// Fecha Ajax Loading ao completar qualquer requisicao ajax
$(document).ajaxComplete(function () {
    if ($.active <= 1) {
        $(document.body).waitMe('hide');
    }
});

$(document).ajaxError(function (e, exc) {
    $(document.body).waitMe('hide');
    // TODO tratar erros e exibir em Modal Box
});

