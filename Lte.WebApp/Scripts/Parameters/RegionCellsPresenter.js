function RegionCellsPresenter(cellsTag) {
    this.cellsTag = cellsTag;
}

RegionCellsPresenter.prototype.showCellList = function (data, removeUrl) {
    var cellsElement = $(this.cellsTag);
    cellsElement.html("");
    for (var i = 0; i < data.length; i++) {
        var cellName = data[i].CellName;
        var celltip = $("<div class='cellinfo'>" + cellName + "</div>");
        cellsElement.append(celltip);
        $(celltip).showTip({
            flagInfo: data[i].CellInfo,
            isAnimate: true,
            dblclickToRemove: true,
            dblclickAjax: function (x) {
                $.ajax({
                    url: removeUrl,
                    type: "GET",
                    dataType: "json",
                    data: { cellName: x },
                    success: function () { }
                });
            }
        });
    }
};

RegionCellsPresenter.prototype.showAllCells = function (getUrl, removeUrl) {
    var presenter = this;
    $.ajax({
        url: getUrl,
        type: "GET",
        dataType: "json",
        success: function (data) { presenter.showCellList(data, removeUrl); }
    });
};

RegionCellsPresenter.prototype.showSelectedCells = function (getUrl, message, removeUrl) {
    var presenter = this;
    $.ajax({
        url: getUrl,
        type: "GET",
        dataType: "json",
        data: { message: message },
        success: function (data) { presenter.showCellList(data, removeUrl); }
    });
};