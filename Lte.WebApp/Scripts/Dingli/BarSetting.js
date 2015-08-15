function BarSetting(plotTitle) {
    this.plotTitle = plotTitle;
}

BarSetting.prototype.getOptions = function () {
    return {
        title: this.plotTitle,
        seriesDefaults: {
            pointLabels: { show: true },
            animation: { show: true }
        },
        series: [{ renderer: $.jqplot.BarRenderer}],
        axesDefaults: {
            tickRenderer: $.jqplot.CanvasAxisTickRenderer
        },
        axes: {
            xaxis: {
                renderer: $.jqplot.CategoryAxisRenderer,
                tickOptions: {
                    angle: -90,
                    fontSize: '10pt'
                }
            },
            yaxis: {
                // Don't pad out the bottom of the data range.  By default,
                // axes scaled as if data extended 10% above and below the
                // actual range to prevent data points right on grid boundaries.
                // Don't want to do that here.
                padMin: 0,
                labelRenderer: $.jqplot.CanvasAxisLabelRenderer
            }
        }
    };
};