$(function () {
    $(document).on("click", "#addFunctionary", function (e) {
        //$("#addFunctionary").unbind('click').click(function (e) {
        var Rut = $('.rutfunctionary').val();
        var Cargo = $('.cargo').val();
        $.post('../Entities/AddFunctionary/', { rut: Rut, cargo: Cargo }, function (result) {
            if (result.sucess) {
                var row = "<tr><td>" + Rut
                + "</td><td>" + result.nombre
                + "</td><td>" + result.apellido
                + "</td><td>" + result.carrera
                + "</td><td>" + Cargo
                + "</td>+<td>" + "<a id='deleteFunctionary' url='/Entities/DeleteFunctionary/' value='" + result.iduser + "'>"
                + "<span class='glyphicon glyphicon-remove' aria-hidden='true'></span></a>"
                + "</td><tr>";
                $('#tableFunctionary> tbody:last').append(row);
                $('#alertwarning').fadeOut().hide();
                $("#alertsucess").fadeOut().hide();
                $("#alertsucess").fadeIn().show();
                $("#alertdelete").fadeOut().hide();
            }
            if (!result.sucess) {
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
        //$("#deleteFunctionary").unbind('click').click(function (e) {
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
            if (!result.sucess) {
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