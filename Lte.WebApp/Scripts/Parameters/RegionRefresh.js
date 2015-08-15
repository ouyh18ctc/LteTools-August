function RegionRefresh(cityTag, districtTag, townTag, regionTag) {
    this.cityTag = cityTag;
    this.districtTag = districtTag;
    this.townTag = townTag;
    this.regionTag = regionTag;
}

RegionRefresh.prototype.getDistrictList = function(url, forceSelection) {
    var districtElement = $(this.districtTag);
    districtElement.html(forceSelection ? "" :
        "<option value='=不限定区域=' selected='selected'>=不限定区域=</option>");
    var townElement = $(this.townTag);
    townElement.html(forceSelection ? "" :
        "<option value='=不限定镇区=' selected='selected'>=不限定镇区=</option>");

    $.ajax({
        type: "GET",
        url: url,
        dataType: "json",
        async: false,
        success: function (json) {
            if (json.length > 0) {
                for (var i = 0; i < json.length; i++) {
                    districtElement.append("<option value='" + json[i] + "'>" + json[i] + "</option>");
                }
            }
        }
    });
}

RegionRefresh.prototype.getTownList = function (url, forceSelection) {
    var townElement = $(this.townTag);
    townElement.html(forceSelection ? "" :
        "<option value='=不限定镇区=' selected='selected'>=不限定镇区=</option>");

    $.ajax({
        type: "GET",
        url: url,
        dataType: "json",
        async: false,
        success: function (json) {
            if (json.length > 0) {
                for (var i = 0; i < json.length; i++) {
                    townElement.append("<option value='" + json[i] + "'>" + json[i] + "</option>");
                }
            }
        }
    });
}

RegionRefresh.prototype.getRegionName = function (url) {
    var regionElement = $(this.regionTag);
    $.ajax({
        type: "GET",
        url: url,
        dataType: "json",
        async: false,
        success: function (json) {
            regionElement.val(json);
        }
    });
}