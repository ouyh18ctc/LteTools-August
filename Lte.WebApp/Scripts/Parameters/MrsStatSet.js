function MrsStatSet(setting) {
    this.setting = setting;
};

MrsStatSet.prototype.generateCoveragePlot = function (url, dom) {
    var cells = [];
    var mrs = [];
    var cov105 = [];
    var cov110 = [];
    var cov115 = [];
    var setting = this.setting;
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        success: function(data) {
            $(data).each(function(index) {
                cells.push(data[index].CellName);
                mrs.push(data[index].TotalMrs);
                cov105.push(data[index].CoveragePercentage);
                cov110.push(data[index].CoverageTo110);
                cov115.push(data[index].CoverageTo115);
            });
            setting.categories = cells;
            setting.xLabel = "小区名称";
            setting.setPrimaryYAxis('次', 'MR数量');
            setting.addLineSeries(mrs, 'MR数量', '次', 0);
            setting.addYAxis('覆盖率', '%', 2);
            setting.addColumnSeries(cov105, "覆盖率(>-105dBm)(%)", '%', 1);
            setting.addColumnSeries(cov110, "覆盖率(>-110dBm)(%)", '%', 1);
            setting.addColumnSeries(cov115, "覆盖率(>-115dBm)(%)", '%', 1);
            dom.highcharts(setting.getOptions());
        }
    });
}

MrsStatSet.prototype.generateRsrpTaPlot = function(url, dom, width, height) {
    var setting = this.setting;
    var intervals = [4, 8, 16, 24, 40, 56, 80, 128];
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        success: function(data) {
            setting.categories = [
                '0~' + parseInt(intervals[0] * 78.12),
                parseInt(intervals[0] * 78.12) + '~' + parseInt(intervals[1] * 78.12),
                parseInt(intervals[1] * 78.12) + '~' + parseInt(intervals[2] * 78.12),
                parseInt(intervals[2] * 78.12) + '~' + parseInt(intervals[3] * 78.12),
                parseInt(intervals[3] * 78.12) + '~' + parseInt(intervals[4] * 78.12),
                parseInt(intervals[4] * 78.12) + '~' + parseInt(intervals[5] * 78.12),
                parseInt(intervals[5] * 78.12) + '~' + parseInt(intervals[6] * 78.12),
                parseInt(intervals[6] * 78.12) + '~' + parseInt(intervals[7] * 78.12),
                parseInt(intervals[7] * 78.12) + '~inf'
            ];
            setting.xLabel = "覆盖距离范围（米）";
            setting.setPrimaryYAxis('次', 'MR数量');
            $(data).each(function(index) {
                setting.addAreaSeries([data[index].TaTo4, data[index].TaTo8, data[index].TaTo16,
                    data[index].TaTo24, data[index].TaTo40, data[index].TaTo56,
                    data[index].TaTo80, data[index].TaTo128, data[index].TaAbove128
                ], data[index].RsrpDescription, '次', 0);
            });
            dom.showChartDialog(setting.title, setting.getOptions(), width, height);
        }
    });
};

MrsStatSet.prototype.generateTaPlot=function(url, dom, width, height) {
    var setting = this.setting;
    var intervals = [2, 4, 6, 8, 12, 16, 20, 24, 32, 40, 48, 56, 64, 80, 96, 128, 192, 256];
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        success: function (data) {
            setting.categories = ['0~' + parseInt(intervals[0] * 78.12),
                parseInt(intervals[0] * 78.12) + '~' + parseInt(intervals[1] * 78.12),
                parseInt(intervals[1] * 78.12) + '~' + parseInt(intervals[2] * 78.12),
                parseInt(intervals[2] * 78.12) + '~' + parseInt(intervals[3] * 78.12),
                parseInt(intervals[3] * 78.12) + '~' + parseInt(intervals[4] * 78.12),
                parseInt(intervals[4] * 78.12) + '~' + parseInt(intervals[5] * 78.12),
                parseInt(intervals[5] * 78.12) + '~' + parseInt(intervals[6] * 78.12),
                parseInt(intervals[6] * 78.12) + '~' + parseInt(intervals[7] * 78.12),
                parseInt(intervals[7] * 78.12) + '~' + parseInt(intervals[8] * 78.12),
                parseInt(intervals[8] * 78.12) + '~' + parseInt(intervals[9] * 78.12),
                parseInt(intervals[9] * 78.12) + '~' + parseInt(intervals[10] * 78.12),
                parseInt(intervals[10] * 78.12) + '~' + parseInt(intervals[11] * 78.12),
                parseInt(intervals[11] * 78.12) + '~' + parseInt(intervals[12] * 78.12),
                parseInt(intervals[12] * 78.12) + '~' + parseInt(intervals[13] * 78.12),
                parseInt(intervals[13] * 78.12) + '~' + parseInt(intervals[14] * 78.12),
                parseInt(intervals[14] * 78.12) + '~' + parseInt(intervals[15] * 78.12),
                parseInt(intervals[15] * 78.12) + '~' + parseInt(intervals[16] * 78.12),
                parseInt(intervals[16] * 78.12) + '~' + parseInt(intervals[17] * 78.12),
                parseInt(intervals[17] * 78.12) + '~inf'
            ];
            setting.xLabel = "覆盖距离范围（米）";
            setting.setPrimaryYAxis('次', 'MR数量');
            setting.addColumnSeries([data.TaTo2, data.TaTo4, data.TaTo6, data.TaTo8, data.TaTo12,
                data.TaTo16, data.TaTo20, data.TaTo24, data.TaTo32, data.TaTo40, data.TaTo48, data.TaTo56,
                data.TaTo64, data.TaTo80, data.TaTo96, data.TaTo128, data.TaTo192, data.TaTo256, data.TaAbove256
            ], "MR数量", '次', 0);
            dom.showChartDialog(setting.title, setting.getOptions(), width, height);
        }
    });
}

function PreciseStatSet(setting) {
    this.setting = setting;
};

PreciseStatSet.prototype.generatePlot = function (url, dom, width, height) {
    var dates = [];
    var firstRates = [];
    var secondRates = [];
    var thirdRates = [];
    var mrs = [];
    var setting = this.setting;
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        success: function (data) {
            $(data).each(function (index) {
                dates.push(data[index].DateString);
                mrs.push(data[index].TotalMrs);
                firstRates.push(data[index].FirstRate);
                secondRates.push(data[index].SecondRate);
                thirdRates.push(data[index].ThirdRate);
            });
            setting.categories = dates;
            setting.xLabel = "日期";
            setting.setPrimaryYAxis('次', 'MR数量');
            setting.addColumnSeries(mrs, 'MR数量', '次', 0);
            setting.addYAxis('叠重覆盖率', '%', 2);
            setting.addLineSeries(firstRates, "第一邻区重叠覆盖率(%)", '%', 1);
            setting.addLineSeries(secondRates, "第二邻区重叠覆盖率(%)", '%', 1);
            setting.addLineSeries(thirdRates, "第三邻区重叠覆盖率(%)", '%', 1);
            dom.showChartDialog(setting.title, setting.getOptions(), width, height);
        }
    });
}