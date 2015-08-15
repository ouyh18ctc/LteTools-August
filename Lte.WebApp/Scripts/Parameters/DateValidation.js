function DateValidation(settings) {
    this.startdatetag = settings.startdatetag;
    this.enddatetag = settings.enddatetag;
    this.messagetag = settings.messagetag;
}

DateValidation.prototype.validate = function() {
    var begin = new Date($("#" + this.startdatetag).val());
    var end = new Date($("#" + this.enddatetag).val());
    $("#" + this.messagetag).html("");
    if (begin.toString() == "Invalid Date" || end.toString() == "Invalid Date") {
        $("#" + this.messagetag).append("<div class='error'>输入日期格式错误！</div>");
        return false;
    } else if (begin > end) {
        $("#" + this.messagetag).append("<div class='error'>开始日期必须早于或等于结束日期！</div>");
        return false;
    }
    return true;
};