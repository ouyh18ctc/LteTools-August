function Pie3dSetting(title, data) {
    this.title = title;
    this.data = data;
}

Pie3dSetting.prototype.getOptions = function (withyValue) { 
    return {
            chart: {
                type: 'pie',
                options3d: {
                    enabled: true,
                    alpha: 45,
                    beta: 0
                }
            },
            title: {
                text: this.title
            },
            tooltip: {
                pointFormat: withyValue ? '数量：<b>{point.y}</b>, {series.name}: <b>{point.percentage:.2f}%</b>' :
                    '{series.name}: <b>{point.percentage:.2f}%</b>'
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: 'pointer',
                    depth: 35,
                    dataLabels: {
                        enabled: true,
                        format: '{point.name}'
                    }
                }
            },
            series: [{
                type: 'pie',
                name: '所占百分比',
                data: this.data
            }]
        };
};