function RuInterferenceSet() {
    this.cellNames = [];
    this.victimCells = [];
    this.interferenceRatios = [];
}

RuInterferenceSet.prototype.generateData = function(topNum, url) {
    var dataSet = this;
    dataSet.cellNames = [];
    dataSet.victimCells = [];
    dataSet.interferenceRatios = [];
    $.ajax({
        type: "GET",
        dataType: "json",
        data: { topNum: topNum },
        url: url,
        async: false,
        success: function(data) {
            $(data).each(function(index) {
                dataSet.cellNames.push(data[index].CellName);
                dataSet.victimCells.push(data[index].VictimCells);
                dataSet.interferenceRatios.push(data[index].InterfernceRatio);
            });
        }
    });
};

RuInterferenceSet.prototype.getOptions = function(title) {
    var setting = new ComboSetting(title);
    setting.setPrimaryYAxis('次', '干扰小区数');
    setting.categories = this.cellNames;
    setting.xLabel = '小区名称';
    setting.addColumnSeries(this.victimCells, '干扰小区数', '次', 0);
    setting.addYAxis('干扰比例', '%', 3);
    setting.addLineSeries(this.interferenceRatios, '干扰比例', '%', 1);
    return setting.getOptions();
};

RuInterferenceSet.prototype.getTopVictimOptions = function (topNum) {
    this.generateData(topNum, "/RutraceAnalysis/GetTopVictimCells/");
    return this.getOptions('TOP干扰小区数量分析');
};

RuInterferenceSet.prototype.getTopInterfernceOptions = function (topNum) {
    this.generateData(topNum, "/RutraceAnalysis/GetTopInterfernceRatio/");
    return this.getOptions('TOP干扰比例');
};

function RtdTaSet() {
    this.cellNames = [];
    this.averageRtds = [];
    this.taAverages = [];
    this.taExcessRates = [];
}

RtdTaSet.prototype.generateData = function (topNum, url) {
    var dataSet = this;
    dataSet.cellNames = [];
    dataSet.averageRtds = [];
    dataSet.taAverages = [];
    dataSet.taExcessRates = [];
    $.ajax({
        type: "GET",
        dataType: "json",
        data: { topNum: topNum },
        url: url,
        async: false,
        success: function (data) {
            $(data).each(function (index) {
                dataSet.cellNames.push(data[index].CellName);
                dataSet.averageRtds.push(data[index].AverageRtd);
                dataSet.taAverages.push(data[index].TaAverage);
                dataSet.taExcessRates.push(data[index].TaExcessRate);
            });
        }
    });
};

RtdTaSet.prototype.getOptions = function(title) {
    var setting = new ComboSetting(title);
    setting.setPrimaryYAxis('米', '平均邻区站间距/接入距离');
    setting.categories = this.cellNames;
    setting.xLabel = '小区名称';
    setting.addColumnSeries(this.averageRtds, '平均邻区站间距', '米', 0);
    setting.addLineSeries(this.taAverages, '平均接入距离', '米', 0);
    setting.addYAxis('超远接入比例', '%', 3);
    setting.addLineSeries(this.taExcessRates, '超远接入比例', '%', 1);
    return setting.getOptions();
};

RtdTaSet.prototype.getTopAverageRtdOptions = function (topNum) {
    this.generateData(topNum, "/RutraceAnalysis/GetTopAverageRtd/");
    return this.getOptions('TOP平均邻区站间距');
};

RtdTaSet.prototype.getTopTaAverageOptions = function (topNum) {
    this.generateData(topNum, "/RutraceAnalysis/GetTopTaAverage/");
    return this.getOptions('TOP平均接入距离');
};

RtdTaSet.prototype.getTopTaExcessOptions = function (topNum) {
    this.generateData(topNum, "/RutraceAnalysis/GetTopTaExcessRate/");
    return this.getOptions('TOP超远接入比例');
};

function RuAnalysisSet() {}

RuAnalysisSet.prototype.getVictimCellsAndInterferenceRatio = function () {
    var dataValue = [];
    $.ajax({
        type: "GET", //提交的类型  
        dataType: "json",
        url: "/RutraceAnalysis/GetVictimCellsAndInterferenceRatio/", //提交地址   
        async: false,
        success: function (data) {
            $(data).each(function (index) {
                dataValue.push([data[index].X, data[index].Y]);
            });
        }
    });
    return dataValue;
};

RuAnalysisSet.prototype.getInterferenceRatioToVictimCellsLine = function (factor) {
    var dataValue = [];
    $.ajax({
        type: "GET", //提交的类型  
        dataType: "json",
        data: { factor: factor },
        url: "/RutraceAnalysis/GetInterferenceRatioToVictimCellsLine/", //提交地址   
        async: false,
        success: function (data) {
            $(data).each(function (index) {
                dataValue.push([data[index].X, data[index].Y]);
            });
        }
    });
    return dataValue;
};

RuAnalysisSet.prototype.getInterferenceOptions = function() {
    var dataValue = this.getVictimCellsAndInterferenceRatio();
    var dataLine1 = this.getInterferenceRatioToVictimCellsLine(1);
    var dataLine2 = this.getInterferenceRatioToVictimCellsLine(2);
    var dataLine4 = this.getInterferenceRatioToVictimCellsLine(4);
    var dataLine8 = this.getInterferenceRatioToVictimCellsLine(8);
    var setting = new ComboSetting('干扰小区数和干扰比例联合分析');
    setting.setPrimaryYAxis('%', '干扰比例');
    setting.xLabel = '干扰小区数';
    setting.addRegressionLine('干扰比例（k=1曲线）', dataLine1);
    setting.addRegressionLine('干扰比例（k=2曲线）', dataLine2);
    setting.addRegressionLine('干扰比例（k=4曲线）', dataLine4);
    setting.addRegressionLine('干扰比例（k=8曲线）', dataLine8);
    setting.addScatterPoints('干扰比例（实测数据）', dataValue);
    return setting.getSimpleOptions();
};

RuAnalysisSet.prototype.getAverageTaAndRtd = function () {
    var dataValue = [];
    $.ajax({
        type: "GET", //提交的类型  
        dataType: "json",
        url: "/RutraceAnalysis/GetAverageTaAndRtd/", //提交地址   
        async: false,
        success: function (data) {
            $(data).each(function (index) {
                dataValue.push([data[index].X, data[index].Y]);
            });
        }
    });
    return dataValue;
};

RuAnalysisSet.prototype.getAverageRtdToTaLine = function (factor) {
    var dataValue = [];
    $.ajax({
        type: "GET", //提交的类型  
        dataType: "json",
        data: { factor: factor },
        url: "/RutraceAnalysis/GetAverageRtdToTaLine/", //提交地址   
        async: false,
        success: function (data) {
            $(data).each(function (index) {
                dataValue.push([data[index].X, data[index].Y]);
            });
        }
    });
    return dataValue;
};

RuAnalysisSet.prototype.getTaDistanceRing = function () {
    var dataValue = [];
    $.ajax({
        type: "GET", //提交的类型  
        dataType: "json",
        url: "/RutraceAnalysis/GetTaDistanceRing/", //提交地址   
        async: false,
        success: function (data) {
            $(data).each(function (index) {
                dataValue.push([data[index].X, data[index].Y]);
            });
        }
    });
    return dataValue;
};

RuAnalysisSet.prototype.getRtdTaOptions= function() {
    var dataValue = this.getAverageTaAndRtd();
    var dataLine1 = this.getAverageRtdToTaLine(1);
    var dataLine2 = this.getAverageRtdToTaLine(2);
    var dataLine4 = this.getAverageRtdToTaLine(4);
    var dataLine8 = this.getAverageRtdToTaLine(8);
    var dataRing = this.getTaDistanceRing();
    var setting = new ComboSetting('平均站间距和平均覆盖距离联合分析');
    setting.setPrimaryYAxis('米', '平均站间距');
    setting.xLabel = '平均覆盖距离（米）';
    setting.addRegressionLine('平均站间距（k=1曲线）', dataLine1);
    setting.addRegressionLine('平均站间距（k=2曲线）', dataLine2);
    setting.addRegressionLine('平均站间距（k=4曲线）', dataLine4);
    setting.addRegressionLine('平均站间距（k=8曲线）', dataLine8);
    setting.addRegressionLine('正常站间距范围', dataRing);
    setting.addScatterPoints('干扰比例（实测数据）', dataValue);
    return setting.getSimpleOptions();
}

RuAnalysisSet.prototype.getAverageTaAndExcessRate = function () {
    var dataValue = [];
    $.ajax({
        type: "GET", //提交的类型  
        dataType: "json",
        url: "/RutraceAnalysis/GetAverageTaAndExcessRate/", //提交地址   
        async: false,
        success: function (data) {
            $(data).each(function (index) {
                dataValue.push([data[index].X, data[index].Y]);
            });
        }
    });
    return dataValue;
};

RuAnalysisSet.prototype.getTaExcessOptions= function() {
    var dataValue = this.getAverageTaAndExcessRate();
    var setting = new ComboSetting('平均覆盖距离和超远覆盖比例');
    setting.setPrimaryYAxis('%', '超远覆盖比例');
    setting.xLabel = '平均覆盖距离（米）';
    setting.addScatterPoints('平均覆盖距离和超远覆盖比例', dataValue);
    return setting.getSimpleOptions();
}