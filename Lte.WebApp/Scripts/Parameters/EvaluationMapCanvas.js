function EvaluationMapCanvas(container) {
    container.map.clearOverlays();
    this.container = container;
    this.updateRange();
}

EvaluationMapCanvas.prototype.updateRange = function () {
    var bs = this.container.map.getBounds();   //获取可视区域
    this.bssw = bs.getSouthWest();   //可视区域左下角
    this.bsne = bs.getNorthEast();   //可视区域右上角
};

EvaluationMapCanvas.prototype.importCellsWithRange = function (url) {
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        data: {
            southWestLon: this.bssw.lng,
            southWestLat: this.bssw.lat,
            northEastLon: this.bsne.lng,
            northEastLat: this.bsne.lat,
            fieldName: ''
        },
        success: function () { }
    });
};

EvaluationMapCanvas.prototype.removeMarkers = function () {
    var container = this.container;
    while (container.markers.length > 0) {
        var marker = container.markers.pop();
        container.map.removeOverlay(marker);
    }
};

EvaluationMapCanvas.prototype.drawMarkers = function (url, ids) {
    var container = this.container;
    if (container.markers.length > 0) {
        return;
    }
    $(ids).each(function (i) {
        sendRequest(url, "GET", { id: ids[i] }, function(result) {
            container.addOneENodebMarker(result);
        });
    });
};

EvaluationMapCanvas.prototype.drawENodebMarkers = function (url) {
    var container = this.container;
    if (container.markers.length > 0) {
        return;
    }
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        success: function (data) {
            $(data).each(function (i) {
                container.addOneENodebMarker(data[i]);
            });
        }
    });
};

EvaluationMapCanvas.prototype.drawBtsMarkers = function (url) {
    var container = this.container;
    if (container.markers.length > 0) {
        return;
    }
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        success: function (data) {
            $(data).each(function (i) {
                container.addOneBtsMarker(data[i]);
            });
        }
    });
};

EvaluationMapCanvas.prototype.drawDistributionMarkers = function (url) {
    var container = this.container;
    if (container.markers.length > 0) {
        return;
    }
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        success: function (data) {
            $(data).each(function (i) {
                container.addOneDistributionMarker(data[i]);
            });
        }
    });
};

EvaluationMapCanvas.prototype.removeSectors = function () {
    var container = this.container;
    while (container.sectors.length > 0) {
        var sector = container.sectors.pop();
        container.map.removeOverlay(sector);
    }
};

EvaluationMapCanvas.prototype.drawSectors = function (url) {
    var container = this.container;
    if (container.sectors.length > 0) {
        return;
    }
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        success: function (data) {
            container.drawSectors(data, true);
        }
    });
};

EvaluationMapCanvas.prototype.drawQuerySectors = function (url) {
    var container = this.container;
    if (container.sectors.length > 0) {
        return;
    }
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        success: function (result) {
            container.drawSectors(result, true);
        }
    });
};

EvaluationMapCanvas.prototype.drawSectors = function (url, ids) {
    var container = this.container;
    if (container.sectors.length > 0) {
        return;
    }
    $(ids).each(function (i) {
        $.ajax({
            url: url + "/" + ids[i],
            type: "GET",
            dataType: "json",
            success: function (result) {
                container.drawSectors(result, true);
            }
        });
    });
};

EvaluationMapCanvas.prototype.drawKpiSectors = function (url, fieldName) {
    var container = this.container;
    if (container.sectors.length > 0) {
        return;
    }
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        data: { fieldName: fieldName },
        success: function (data) {
            container.drawSectors(data, false);
        }
    });
};

EvaluationMapCanvas.prototype.drawCoveragePoints = function (url, fieldName) {
    var container = this.container;
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        data: { fieldName: fieldName },
        async: false,
        success: function (data) {
            container.addCoveragePoints(data, 20);
        }
    });
};

EvaluationMapCanvas.prototype.removeGrids = function () {
    var container = this.container;
    while (container.grids.length > 0) {
        var grid = container.grids.pop();
        container.map.removeOverlay(grid);
    }
};

EvaluationMapCanvas.prototype.drawGrids = function (url, fieldName) {
    var container = this.container;
    if (container.grids.length > 0) {
        return;
    }
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        data: { fieldName: fieldName },
        success: function (data) {
            container.addGrids(data);
        }
    });
};

EvaluationMapCanvas.prototype.drawGridsWithRange = function (url, fieldName) {
    var container = this.container;
    if (container.grids.length > 0) {
        return;
    }
    $.ajax({
        url: url,
        type: "GET",
        dataType: "json",
        data: {
            southWestLon: this.bssw.lng,
            southWestLat: this.bssw.lat,
            northEastLon: this.bsne.lng,
            northEastLat: this.bsne.lat,
            fieldName: fieldName
        },
        success: function (data) {
            container.addGrids(data);
        }
    });
};