function TopHistorySet(cellInfo, end) {
    this.infos = cellInfo.split(';');
    this.cellName = this.infos[1];
    this.cellId = this.infos[2];
    this.sectorId = this.infos[3];
    this.end = end;
}

TopHistorySet.prototype.get2GOptions = function(url, kpiName, statSet) {
    var frequency = this.infos[4];
    var setting = new ComboSetting(this.cellName + '-' + this.sectorId + '-' + frequency + kpiName);
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        data: {
            cellId: this.cellId,
            sectorId: this.sectorId,
            frequency: frequency,
            end: this.end
        },
        async: false,
        success: function (result) {
            statSet.setComboSetting(setting, result);
        }
    });
    return setting.getOptions();
};

TopHistorySet.prototype.getTopDrop2GOptions = function () {
    var statSet = new TopDrop2GDataSet();
    return this.get2GOptions("/TopDrop2G/QueryHistory", '掉话变化趋势', statSet);
};

TopHistorySet.prototype.getTopDrop2GDailyOptions = function () {
    var statSet = new TopDrop2GDailySet();
    return this.get2GOptions("/TopDrop2G/QueryHistoryDaily", '掉话变化趋势', statSet);
};

TopHistorySet.prototype.getTop2GDistanceDistribution = function () {
    var statSet = new TopDrop2GDistanceSet();
    return this.get2GOptions("/TopDrop2G/QueryDistanceDistribution", '按距离指标变化趋势', statSet);
};

TopHistorySet.prototype.getTop2GDropsHourDistribution = function () {
    var statSet = new Top2GDropsHourSet();
    return this.get2GOptions("/TopDrop2G/QueryDropsHourDistribution", '掉话次数小时变化趋势', statSet);
};

TopHistorySet.prototype.getCoverageInterferenceDistribution = function () {
    var statSet = new CoverageInterferenceHourSet();
    return this.get2GOptions("/TopDrop2G/QueryCoverageInterferenceDistribution", '覆盖干扰指标小时变化趋势', statSet);
};

TopHistorySet.prototype.getAlarmHourDistribution= function() {
    var statSet = new AlarmHourSet();
    return this.get2GOptions("/TopDrop2G/QueryAlarmHourDistribution", '告警指标小时变化趋势', statSet);
}

TopHistorySet.prototype.getTopConnection3GOptions = function () {
    var setting = new ComboSetting(this.cellName + '-' + this.sectorId + '-' + '连接成功率变化趋势');
    var dataSet = new TopConnection3GSet();

    $.ajax({
        url: "/TopConnection3G/QueryHistory",
        type: "GET",
        dataType: "json",
        data: {
            cellId: this.cellId,
            sectorId: this.sectorId,
            end: this.end
        },
        async: false,
        success: function (result) {
            dataSet.setComboSetting(setting, result);
        }
    });
    setting.xAxisStep = 8;
    return setting.getOptions();
};

function TopDrop2GDataSet() {
    this.dates = [];
    this.drops = [];
    this.mocalls = [];
    this.mtcalls = [];
    this.droprate = [];
}

TopDrop2GDataSet.prototype.setComboSetting = function (setting, result) {
    var dataSet = this;
    if (result.length > 0) {
        $(result).each(function(index) {
            dataSet.dates.push(result[index].StatDate);
            dataSet.drops.push(result[index].Drops);
            dataSet.mocalls.push(result[index].MoCalls);
            dataSet.mtcalls.push(result[index].MtCalls);
            dataSet.droprate.push(result[index].DropRate);
        });
        setting.categories = this.dates;
        setting.xLabel = '日期';
        setting.setPrimaryYAxis('次', '掉话次数');
        setting.addColumnSeries(this.drops, '掉话次数', '次', 0);
        setting.addYAxis('呼叫次数', '次', 2);
        setting.addSplineSeries(this.mocalls, '主叫呼叫次数', '次', 1);
        setting.addSplineSeries(this.mtcalls, '被叫呼叫次数', '次', 1);
        setting.addYAxis('掉话率', '%', 3);
        setting.addLineSeries(this.droprate, '掉话率', '%', 2);
    }
};

function TopDrop2GDailySet() {
    this.dates = [];
    this.drops = [];
    this.distance = [];
    this.rssi = [];
    this.droprate = [];
}

TopDrop2GDailySet.prototype.setComboSetting = function (setting, result) {
    var dataSet = this;
    if (result.length > 0) {
        $(result).each(function(index) {
            dataSet.dates.push(result[index].StatDate);
            dataSet.drops.push(result[index].Drops);
            dataSet.distance.push(result[index].DropDistance);
            dataSet.rssi.push(result[index].DropRssi);
            dataSet.droprate.push(result[index].DropRate);
        });
        setting.categories = this.dates;
        setting.xLabel = '日期';
        setting.setPrimaryYAxis('次', '掉话次数');
        setting.addColumnSeries(this.drops, '掉话次数', '次', 0);
        setting.addYAxis('掉话距离', '米', 2);
        setting.addSplineSeries(this.distance, '掉话距离', '米', 1);
        setting.addYAxis('掉话平均RSSI', 'dBm', 4);
        setting.addSplineSeries(this.rssi, '掉话平均RSSI', 'dBm', 2);
        setting.addYAxis('掉话率', '%', 3);
        setting.addLineSeries(this.droprate, '掉话率', '%', 3);
    }
};

function TopDrop2GDistanceSet() {
    this.distances = [];
    this.cdrcalls = [];
    this.cdrdrops = [];
    this.dropecio = [];
    this.goodecio = [];
}

TopDrop2GDistanceSet.prototype.setComboSetting = function (setting, result) {
    var dataSet = this;
    if (result.length > 0) {
        $(result).each(function(index) {
            dataSet.distances.push(result[index].DistanceDescription);
            dataSet.cdrcalls.push(result[index].CdrCalls);
            dataSet.cdrdrops.push(result[index].CdrDrops);
            dataSet.dropecio.push(result[index].DropEcio);
            dataSet.goodecio.push(result[index].GoodEcio);
        });
        setting.categories = this.distances;
        setting.xLabel = '距离区间';
        setting.setPrimaryYAxis('次', '呼叫次数');
        setting.addColumnSeries(this.cdrcalls, '呼叫次数', '次', 0);
        setting.addYAxis('掉话次数', '次', 2);
        setting.addSplineSeries(this.cdrdrops, '掉话次数', '次', 1);
        setting.addYAxis('掉话平均EcIo', 'dB', 4);
        setting.addSplineSeries(this.dropecio, '平均EcIo', 'dB', 2);
        setting.addYAxis('EcIo优良率', '%', 3);
        setting.addLineSeries(this.goodecio, 'EcIo优良率', '%', 3);
    }
};

function Top2GDropsHourSet() {
    this.hours = [];
    this.cdrcalls = [];
    this.cdrdrops = [];
    this.erasuredrops = [];
    this.kpicalls = [];
    this.kpidrops = [];
}

Top2GDropsHourSet.prototype.setComboSetting = function (setting, result) {
    var dataSet = this;
    if (result.length > 0) {
        $(result).each(function (index) {
            dataSet.hours.push(result[index].Hour);
            dataSet.cdrcalls.push(result[index].CdrCalls);
            dataSet.cdrdrops.push(result[index].CdrDrops);
            dataSet.erasuredrops.push(result[index].ErasureDrops);
            dataSet.kpicalls.push(result[index].KpiCalls);
            dataSet.kpidrops.push(result[index].KpiDrops);
        });
        setting.categories = this.hours;
        setting.xLabel = '时段';
        setting.setPrimaryYAxis('次', '呼叫次数');
        setting.addColumnSeries(this.cdrcalls, 'CDR呼叫次数', '次', 0);
        setting.addYAxis('掉话次数', '次', 2);
        setting.addSplineSeries(this.cdrdrops, 'CDR掉话次数', '次', 1);
        setting.addSplineSeries(this.erasuredrops, 'Erasure掉话次数', '次', 1);
        setting.addColumnSeries(this.kpicalls, 'KPI呼叫次数', '次', 0);
        setting.addSplineSeries(this.kpidrops, 'KPI掉话次数', '次', 1);
    }
};

function CoverageInterferenceHourSet( ) {
    this.hours = [];
    this.dropecio = [];
    this.mainrssi = [];
    this.subrssi = [];
}

CoverageInterferenceHourSet.prototype.setComboSetting = function(setting, result) {
    var dataSet = this;
    if (result.length > 0) {
        $(result).each(function (index) {
            dataSet.hours.push(result[index].Hour);
            dataSet.dropecio.push(result[index].DropEcio);
            dataSet.mainrssi.push(result[index].MainRssi);
            dataSet.subrssi.push(result[index].SubRssi);
        });
        setting.categories = this.hours;
        setting.xLabel = '时段';
        setting.setPrimaryYAxis('dBm', '掉话Ec/Io');
        setting.addColumnSeries(this.dropecio, '掉话Ec/Io', 'dBm', 0);
        setting.addYAxis('RSSI', 'dBm', 2);
        setting.addSplineSeries(this.mainrssi, '主集RSSI', 'dBm', 1);
        setting.addSplineSeries(this.subrssi, '分集RSSI', 'dBm', 1);
    }
};

function AlarmHourSet() {
    this.hours = [0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23];
}

AlarmHourSet.prototype.setComboSetting= function(setting, result) {
    if (result.length > 0) {
        setting.categories = this.hours;
        setting.xLabel = '时段';
        setting.setPrimaryYAxis('次', '告警数量');
        $(result).each(function(index) {
            setting.addLineSeries(result[index].Data, result[index].Type, '次', 0);
        });
    }
}

function TopConnection3GSet() {
    this.dates = [];
    this.connectionfails = [];
    this.connectionrate = [];
    this.linkbusyrate = [];
    this.droprate = [];
}

TopConnection3GSet.prototype.setComboSetting = function (setting, result) {
    var dataSet = this;
    if (result.length > 0) {
        $(result).each(function(index) {
            dataSet.dates.push(result[index].StatDate);
            dataSet.connectionfails.push(result[index].ConnectionFails);
            dataSet.connectionrate.push(result[index].ConnectionRate);
            dataSet.linkbusyrate.push(result[index].LinkBusyRate);
            dataSet.droprate.push(result[index].DropRate);
        });
        setting.categories = this.dates;
        setting.xLabel = '日期';
        setting.xLabel = '日期';
        setting.setPrimaryYAxis('次', '连接失败次数');
        setting.addColumnSeries(this.connectionfails, '连接失败次数', '次', 0);
        setting.addYAxis('连接成功率/链路繁忙率', '%', 2);
        setting.addSplineSeries(this.connectionrate, '连接成功率', '%', 1);
        setting.addSplineSeries(this.linkbusyrate, '链路繁忙率', '%', 1);
        setting.addYAxis('掉线率', '%', 3);
        setting.addLineSeries(this.droprate, '掉线率', '%', 2);
    }
};
