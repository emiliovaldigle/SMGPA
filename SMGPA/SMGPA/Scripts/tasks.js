$(function () {
    $.datepicker.regional['es'] = {
        closeText: 'Cerrar',
        prevText: '<Ant',
        nextText: 'Sig>',
        currentText: 'Hoy',
        monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'],
        monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'],
        dayNames: ['Domingo', 'Lunes', 'Martes', 'Miércoles', 'Jueves', 'Viernes', 'Sábado'],
        dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mié', 'Juv', 'Vie', 'Sáb'],
        dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sá'],
        weekHeader: 'Sm',
        dateFormat: 'dd/mm/yy',
        firstDay: 1,
        isRTL: false,
        showMonthAfterYear: false,
        yearSuffix: ''
    };
    $.datepicker.setDefaults($.datepicker.regional['es']);
    $("#datepicker1").datetimepicker();
    $("#datepicker2").datetimepicker();
    $("#datepicker3").datepicker();
    $("#datepicker4").datepicker();
    $('#myModal').on('hidden.bs.modal', function () {
        location.reload();
    })
    $(".rutfunctionary").autocomplete({
        source: '/SMGPA/Activities/RutAutoComplete',
        appendTo: ".modal-body",
    });
    $('.ui-autocomplete').css('list-style-type', 'none');
    $('.ui-autocomplete').css('text-decoration', 'none');
    $(document).on("change", ".rutfunctionary", function (e) {
        var Rut = $('.rutfunctionary').val();
        $.get('/SMGPA/Activities/CheckUser/', { rut: Rut }, function (result) {
            if (result.sucess) {
                $('.Nombre').text(result.nombre);
                $('.Apellido').text(result.apellido);
                $('.Carrera').text(result.carrera);
                $("#idFunctionary").val(result.iduser);
            }
        });
    });
    $(".entidad").autocomplete({
        source: '/SMGPA/Activities/EntityAutoComplete',
        appendTo: ".modal-body",
    });
    $(document).on("change", ".entidad", function (e) {
        var Nombre = $('.entidad').val();
        $.get('/SMGPA/Activities/CheckEntity/', { nombre: Nombre }, function (result) {
            if (result.sucess) {
                $('#idResponsable').val(result.identity);
            }
        });
    });
    $(".validadora").autocomplete({
        source: '/SMGPA/Activities/EntityAutoComplete',
        appendTo: ".modal-body",
    });
    $(document).on("change", ".validadora", function (e) {
        var Nombre = $('.validadora').val();
        $.get('/SMGPA/Activities/CheckEntity/', { nombre: Nombre }, function (result) {
            if (result.sucess) {
                $('#idEntities').val(result.identity);
            }
        });
    });
});
