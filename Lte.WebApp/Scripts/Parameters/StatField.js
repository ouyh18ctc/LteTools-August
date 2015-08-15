function UpdateFieldName(actionName) {
    $("#FieldName").change(function () {
        $.ajax({
            url: actionName,
            type: "GET",
            dataType: "json",
            data: { fieldName: $(this).val() },
            success: function (data) {
                $("#intervals").html("");
                for (var i = 0; i < data.length; i++) {
                    $("#intervals").append("<tr><td>" + data[i].L +
					"</td><td>" + data[i].H + "</td><td style='background-color:#" +
					data[i].C + "'>#" + data[i].C + "</td></tr>");
                }
            }
        });
    });
}