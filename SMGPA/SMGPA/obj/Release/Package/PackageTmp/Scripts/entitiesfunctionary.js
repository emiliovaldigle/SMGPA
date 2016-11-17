$(function () {
    $(".rutfunctionary").autocomplete({
        source: '../Entities/RutAutoComplete',
        appendTo: ".modal-body",
    });
    $('.ui-autocomplete').css('list-style-type', 'none');
    $('.ui-autocomplete').css('text-decoration', 'none');
    $("#checkUser").click(function (e) {
        var Rut = $('.rutfunctionary').val();
        $.get('../Entities/CheckUser/', { rut: Rut }, function (result) {
            if (result.sucess) {
                $('.Nombre').text(result.nombre);
                $('.Apellido').text(result.apellido);
                $('.Carrera').text(result.carrera);
            }  
        });
    });
    $("#addFunctionary").unbind('click').click(function (e) {
        var Rut = $('.rutfunctionary').val();
        $.post('../Entities/AddFunctionary/', {rut: Rut}, function (result) {
            if (result.sucess) {
                var row = "<tr><td>" + Rut
                +"</td><td>" + result.nombre
                + "</td><td>" + result.apellido
                + "</td><td>" + result.carrera
                + "</td>+<td>" + "<a id='deleteFunctionary' url='/Entities/DeleteFunctionary/' value='" + result.iduser + "'>"
                + "<span class='glyphicon glyphicon-remove' aria-hidden='true'></span></a>"
                + "</td><tr>";
                $('#tableFunctionary> tbody:last').append(row);
                $('#alertwarning').fadeOut().hide();
                $("#alertsucess").fadeOut().hide();
                $("#alertsucess").fadeIn().show();
                $("#alertdelete").fadeOut().hide();
            }
            if(!result.sucess){
                $("#alertsucess").fadeOut().hide();
                $("#alertdelete").fadeOut().hide();
                $('#alertwarning').fadeIn().show();
                if (result.reload) {
                    location.reload();
                }
            }
        });
    });
    $(document).on("click", "#deleteFunctionary", function (e) {
        var $tr = $(this).closest('tr');
        var id = $(this).attr('value');
        var url = $(this).attr('url');
        var urlf = url + id;
        $.post(".." + urlf, function (result) {
            if (result.sucess) {
                $tr.fadeOut().remove();
                $('#alertwarning').fadeOut().hide
                $("#alertsucess").fadeOut().hide();
                $("#alertdelete").fadeOut().hide();
                $("#alertdelete").fadeIn().show();
                return false;
            }
            if(!result.sucess){
                $("#alertsucess").fadeOut().hide();
                $("#alertdelete").fadeOut().hide();
                $('#alertwarning').fadeIn().show();
                if (result.reload) {
                    location.reload();
                }
            }
        });
        return false;
    });

});