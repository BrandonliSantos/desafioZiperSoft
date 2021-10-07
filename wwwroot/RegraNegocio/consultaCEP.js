$(document).ready(function () {

    function limpa_formulario_cep() {
        
        $("#endereco").val("");
        $("#bairro").val("");
        $(".cidade").val("");
        $(".estado").val("");
    }

    function recuperarCidade(cidade) {
        var estado = $(".estado option:selected").val();
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

                    $('.cidade').val(cidade);
                }
            },
            error: function () {
                console.log("erro");
            }
        });
    }

    
    $("#cep").blur(function () {
  
        var cep = $(this).cleanVal();

       
        if (cep != "") {

            //Expressão regular para validar o CEP.
            var validacep = /^[0-9]{8}$/;

            //Valida o formato do CEP.
            if (validacep.test(cep)) {

                //Preenche os campos com "..." enquanto consulta webservice.
                $("#endereco").val("...");
                $("#bairro").val("...");
                $(".estado").val("...");
                $(".cidade").val("...");

                //Consulta o webservice viacep.com.br/
                $.getJSON("https://viacep.com.br/ws/" + cep + "/json/?callback=?", function (dados) {

                    if (!("erro" in dados)) {
                        //Atualiza os campos com os valores da consulta.
                        $("#endereco").val(dados.logradouro);
                        $("#bairro").val(dados.bairro);
                        $(".estado").val(dados.uf);
                        $(".estado").focus();
                        $("#numero").focus();
                        var cidade = dados.ibge;
                        recuperarCidade(cidade);
                        
                                               
                  
                    }
                    
                    else {
                        
                        limpa_formulario_cep();
                        alert("CEP não encontrado.");
                    }
                    
                });
                
            } 
            else {
                
                limpa_formulario_cep();
                alert("Formato de CEP inválido.");
            }
        } 
        else {
            
            limpa_formulario_cep();
        }

        
    });
});
