$(function () {
    $(document).on("click", "#deleteOperation", function (e) {
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
            } else {
                alert("Problemas Eliminando");
            }
        });
        return false;
    });
    $(document).on("click", "#newOperation", function (e) {
        $('#myModal').modal('hide');
        
    });
    $(document).on("click", ".editOperation", function (e) {
        $('#myModal').modal('hide');
    });
});