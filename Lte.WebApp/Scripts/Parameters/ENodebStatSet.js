function ENodebStatSet() { }

ENodebStatSet.prototype.getDistrictENodebsStat = function (cityName) {
    var dataValue = [];
    $.ajax({
        url: "/Parameters/GetDistrictENodebsStat",
        type: "GET",
        dataType: "json",
        data: { cityName: cityName },
        async: false,
        success: function (data) {
            $(data).each(function (index) {
                dataValue.push([data[index].D, data[index].N]);
            });
        }
    });
    return dataValue;
};

ENodebStatSet.prototype.getTownENodebsStat = function (cityName, districtName) {
    var dataValue = [];
    $.ajax({
        url: "/Parameters/GetTownENodebsStat",
        type: "GET",
        dataType: "json",
        data: { cityName: cityName, districtName: districtName },
        async: false,
        success: function (data) {
            $(data).each(function (index) {
                dataValue.push([data[index].T, data[index].N]);
            });
        }
    });
    return dataValue;
};