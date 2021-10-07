$(document).ready(function () {

    function recuperarCidade() {
        var estado = $(".estado option:selected").val();
        var cidadeCodigo = $("#cidadeCodigo").val();
        $.ajax({
            type: 'GET',
            data: { estado: estado },
            url: '/Cliente/recuperarCidadePorEstado',
            dataType: 'json',
            success: function (dados) {
                if (dados !== null) {

                    var selectbox = $('.cidade');
                    $(".cidade").empty()
                    $('<option>').val("0").text("").appendTo(selectbox);
                    $.each(dados.cidade, function (i, d) {
                        $('<option>').val(d.id).text(d.value).appendTo(selectbox);
                    })

                    if (cidadeCodigo != null) {
                        $('.cidade').val(cidadeCodigo);
                    }

                }
            },
            error: function () {
                console.log("erro");
            }
        });
    };

    //carga inicial
    recuperarCidade();

    $(".estado").on("change", function () {
        recuperarCidade();
    });

});