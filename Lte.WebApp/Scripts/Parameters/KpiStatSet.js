function KpiStatSet(setting) {
    this.setting = setting;
}

KpiStatSet.prototype.getRegionStat = function(city, region, url) {
    var result = [];
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        async: false,
        data: {
            city: city,
            region: region
        },
        success: function (data) {
            result = data;
        }
    });
    return result;
};

KpiStatSet.prototype.getTownStat = function (district, town, url) {
    var result = [];
    $.ajax({
        url: url + district + "/" + town,
        type: "GET",
        dataType: "json",
        async: false,
        success: function (data) {
            result = data;
        }
    });
    return result;
};

KpiStatSet.prototype.getRegionErlang2GStat = function (city, region) {
    return KpiStatSet.prototype.getRegionStat(city, region, "/KpiStat/GetRegionErlang2GSeries");
};

KpiStatSet.prototype.getRegionDrop2GRateStat = function (city, region) {
    return KpiStatSet.prototype.getRegionStat(city, region, "/KpiStat/GetRegionDrop2GRateSeries");
};

KpiStatSet.prototype.getRegionEcio2GStat = function (city, region) {
    return KpiStatSet.prototype.getRegionStat(city, region, "/KpiStat/GetRegionEcio2GSeries");
};

KpiStatSet.prototype.getRegionFlow3GStat = function (city, region) {
    return KpiStatSet.prototype.getRegionStat(city, region, "/KpiStat/GetRegionFlow3GSeries");
};

KpiStatSet.prototype.getRegionDownSwitchRateStat = function (city, region) {
    return KpiStatSet.prototype.getRegionStat(city, region, "/KpiStat/GetRegionDownSwitchRateSeries");
};

KpiStatSet.prototype.getRegionCi3GStat = function (city, region) {
    return KpiStatSet.prototype.getRegionStat(city, region, "/KpiStat/GetRegionCi3GSeries");
};

KpiStatSet.prototype.getTownTotalMrStat = function(district, town) {
    return KpiStatSet.prototype.getTownStat(district, town, "/api/QueryLteMrs/");
};

KpiStatSet.prototype.getTownPreciseRateStat= function(district, town) {
    return KpiStatSet.prototype.getTownStat(district, town, "/api/QueryPreciseRates/");
}

KpiStatSet.prototype.generateRegionSeries = function(city, getStatMethod, setting, unit) {
    $.ajax({
        url: "/KpiStat/GetRegionList",
        type: "GET",
        dataType: "json",
        async: false,
        data: {
            city: city
        },
        success: function (regions) {
            $(regions).each(function (index) {
                var stat = getStatMethod(city, regions[index]);
                setting.addLineSeries(stat, regions[index], unit, 0);
            });
        }
    });
};

KpiStatSet.prototype.generateTownSeries = function (district, getStatMethod, setting, unit) {
    $.ajax({
        url: "/api/QueryLteKpiTownList/" + district,
        type: "GET",
        dataType: "json",
        async: false,
        success: function (regions) {
            $(regions).each(function (index) {
                var stat = getStatMethod(district, regions[index]);
                setting.addLineSeries(stat, regions[index], unit, 0);
            });
        }
    });
};

KpiStatSet.prototype.generateRegionErlang2GSeries = function (city) {
    this.generateRegionSeries(city, this.getRegionErlang2GStat, this.setting, 'Erl');
};

KpiStatSet.prototype.generateRegionDrop2GRateSeries = function (city) {
    this.generateRegionSeries(city, this.getRegionDrop2GRateStat, this.setting, '%');
};

KpiStatSet.prototype.generateRegionEcio2GSeries = function (city) {
    this.generateRegionSeries(city, this.getRegionEcio2GStat, this.setting, '%');
};

KpiStatSet.prototype.generateRegionFlow3GSeries = function(city) {
    this.generateRegionSeries(city, this.getRegionFlow3GStat, this.setting, 'GB');
}

KpiStatSet.prototype.generateRegionDownSwitchRateSeries = function (city) {
    this.generateRegionSeries(city, this.getRegionDownSwitchRateStat, this.setting, 'MB');
};

KpiStatSet.prototype.generateRegionCi3GSeries = function (city) {
    this.generateRegionSeries(city, this.getRegionCi3GStat, this.setting, '%');
};

KpiStatSet.prototype.generateTownTotalMrSeries= function(district) {
    this.generateTownSeries(district, this.getTownTotalMrStat, this.setting, '次');
}

KpiStatSet.prototype.generateTownPreciseRateSeries = function (district) {
    this.generateTownSeries(district, this.getTownPreciseRateStat, this.setting, '%');
}

KpiStatSet.prototype.generateCitySeries = function(city, data, url, unit, title) {
    var setting = this.setting;
    setting.categories = data;
    setting.xLabel = '日期';
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        async: false,
        data: {
            city: city
        },
        success: function (seriesData) {
            setting.setPrimaryYAxis(unit, title);
            setting.addColumnSeries(seriesData, city, unit, 0);
        }
    });
};

KpiStatSet.prototype.generateDistrictSeries = function (district, data, url, unit, title) {
    var setting = this.setting;
    setting.categories = data;
    setting.xLabel = '日期';
    $.ajax({
        url: url + district,
        type: "GET",
        dataType: "json",
        async: false,
        success: function (seriesData) {
            setting.setPrimaryYAxis(unit, title);
            setting.addColumnSeries(seriesData, district, unit, 0);
        }
    });
};

KpiStatSet.prototype.generateDistrictSeriesWithRange = function (district, data, url, unit, title) {
    var setting = this.setting;
    setting.categories = data;
    setting.xLabel = '日期';
    $.ajax({
        url: url + district,
        type: "GET",
        dataType: "json",
        async: false,
        success: function (seriesData) {
            var min = Math.min.apply(null, seriesData) * 0.95;
            var max = Math.max.apply(null, seriesData) * 1.03;
            setting.setPrimaryYAxis(unit, title, min, max);
            setting.addColumnSeries(seriesData, district, unit, 0);
        }
    });
};

KpiStatSet.prototype.generateChart = function(city, dates, chartTapPrefix, kpiName) {
    switch (kpiName) {
        case '2G话务量':
            this.generateErlang2GChart(city, dates, chartTapPrefix);
            break;
        case '2G掉话率':
            this.generateDrop2GRateChart(city, dates, chartTapPrefix);
            break;
        case 'Ec/Io优良比':
            this.generateEcio2GChart(city, dates, chartTapPrefix);
            break;
        case '3G流量':
            this.generateFlow3GChart(city, dates, chartTapPrefix);
            break;
        case '3G下切2G流量比':
            this.generateDownSwitchRateChart(city, dates, chartTapPrefix);
            break;
        case 'C/I优良比':
            this.generateCi3GChart(city, dates, chartTapPrefix);
            break;
        case 'MR数量':
            this.generateTotalMrChart(city, dates, chartTapPrefix);
            break;
        case '精确覆盖率':
            this.generatePreciseRateChart(city, dates, chartTapPrefix);
            break;
    }
};

KpiStatSet.prototype.generateErlang2GChart = function (city, data, chartTapPrefix) {
    if (data.length > 0) {
        this.generateCitySeries(city, data, "/KpiStat/GetCityErlang2GSeries", 'Erl', '话务量（不含切换）');
        this.generateRegionErlang2GSeries(city);
        chartTapPrefix.highcharts(this.setting.getOptions());
    }
};

KpiStatSet.prototype.generateDrop2GRateChart = function (city, data, chartTapPrefix, kpiName) {
    if (data.length > 0) {
        this.generateCitySeries(city, data, "/KpiStat/GetCityDrop2GRateSeries", '%', kpiName);
        this.generateRegionDrop2GRateSeries(city);
        chartTapPrefix.highcharts(this.setting.getOptions());
    }
};

KpiStatSet.prototype.generateEcio2GChart = function (city, data, chartTapPrefix, kpiName) {
    if (data.length > 0) {
        this.generateCitySeries(city, data, "/KpiStat/GetCityEcio2GSeries", '%', kpiName);
        this.generateRegionEcio2GSeries(city);
        chartTapPrefix.highcharts(this.setting.getOptions());
    }
};

KpiStatSet.prototype.generateFlow3GChart = function (city, data, chartTapPrefix, kpiName) {
    if (data.length > 0) {
        this.generateCitySeries(city, data, "/KpiStat/GetCityFlow3GSeries", 'GB', kpiName);
        this.generateRegionFlow3GSeries(city);
        chartTapPrefix.highcharts(this.setting.getOptions());
    }
};

KpiStatSet.prototype.generateDownSwitchRateChart = function (city, data, chartTapPrefix, kpiName) {
    if (data.length > 0) {
        this.generateCitySeries(city, data, "/KpiStat/GetCityDownSwitchRateSeries", 'MB', kpiName);
        this.generateRegionDownSwitchRateSeries(city);
        chartTapPrefix.highcharts(this.setting.getOptions());
    }
};

KpiStatSet.prototype.generateCi3GChart = function (city, data, chartTapPrefix, kpiName) {
    if (data.length > 0) {
        this.generateCitySeries(city, data, "/KpiStat/GetCityCi3GSeries", '%', kpiName);
        this.generateRegionCi3GSeries(city);
        chartTapPrefix.highcharts(this.setting.getOptions());
    }
};

KpiStatSet.prototype.generateTotalMrChart = function(district, data, chartTapPrefix, kpiName) {
    if (data.length > 0) {
        this.generateDistrictSeries(district, data, "/api/QueryLteMrs/", '次', kpiName);
        this.generateTownTotalMrSeries(district);
        chartTapPrefix.highcharts(this.setting.getOptions());
    }
};

KpiStatSet.prototype.generatePreciseRateChart = function (district, data, chartTapPrefix, kpiName) {
    if (data.length > 0) {
        this.generateDistrictSeriesWithRange(district, data, "/api/QueryPreciseRates/", '%', kpiName);
        this.generateTownPreciseRateSeries(district);
        chartTapPrefix.highcharts(this.setting.getOptions());
    }
};