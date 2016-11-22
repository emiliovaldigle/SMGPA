$(document).on("click", "#removeNotification", function (e) {
        var $div = $(this).closest('.notifications-list');
        var id = $(this).attr('value');
        var url = $(this).attr('url');
        var urlf = url + id;
        //SMGPA for publishing and .. for local run 
        $.post("SMGPA/"+urlf, function (result) {
            if (result.sucess) {
                location.reload();
            }
        });
});
