(function ($) {
    $.fn.showChartDialog = function(title, options, width, height) {
        this.dialog({
            modal: true,
            title: title,
            hide: 'slide',
            width: width,
            height: height,
            buttons: {
                '关闭': function() {
                    $(this).dialog("close");
                }
            },
            open: function() {
                $(this).html("");
                $(this).highcharts(options);
            }
        });
    };

    $.fn.showDtInfoListDialog = function(title, list, width, height) {
        this.dialog({
            modal: true,
            title: title,
            hide: 'slide',
            width: width,
            height: height,
            buttons: {
                '关闭': function () {
                    $(this).dialog("close");
                }
            },
            open: function () {
                var content = $(this);
                content.html("");
                content.append('<thead><tr><th>时间</th><th>类型</th><th>文件名</th></tr></thead>');
                $(list).each(function(index) {
                    content.append('<tr><td>' + list[index].TestDate + '</td><td>'
                        + list[index].Type + '</td><td><a href="/Coverage/FileRecords'
                        + list[index].Type + '?fileName='
                        + list[index].Name.split('.')[0] + '">' + list[index].Name + '</a></td></tr>');
                });
            }
        });
    };

    $.fn.showRasterInfoListDialog = function(title, list, type, width, height) {
        this.dialog({
            modal: true,
            title: title,
            hide: 'slide',
            width: width,
            height: height,
            buttons: {
                '关闭': function () {
                    $(this).dialog("close");
                }
            },
            open: function () {
                var content = $(this);
                content.html("");
                content.append('<tr><th>文件名</th></tr>');
                $(list).each(function (index) {
                    content.append('<tr><td><a href="/Coverage/FileRecords'
                        + type + '?fileName='
                        + list[index] + '">' + list[index] + '</a></td></tr>');
                });
            }
        });
    };

    $.fn.updateColor = function(data, fieldName) {
        var colorWidth = parseInt(400 / data.length);
        var tag = $(this);
        tag.html("");
        tag.append("<td>" + fieldName + "</td>");

        for (var i = 0; i < data.length; i++) {
            tag.append("<td style='background-color:#"
                + data[i].C + "' width='" + colorWidth + "'></td>");
        }
    };

    $.fn.updateColorTag = function (actionName, fieldName) {
        var tag = $(this);
        $.ajax({
            url: actionName,
            type: "GET",
            dataType: "json",
            data: { fieldName: fieldName },
            success: function (data) {
                tag.updateColor(data, fieldName);
            }
        });
    };

    $.fn.updateLimit = function(data) {
        var tag = $(this);
        tag.html("");
        tag.append("<td></td>");
        for (var i = 0; i < data.length; i++) {
            tag.append("<td align='left'>" + data[i].L + "</td>");
        }
    };

    $.fn.updateLimitTag = function(actionName, fieldName) {
        var tag = $(this);
        $.ajax({
            url: actionName,
            type: "GET",
            dataType: "json",
            data: { fieldName: fieldName },
            success: function (data) {
                tag.updateLimit(data);
            }
        });
    };

    $.fn.generatePreciseRateCharts = function(dataSet, district, keyword) {
        var dom = $(this);
        $.ajax({
            url: "/api/QueryLteDistrictDates/" + district,
            type: "GET",
            dataType: "json",
            success: function (dates) {
                dataSet.generateChart(district, dates, dom, keyword);
            }
        });
    };

    $.fn.updateDatesTableHeader = function(dates) {
        var header = $(this);
        var contents = "<th>区域</th>";
        $(dates).each(function(index) {
            contents += "<th>" + dates[index] + "</th>";
        });
        header.html(contents);
    };

    $.fn.appendMrsTableRow = function(district) {
        var row = $(this);
        $.ajax({
            url: "/api/QueryLteMrTable/" + district,
            type: "GET",
            dataType: "json",
            success: function (value) {
                var contents = "<td>" + district + "</td>";
                $(value).each(function (index) {
                    contents += "<td>" + value[index] + "</td>";
                });
                row.html(contents);
            }
        });
    };

    $.fn.appendRatesTableRow = function (district) {
        var row = $(this);
        $.ajax({
            url: "/api/QueryPreciseRateTable/" + district,
            type: "GET",
            dataType: "json",
            success: function (value) {
                var contents = "<td>" + district + "</td>";
                $(value).each(function (index) {
                    contents += "<td>" + parseInt(value[index]*10000)/100 + "</td>";
                });
                row.html(contents);
            }
        });
    };
})(jQuery)