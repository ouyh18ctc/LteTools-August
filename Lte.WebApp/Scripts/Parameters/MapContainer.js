function MapContainer(map) {
    this.map = map;
    this.grids = [];
    this.sectors = [];
    this.markers = [];
}

MapContainer.prototype.initalize = function (point, scale) {
    this.map.centerAndZoom(point, scale);
    this.map.enableScrollWheelZoom(); //启用滚轮放大缩小
};

MapContainer.prototype.addOneENodebMarker = function (data) {
    var marker = new BMap.Marker(new BMap.Point(data.BaiduLongtitute, data.BaiduLattitute));
    this.map.addOverlay(marker);
    var html = '<div class="infoBoxContent">'
        + '<div class="title">ENodeb ID: <span class="price">'
        + data.ENodebId + '</span></div>'
        + '<div class="list"><ul>'
        + '<li><div class="left">Name:</div><div class="rmb"> '
        + data.Name + '</div></li><li><div class="left">Address: </div><div class="rmb">' + data.Address
        + '</div></li><li><div class="left">Factory: </div><div class="rmb">' + data.Factory
        + '</div></li></ul></div></div>';
    //var infoWindow = new BMap.InfoWindow(html);
    var infoBox = new BMapLib.InfoBox(map, html, {
        boxStyle: {
            width: "270px",
            height: "200px"
        },
        closeIconMargin: "1px 1px 0 0",
        enableAutoPan: true,
        align: INFOBOX_AT_TOP
    });
    marker.addEventListener("click", function() {
        // this.openInfoWindow(infoWindow);
        infoBox.open(this);
    });
    this.markers.push(marker);
};

MapContainer.prototype.addOneBtsMarker = function (data) {
    var marker = new BMap.Marker(new BMap.Point(data.BaiduLongtitute, data.BaiduLattitute));
    this.map.addOverlay(marker);
    var infoWindow = new BMap.InfoWindow('<p>BTS ID: ' + data.BtsId + '</br>Name: '
        + data.Name + '</br>Address: ' + data.Address + '<br>BSC ID: ' + data.BscId + '</p>');
    marker.addEventListener("click", function () { this.openInfoWindow(infoWindow); });
    this.markers.push(marker);
};

MapContainer.prototype.addOneDistributionMarker = function (data) {
    var marker = new BMap.Marker(new BMap.Point(data.BaiduLongtitute, data.BaiduLattitute));
    this.map.addOverlay(marker);
    var infoWindow = new BMap.InfoWindow('<p>施主基站: ' + data.SourceName + '</br>室分名称: '
        + data.Name + '</br>覆盖范围: ' + data.Range + '<br>信源种类: ' + data.SourceType + '</p>');
    marker.addEventListener("click", function () { this.openInfoWindow(infoWindow); });
    this.markers.push(marker);
};

MapContainer.prototype.drawSectors = function (data, defaultColor) {
    for (var i = 0; i < data.length; i++) {
        var fillColor = defaultColor ? "#C8C1E3" : "#" + data[i].ColorString;
        var polygon = new BMap.Polygon([
                    new BMap.Point(data[i].X1, data[i].Y1),
                    new BMap.Point(data[i].X2, data[i].Y2),
                    new BMap.Point(data[i].X3, data[i].Y3)
        ], { strokeColor: "blue", strokeWeight: 3, strokeOpacity: 0.5, fillColor: fillColor });
        this.map.addOverlay(polygon);
        polygon.addEventListener("click", function () {
            var points = this.getPath();
            for (var j = 0; j < data.length; j++) {
                if (points[1].lng == data[j].X2 && points[1].lat == data[j].Y2) {
                    var cellInfo = new BMap.InfoWindow("<p>" + data[j].Info
                        + "</p>");
                    this.map.openInfoWindow(cellInfo, points[2]);
                    break;
                }
            }
        });
        this.sectors.push(polygon);
    }
};

MapContainer.prototype.addCoveragePoints = function (data, pointRadius) {
    for (var i = 0; i < data.length; i++) {
        var circle = new BMap.Circle(
            new BMap.Point(data[i].X, data[i].Y),
            pointRadius,
            {
                strokeColor: data[i].C,
                fillColor: "#" + data[i].C
            });
        map.addOverlay(circle);
    }
};

MapContainer.prototype.addGrids = function (data) {
    for (var i = 0; i < data.length; i++) {
        var polygon = new BMap.Polygon([
            new BMap.Point(data[i].X1, data[i].Y1),
            new BMap.Point(data[i].X1, data[i].Y2),
            new BMap.Point(data[i].X2, data[i].Y2),
            new BMap.Point(data[i].X2, data[i].Y1)
        ], {
            strokeColor: data[i].C, strokeWeight: 2, strokeOpacity: 0.5,
            fillColor: "#" + data[i].C
        });
        this.map.addOverlay(polygon);
        this.grids.push(polygon);
    }
};

MapContainer.prototype.addDtPoints = function (url, low, high, colorStr) {
    var points = [];
    $.ajax({
        url: url + low + "/" + high,
        type: "GET",
        dataType: "json",
        success: function (data) {
            for (var i = 0; i < data.length; i++) {
                points.push(new BMap.Point(data[i].Lon, data[i].Lat));
            }
        }
    });
    var options = {
        size: BMAP_POINT_SIZE_SMALL,
        color: colorStr
    }
    var pointCollection = new BMap.PointCollection(points, options);  // 初始化PointCollection
    pointCollection.addEventListener('click', function (e) {
        alert('坐标：(' + e.point.lng + ',' + e.point.lat + ')');  // 监听点击事件
    });
    this.map.addOverlay(pointCollection);
};