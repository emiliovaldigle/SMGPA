$(function () {
    $(document).on("click", "#deleteOperation", function (e) {
        var $tr = $(this).closest('tr');
        var id = $(this).attr('value');
        var url = $(this).attr('url');
        var urlf = url + id;
        $.post(".." + urlf, function (result) {
            if (result.sucess) {
                $("#alertdelete").fadeOut().hide();
                $("#alertdeleted").fadeOut().hide();
                $("#alertdelete").fadeIn().show();
                $("#confirmButton").unbind('click').click(function (e) {
                    $.post(".." + "/Processes/ConfirmDeleteOperation/", function (result) {
                        if (result.sucess) {
                            $("#alertdelete").fadeOut().hide();
                            $("#alertdeleted").fadeIn().show
                            $tr.fadeOut().remove();
                        }
                    });
                });
                $("#cancelButton").unbind('click').click(function (e) {
                   $("#alertdelete").fadeOut().hide();
                });
                return false;
            } else {
                location.reload();
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