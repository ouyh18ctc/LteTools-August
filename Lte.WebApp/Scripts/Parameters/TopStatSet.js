function TopStatSet(city) {
    this.city = city;
}

TopStatSet.prototype.generateTopChart = function (data, topStatTag, kpiName) {
    if (data.length > 0) {
        var carriernames = [];
        var topdates = [];
        var sumofdrops = [];
        $(data).each(function (index) {
            carriernames.push(data[index].CarrierName);
            topdates.push(data[index].TopDates);
            sumofdrops.push(data[index].SumOfTimes);
        });
        var setting = new ComboSetting(this.city + 'TOP' + kpiName + '分析（按次数降序排列）');
        setting.categories = carriernames;
        setting.xLabel = '载扇名称';
        setting.setPrimaryYAxis('次', kpiName + '次数');
        setting.addColumnSeries(sumofdrops, kpiName + '次数', '次', 0);
        setting.addYAxis('TOP天数', '天', 1);
        setting.addLineSeries(topdates, 'TOP天数', '天', 1);
        $(topStatTag + this.city).highcharts(setting.getOptions());
    }
};

TopStatSet.prototype.queryTopDrop2G = function (begin, end, topStatTag, topCounts) {
    var statSet = this;
    $.ajax({
        url: "/TopDrop2G/Query",
        type: "GET",
        dataType: "json",
        data: {
            city: this.city,
            begin: begin,
            end: end,
            topCounts: topCounts
        },
        success: function (data) {
            statSet.generateTopChart(data, topStatTag, '掉话');
        }
    });
};

TopStatSet.prototype.queryTopDrop2GDaily = function (begin, end, topStatTag, topCounts) {
    var statSet = this;
    $.ajax({
        url: "/TopDrop2G/QueryDaily",
        type: "GET",
        dataType: "json",
        data: {
            begin: begin,
            end: end,
            topCounts: topCounts
        },
        success: function (data) {
            statSet.generateTopChart(data, topStatTag, '掉话');
        }
    });
};

TopStatSet.prototype.queryTopConnection3G = function (begin, end, topStatTag, topCounts) {
    var statSet = this;
    $.ajax({
        url: "/TopConnection3G/Query",
        type: "GET",
        dataType: "json",
        data: {
            city: this.city,
            begin: begin,
            end: end,
            topCounts: topCounts
        },
        success: function (data) {
            statSet.generateTopChart(data, topStatTag, '连接失败');
        }
    });
};