function RateAnalysisSet() { }

RateAnalysisSet.prototype.getValuePdschRb = function () {
    var dataValuePdschRb = [];
    $.ajax({
        type: "GET", //提交的类型  
        dataType: "json",
        url: "/RateStatChart/GetPdschRbTimeSeries/", //提交地址  
        async: false,
        success: function (data) {
            $(data).each(function (index, value) {
                dataValuePdschRb.push([data[index].Time, data[index].RB]);
            });
        }
    });
    return dataValuePdschRb;
};

RateAnalysisSet.prototype.getValueFrequencyEfficiency = function () {
    var dataValueFrequencyEfficiency = [];
    $.ajax({
        type: "GET", //提交的类型  
        dataType: "json",
        url: "/RateStatChart/GetDlFrequencyEfficiencyTimeSeries/", //提交地址   
        async: false,  
        success: function (data) {
            $(data).each(function (index, value) {
                dataValueFrequencyEfficiency.push([data[index].Time, data[index].FE]);
            });
        }
    });
    return dataValueFrequencyEfficiency;
};

RateAnalysisSet.prototype.getValueDlThroughput = function () {
    var dataValueDlThroughput = [];
    $.ajax({
        type: "GET", //提交的类型  
        dataType: "json",
        url: "/RateStatChart/GetDlPdcpThroughputTimeSeries/", //提交地址     
        async: false,
        success: function (data) {
            $(data).each(function (index, value) {
                dataValueDlThroughput.push([data[index].Time, data[index].TP]);
            });
        }
    });
    return dataValueDlThroughput;
};

RateAnalysisSet.prototype.getValueRsrp = function () {
    var dataValueRsrp = [];
    $.ajax({
        type: "GET", //提交的类型  
        dataType: "json",
        url: "/RateStatChart/GetRsrpTimeSeries/", //提交地址     
        async: false,
        success: function (data) {
            $(data).each(function (index, value) {
                dataValueRsrp.push([data[index].Time, data[index].Rsrp]);
            });
        }
    });
    return dataValueRsrp;
};

RateAnalysisSet.prototype.getValueSinr = function () {
    var dataValueSinr = [];
    $.ajax({
        type: "GET", //提交的类型  
        dataType: "json",
        url: "/RateStatChart/GetSinrTimeSeries/", //提交地址     
        async: false,
        success: function (data) {
            $(data).each(function (index, value) {
                dataValueSinr.push([data[index].Time, data[index].Sinr]);
            });
        }
    });
    return dataValueSinr;
};

RateAnalysisSet.prototype.getValueLowSinr2FE = function () {
    var dataValueLowSinr2FE = [];
    $.ajax({
        type: "GET", //提交的类型  
        dataType: "json",
        url: "/RateStatChart/GetLowRbRateSinrToFrequencyEfficiency/", //提交地址    
        async: false, 
        success: function (data) {
            $(data).each(function (index, value) {
                dataValueLowSinr2FE.push([data[index].Sinr, data[index].FE]);
            });
        }
    });
    return dataValueLowSinr2FE;
};

RateAnalysisSet.prototype.getValueHighSinr2FE = function () {
    var dataValueHighSinr2FE = [];
    $.ajax({
        type: "GET", //提交的类型  
        dataType: "json",
        url: "/RateStatChart/GetHighRbRateSinrToFrequencyEfficiency/", //提交地址     
        async: false,
        success: function (data) {
            $(data).each(function (index, value) {
                dataValueHighSinr2FE.push([data[index].Sinr, data[index].FE]);
            });
        }
    });
    return dataValueHighSinr2FE;
};

RateAnalysisSet.prototype.getValueTheorySinr2FE = function () {
    var dataValueTheorySinr2FE = [];
    $.ajax({
        type: "GET", //提交的类型  
        dataType: "json",
        url: "/RateStatChart/GetTheorySinrToFrequencyEfficiency/", //提交地址     
        async: false,
        success: function (data) {
            $(data).each(function (index, value) {
                dataValueTheorySinr2FE.push([data[index].Sinr, data[index].FE]);
            });
        }
    });
    return dataValueTheorySinr2FE;
};

RateAnalysisSet.prototype.getValueLowRsrp2FE = function () {
    var dataValueLowRsrp2FE = [];
    $.ajax({
        type: "GET", //提交的类型  
        dataType: "json",
        url: "/RateStatChart/GetLowRbRateRsrpToFrequencyEfficiency/", //提交地址  
        async: false,   
        success: function (data) {
            $(data).each(function (index, value) {
                dataValueLowRsrp2FE.push([data[index].Rsrp, data[index].FE]);
            });
        }
    });
    return dataValueLowRsrp2FE;
};

RateAnalysisSet.prototype.getValueHighRsrp2FE = function () {
    var dataValueHighRsrp2FE = [];
    $.ajax({
        type: "GET", //提交的类型  
        dataType: "json",
        url: "/RateStatChart/GetHighRbRateRsrpToFrequencyEfficiency/", //提交地址     
        async: false,
        success: function (data) {
            $(data).each(function (index, value) {
                dataValueHighRsrp2FE.push([data[index].Rsrp, data[index].FE]);
            });
        }
    });
    return dataValueHighRsrp2FE;
};