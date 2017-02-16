var tempo = new Number();

// Tempo em segundos
tempo = 0;

var contadorDeTempoAtivo;
var tempoAtivo = new Number();

tempoAtivo = 0;

var saiu = 0;
var entrou = 0;

var transmissaoId = 0;

function configurarFirebase() {
    // Initialize Firebase
    var config = {
        apiKey: "AIzaSyCQKBE_vogjU_ZOq35w21OE25HYrlza5tU",
        authDomain: "netshowme03.firebaseapp.com",
        databaseURL: "https://netshowme03.firebaseio.com",
        storageBucket: "netshowme03.appspot.com",
        messagingSenderId: "435903866174"
    };
    firebase.initializeApp(config);
}

function getTransmissaoId() {

    var parametro = getUrlParameter('id');

    if (parametro)
        transmissaoId = getUrlParameter('id');

    return transmissaoId;
}

function getUsuarioID() {
    return $("#userId").val();
}

// https://davidwalsh.name/query-string-javascript
function getUrlParameter(name) {
    name = name.replace(/[\[]/, '\\[').replace(/[\]]/, '\\]');
    var regex = new RegExp('[\\?&]' + name + '=([^&#]*)');
    var results = regex.exec(location.search);
    return results === null ? '' : decodeURIComponent(results[1].replace(/\+/g, ' '));
}

function atualizarRegistroDeTransmissao(transmissaoID, usuarioID, qtdDeixouPagina) {
    firebase.database().ref('transmissao/' + transmissaoID + '/usuarios/' + usuarioID).update(
        {
            qtd_deixou_pagina: qtdDeixouPagina,
            tempo_transmissao: tempo,
            tempo_transmissao_ativa: tempoAtivo
        });

}

function inserirRegistroDeTransmissao(transmissaoID, usuarioID, inicioDaTransmissao) {
    firebase.database().ref('transmissao/' + transmissaoID + '/usuarios/' + usuarioID).set(
        {
            inicio_transmissao: inicioDaTransmissao,
            qtd_deixou_pagina: 0,
            tempo_transmissao: 0,
            tempo_transmissao_ativa: 0
        });

}

function registrarInicioDaTransmissao() {
    var inicioDaTransmissao = new Date();

    var streamId = getTransmissaoId();
    var userId = getUsuarioID();

    inserirRegistroDeTransmissao(streamId, userId, inicioDaTransmissao.toString());
}

function iniciarContador() {

    // Pega a parte inteira dos minutos
    var min = parseInt(tempo / 60);
    // Calcula os segundos restantes
    var seg = tempo % 60;

    // Formata o número menor que dez, ex: 08, 07, ...
    if (min < 10) {
        min = "0" + min;
        min = min.substr(0, 2);
    }
    if (seg <= 9) {
        seg = "0" + seg;
    }

    // Cria a variável para formatar no estilo hora/cronômetro
    var horaImprimivel = '00:' + min + ':' + seg;
    //JQuery pra setar o valor
    $("#tempoDeTransmissao").html(horaImprimivel);

    // Define que a função será executada novamente em 1000ms = 1 segundo
    setTimeout('iniciarContador()', 1000);

    // diminui o tempo
    tempo++;
}

function iniciarContadorDeTempoAtivo() {

    // Pega a parte inteira dos minutos
    var min = parseInt(tempoAtivo / 60);
    // Calcula os segundos restantes
    var seg = tempoAtivo % 60;

    // Formata o número menor que dez, ex: 08, 07, ...
    if (min < 10) {
        min = "0" + min;
        min = min.substr(0, 2);
    }
    if (seg <= 9) {
        seg = "0" + seg;
    }

    // Cria a variável para formatar no estilo hora/cronômetro
    var horaImprimivel = '00:' + min + ':' + seg;
    //JQuery pra setar o valor
    $("#tempoDeTransmissaoAtivo").html(horaImprimivel);

    clearTimeout(contadorDeTempoAtivo);

    // Define que a função será executada novamente em 1000ms = 1 segundo
    contadorDeTempoAtivo = setTimeout('iniciarContadorDeTempoAtivo()', 1000);

    // diminui o tempoAtivo
    tempoAtivo++;
}
function pararContadorDeTempoAtivo() {
    clearTimeout(contadorDeTempoAtivo);
}

function atualizaEstatisticaDeSaiu() {
    saiu += 1;
    $("#quantidadeVezesSaiu").attr('value', saiu);

    pararContadorDeTempoAtivo();

    var transmissaoID = getTransmissaoId();
    var usuarioID = getUsuarioID();

    atualizarRegistroDeTransmissao(transmissaoID, usuarioID, saiu);
}

function atualizaEstatisticaDeEntrou() {
    entrou += 1;
    $("#quantidadeVezesEntrou").attr('value', entrou);

    iniciarContadorDeTempoAtivo();
}

function comecarTransmissao() {

    saiu = 0;
    entrou = 0;

    tempo = 0;
    tempoAtivo = 0;

    iniciarContador();

    iniciarContadorDeTempoAtivo();

    registrarInicioDaTransmissao();

    $("#divTransmissao").show();
}

configurarFirebase();

if (getTransmissaoId()) {
    comecarTransmissao();
}

//https://greensock.com/forums/topic/9059-cross-browser-to-detect-tab-or-window-is-active-so-animations-stay-in-sync-using-html5-visibility-api/

// check if browser window has focus
var notIE = (document.documentMode === undefined),
    isChromium = window.chrome;

if (notIE && !isChromium) {

    // checks for Firefox and other  NON IE Chrome versions
    $(window).on("focusin", function () {

        // tween resume() code goes here
        setTimeout(function () {
            atualizaEstatisticaDeEntrou();
        }, 300);

    }).on("focusout", function () {

        atualizaEstatisticaDeSaiu();

    });

} else {

    // checks for IE and Chromium versions
    if (window.addEventListener) {

        // bind focus event
        window.addEventListener("focus", function (event) {

            // tween resume() code goes here
            setTimeout(function () {
                atualizaEstatisticaDeEntrou();
            }, 300);

        }, false);

        // bind blur event
        window.addEventListener("blur", function (event) {

            atualizaEstatisticaDeSaiu();

        }, false);

    } else {

        // bind focus event
        window.attachEvent("focus", function (event) {

            // tween resume() code goes here
            setTimeout(function () {
                atualizaEstatisticaDeEntrou();
            }, 300);

        });

        // bind focus event
        window.attachEvent("blur", function (event) {
            atualizaEstatisticaDeSaiu();
        });
    }
}
