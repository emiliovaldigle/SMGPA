$(document).on("change", ".traceable", function (e) {
    var id = $(this).val();
    $("#addCareer").unbind('click').click(function (e) {
        $.post("/SMGPA/Entities/AddCareer/"+id, function (result) {
            if (result.sucess) {
                var row = "<tr><td>" + result.nombre
                    + "</td>+</tr>"
                $("#tableCareer").find('tbody').append($(row));
                $("option[value='" + result.idcareer + "']").remove();
                id = null;
                return false;
            }
            if (!result.sucess) {
                return false;
            }
            return false;
        });
    });
});