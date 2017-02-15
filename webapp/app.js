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

function atualizarRegistroDeTransmissao(streamId, userId, qtdDeixouPagina) {
    firebase.database().ref('transmissao/' + streamId).update({ qtd_deixou_pagina: qtdDeixouPagina });
}

function inserirRegistroDeTransmissao(streamId, userId, inicioDaTransmissao) {
    firebase.database().ref('transmissao/' + streamId).set({
        user_id: userId,
        inicio_transmissao: inicioDaTransmissao,
        qtdDeixouPagina: 0
    });

};

function registrarInicioDaTransmissao() {

    var inicioDaTransmissao = new Date();

    var streamId = $("#streamId").val();
    var userId = $("#userId").val();

    inserirRegistroDeTransmissao(streamId, userId, inicioDaTransmissao.toString());
}

var tempo = new Number();

// Tempo em segundos
tempo = 0;

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
    horaImprimivel = '00:' + min + ':' + seg;
    //JQuery pra setar o valor
    $("#tempoDeTransmissao").html(horaImprimivel);

    // Define que a função será executada novamente em 1000ms = 1 segundo
    setTimeout('iniciarContador()', 1000);

    // diminui o tempo
    tempo++;
}

function atualizaEstatisticaDeSaiu() {
    saiu += 1;
    $("#quantidadeVezesSaiu").text(saiu);

    var streamId = $("#streamId").val();
    var userId = $("#userId").val();

    atualizarRegistroDeTransmissao(streamId, userId, saiu);
}

function atualizaEstatisticaDeEntrou() {
    entrou += 1;
    $("#quantidadeVezesEntrou").text(saiu);
}

// Chama a função ao carregar a tela
iniciarContador();

configurarFirebase();

registrarInicioDaTransmissao();


var saiu = 0;
var entrou = 0;

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
