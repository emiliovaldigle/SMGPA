function deletePermission(elem) {
    var $tr = $(this).closest('tr');
    $.post(elem, function (result) {
        var res = (JSON.stringify(result));
        if (res) {
            $tr.fadeOut().remove();
            $(".alert").fadeIn().show();
            $tr = null;
            return false;
        } else {
            alert("Problemas borrando");
        }
        
    });
    return false;
};
