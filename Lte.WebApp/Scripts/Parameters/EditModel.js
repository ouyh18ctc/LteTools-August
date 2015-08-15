function EditModel(id) {
    var self = this;
    self.item = ko.observable();
    self.controller = "ENodeb";
    self.load = function () {
        $.ajax({
            url: "/api/" + self.controller + "/" + id,
            type: "GET",
            success: function (result) {
                self.item(result);
            }
        });
    };
    self.save = function () {
        $.ajax({
            url: "/api/" + self.controller + "/" + id,
            type: "POST",
            data: self.item(),
            success: function () { self.load(); }
        });
    };
    self.load();
}