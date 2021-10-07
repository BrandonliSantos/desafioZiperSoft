$(document).ready(function () {

    var mascaraAtualCpf_cnpj;
    var mascaraAtualCelular;

    //mascara temporaria para o valor nao retornar nulo
    $('#cpf_cnpj').mask('00.000.000/0000-00');

    var cpf_cnpj = $('#cpf_cnpj').cleanVal();

    if (cpf_cnpj.length == 14) {
        $('#cpf_cnpj').mask('00.000.000/0000-00');
        $('#labelCpf_cnpj').text("CNPJ");
        $('#rg_ie').mask('000.000.000.000');
        $('#labelRg_ie').text("Inscrição Estadual");
        $('#razao_social').attr('readonly', false);
        mascaraAtualCpf_cnpj = "cnpj";
    }

    else {
        $('#cpf_cnpj').mask('000.000.000-009');
        $('#labelCpf_cnpj').text("CPF");
        $('#rg_ie').mask('00.000.000-0');
        $('#labelRg_ie').text("RG");
        $('#razao_social').attr('readonly', true);
        mascaraAtualCpf_cnpj = "cpf";
    }

    $('#celular').mask('(00)0000-00009');
    var celular = $('#celular').cleanVal();

    if (celular.length == 11) {
        $('#celular').mask('(00)00000-0000');
        mascaraAtualCelular = "9digitos";
    }
    else {
        $('#celular').mask('(00)0000-00009');
        mascaraAtualCelular = "8digitos";
    }


    $('#cep').mask('99999-999');
    $('#telefone').mask('(99)9999-9999');
    
    
    

    //mascara dinamica para campo cpf/cnpj - rg/inscricao estadual e razao social
    $(document).on('input', '#cpf_cnpj', function (event) {

        var cpf_cnpj = $(this).cleanVal();

        if (jQuery.isNumeric(cpf_cnpj) == true) {

            if (cpf_cnpj.length == 12 && mascaraAtualCpf_cnpj != "cnpj") {
                $('#cpf_cnpj').unmask().mask('00.000.000/0000-00');
                mascaraAtualCpf_cnpj = "cnpj";
                $('#labelCpf_cnpj').text("CNPJ");
                $('#rg_ie').unmask().mask('000.000.000.000');
                $('#rg_ie').val('');
                $('#labelRg_ie').text("Inscrição Estadual");
                $('#razao_social').val('');
                $('#razao_social').attr('readonly', false);
                this.selectionStart = this.selectionEnd = this.value.length;
            }
            else if (cpf_cnpj.length == 11 && mascaraAtualCpf_cnpj != "cpf") {
                $('#cpf_cnpj').unmask().mask('000.000.000-009');
                mascaraAtualCpf_cnpj = "cpf";
                $('#labelCpf_cnpj').text("CPF");
                $('#rg_ie').unmask().mask('00.000.000-0');
                $('#rg_ie').val('');
                $('#labelRg_ie').text("RG");
                $('#razao_social').val('');
                $('#razao_social').attr('readonly', true);
                this.selectionStart = this.selectionEnd = this.value.length;
                
            }
        }
    });

    //mascara para celular com ou sem 9° digito
    $(document).on('input', '#celular', function (event) {
        var celular = $(this).cleanVal();
        console.log(celular.length);
        if (jQuery.isNumeric(celular) == true) {
            if (celular.length == 11 && mascaraAtualCelular != "9digitos") {
                $('#celular').unmask().mask('(00)00000-0000');
                mascaraAtualCelular = "9digitos";
                this.selectionStart = this.selectionEnd = this.value.length;
            }

            else if (celular.length == 10 && mascaraAtualCelular != "8digitos") {
                $('#celular').unmask().mask('(00)0000-00009');
                mascaraAtualCelular = "8digitos";
                this.selectionStart = this.selectionEnd = this.value.length;
            }
        }
    });

    // Permitir apenas números
    $("#telefone").keypress(function (e) {
 
        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {          
            return false;
        }
    });

    $("#celular").keypress(function (e) {

        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });

    $("#cep").keypress(function (e) {

        if (e.which != 8 && e.which != 0 && (e.which < 48 || e.which > 57)) {
            return false;
        }
    });


});









