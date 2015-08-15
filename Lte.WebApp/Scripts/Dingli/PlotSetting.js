function PlotSetting(plotTitle, xlabel) {
    this.plotTitle = plotTitle;
    this.series = new Array();
    this.axes = {
        xaxis: { label: xlabel }
    };
}

PlotSetting.prototype.setScatterDef = function (scatterDef) {
    var scanLength = Math.min(this.series.length, scatterDef.length);
    for (var i = 0; i < scanLength; i++) {
        if (scatterDef[i]) {
            this.series[i].showLine = false;
            this.series[i].showMarker = true;
        }
    }
};

PlotSetting.prototype.addTrendLines = function () {
    for (var i = 0; i < this.series.length; i++) {
        if (this.series[i].showLine == false && this.series[i].showMarker == true) {
            this.series[i].trendline = { show: true };
        }
    }
};

PlotSetting.prototype.setYAxisRange = function (i, min, max) {
    if (i == 0) {
        this.axes.yaxis.min = min;
        this.axes.yaxis.max = max;
    } else if (i == 1) {
        this.axes.y2axis.min = min;
        this.axes.y2axis.max = max;
    } else if (i == 2) {
        this.axes.y3axis.min = min;
        this.axes.y3axis.max = max;
    }
};

PlotSetting.prototype.pushSeries = function (ylabels) {
    this.series = [];
    for (var i = 0; i < ylabels.length; i++) {
        this.series.push({ label: ylabels[i] });
        if (i == 0) {
            this.series[0].yaxis = 'yaxis';
            this.axes.yaxis = { label: ylabels[0],
                labelRenderer: $.jqplot.CanvasAxisLabelRenderer
            };
        } else if (i == 1) {
            this.series[1].yaxis = 'y2axis';
            this.axes.y2axis = { label: ylabels[1],
                labelRenderer: $.jqplot.CanvasAxisLabelRenderer
            };
        } else if (i == 2) {
            this.series[2].yaxis = 'y3axis';
            this.axes.y3axis = { label: ylabels[2],
                labelRenderer: $.jqplot.CanvasAxisLabelRenderer
            };
        } else {
            this.series[i].yaxis = 'y3axis';
        }
    }
};

PlotSetting.prototype.setYlabel = function (yaxisLabel) {
    var labelLength = this.series.length;
    if (labelLength > 0) {
        this.axes.yaxis.label = yaxisLabel;
        for (var i = 1; i < labelLength; i++) {
            this.series[i].yaxis = 'yaxis';
        }
    }
};

PlotSetting.prototype.getOptions = function () {
    return {
        title: this.plotTitle,
        seriesDefaults: {
            showMarker: false,
            rendererOptions: {
                smooth: true,
                animation: { show: true }
            }
        },
        series: this.series,
        legend: {
            show: true
        },
        axes: this.axes,
        highlighter: {
            show: true,
            sizeAdjust: 7.5
        }
    };
};