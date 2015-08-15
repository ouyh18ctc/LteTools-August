function ComboSetting(title) {
    this.title = title;
    this.categories = [];
    this.yAxis = [{ // Primary yAxis
        labels: {
            format: '{value}',
            style: {
                color: Highcharts.getOptions().colors[0]
            }
        },
        title: {
            text: 'YLabel',
            style: {
                color: Highcharts.getOptions().colors[0]
            }
        }

    }];
    this.series = [];
    this.xAxisStep = 1;
    this.legend = {
        layout: 'vertical',
        align: 'left',
        x: 100,
        verticalAlign: 'top',
        y: 30,
        floating: true,
        backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
    };
    this.xLabel = 'XLabel';
}

ComboSetting.prototype.setLegendRight = function() {
    this.legend = {
        layout: 'vertical',
        align: 'right',
        x: -100,
        verticalAlign: 'top',
        y: 30,
        floating: true,
        backgroundColor: (Highcharts.theme && Highcharts.theme.legendBackgroundColor) || '#FFFFFF'
    };
};

ComboSetting.prototype.setPrimaryYAxis = function (unit, yLabel, min, max) {
    this.yAxis[0].labels.format = '{value} ' + unit;
    this.yAxis[0].title.text = yLabel;
    if (min != undefined) this.yAxis[0].min = min;
    if (max != undefined) this.yAxis[0].max = max;
};

ComboSetting.prototype.addYAxis = function (title, unit, colorIndex) {
    this.yAxis.push({ // Secondary yAxis
        gridLineWidth: 0,
        title: {
            text: title,
            style: {
                color: Highcharts.getOptions().colors[colorIndex]
            }
        },
        labels: {
            format: '{value} ' + unit,
            style: {
                color: Highcharts.getOptions().colors[colorIndex]
            }
        },
        opposite: true

    });
};

ComboSetting.prototype.addYAxisWithRange = function (title, unit, colorIndex, min, max) {
    this.yAxis.push({ // Secondary yAxis
        gridLineWidth: 0,
        title: {
            text: title,
            style: {
                color: Highcharts.getOptions().colors[colorIndex]
            }
        },
        labels: {
            format: '{value} ' + unit,
            style: {
                color: Highcharts.getOptions().colors[colorIndex]
            }
        },
        opposite: true,
        min: min,
        max: max

    });
};

ComboSetting.prototype.addSeries = function (data, name, type, unit, yAxis) {
    this.series.push({
        name: name,
        type: type,
        yAxis: yAxis,
        data: data,
        tooltip: {
            valueSuffix: ' ' + unit
        }

    });
};

ComboSetting.prototype.addAreaSeries = function(data, name, unit, yAxis) {
    this.addSeries(data, name, 'area', unit, yAxis);
};

ComboSetting.prototype.addColumnSeries = function (data, name, unit, yAxis) {
    this.addSeries(data, name, 'column', unit, yAxis);
};

ComboSetting.prototype.addLineSeries = function (data, name, unit, yAxis) {
    this.addSeries(data, name, 'line', unit, yAxis);
};

ComboSetting.prototype.addSplineSeries = function (data, name, unit, yAxis) {
    this.addSeries(data, name, 'spline', unit, yAxis);
};

ComboSetting.prototype.getOptions = function () {
    return {
        chart: {
            zoomType: 'xy'
        },
        title: {
            text: this.title
        },
        xAxis: [{
            categories: this.categories,
            labels:{ 
                step: this.xAxisStep 
            },
            title: {
                text: this.xLabel,
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            }
        }],
        yAxis: this.yAxis,
        tooltip: {
            shared: true
        },
        legend: this.legend,
        series: this.series
    };
};

ComboSetting.prototype.getSimpleOptions = function() {
    return {
        xAxis: {
            title: {
                text: this.xLabel,
                style: {
                    color: Highcharts.getOptions().colors[0]
                }
            }
        },
        yAxis: this.yAxis,
        title: {
            text: this.title
        },
        legend: this.legend,
        series: this.series
    };
};

ComboSetting.prototype.addRegressionLine = function(title, data) {
    this.series.push({
        type: 'line',
        name: title,
        data: data,
        marker: {
            enabled: false
        },
        states: {
            hover: {
                lineWidth: 0
            }
        },
        enableMouseTracking: false
    });
};

ComboSetting.prototype.addScatterPoints= function(title, data) {
    this.series.push({
        type: 'scatter',
        name: title,
        data: data,
        marker: {
            radius: 4
        }
    });
}