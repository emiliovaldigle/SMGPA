$(document).on("click", "#deletePermission", function (e) {
    var $tr = $(this).closest('tr');
    var id = $(this).attr('value');
    var url = $(this).attr('url');
    var urlf = url + id;
    $.post(".."+urlf, function (result) {
        if (result.sucess) {
            $tr.fadeOut().remove();
            $("#alertdelete").fadeOut().hide();
            $("#alertdelete").fadeIn().show();
            $("#alertsucess").fadeOut().hide();
            $(".traceable").append("<option value = '" + result.idpermission + "'>" + result.textlink + "</option>");
            return false;
        }
        if(!result.sucess){
            location.reload();
        }
    });
    return false;
});
$(document).on("change", ".traceable", function (e) {
        var id = $(this).val();
        var url = $(this).attr('url');
        var urlf = url + id;
        $("#sendPermission").unbind('click').click(function(e) {
            $.post(".."+urlf, function (result) {
                if (result.sucess) {
                    var row = "<tr><td>" + result.textlink
                        + "</td><td>" + result.controller
                        +"</td><td>" + result.actionresult
                        + "</td>+<td>" + "<a id='deletePermission' url='/Roles/DeletePermission/' value='" + result.idpermission + "'>"
                        + "<span class='glyphicon glyphicon-remove' aria-hidden='true'></span></a>"
                        + "</td><tr>";
                    $("#tablePermission").find('tbody').append($(row));
                    $("#alertsucess").fadeOut().hide();
                    $("#alertsucess").fadeIn().show();
                    $("#alertdelete").fadeOut().hide();
                    $("option[value='" + result.idpermission + "']").remove();
                    return false;
                }
                if (!result.sucess) {
                    location.reload();
                }
                return false;
            });
        });
    });
        