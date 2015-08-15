; (function ($) {
    jQuery.fn.extend({
        showTip: function (settings) {
            $(this).each(function () {
                //初始化配置信息
                var options = jQuery.extend({
                    flagCss: "tip",
                    flagWidth: $(this).outerWidth(),
                    flagInfo: $(this).attr("title"),
                    flagInfoAction: "",
                    isAnimate: false,
                    dblclickToRemove: false,
                    dblclickAjax: function(x){}
                },
                settings);
                if (!options.flagInfo) {
                    return;
                }
                $(this).removeAttr("title");
                $(this).mouseenter(function () {
                    //设置提示信息最小宽度为163
                    options.flagWidth = (parseInt(options.flagWidth) < 100) ? 163 : parseInt(options.flagWidth);
                    if (options.flagInfoAction != ""){
                        $.ajax({
                            type: "GET", //提交的类型 
                            data: { field: options.flagInfo },
                            dataType: "json",
                            url: options.flagInfoAction, //提交地址   
                            success: function (data) {
                                options.flagInfo = data;
                            }
                        });
                    }
                    var oTip = $("<div class='ui-slider-tooltip  ui-corner-all'></div>");
                    var oPointer = $("<div class='ui-tooltip-pointer-down'><div class='ui-tooltip-pointer-down-inner'></div></div>");
                    var oTipInfo = $("<div>" + options.flagInfo + "</div>").attr("class", options.flagCss).css("width", options.flagWidth + "px");
                    //合并提示信息
                    var oToolTip = $(oTip).append(oTipInfo).append(oPointer);
                    //添加淡入效果
                    if (options.isAnimate) {
                        $(oToolTip).fadeIn("slow");
                    }
                    $(this).after(oToolTip);

                    //计算提示信息的top、left和width
                    var position = $(this).position();
                    var oTipTop = eval(position.top - $(oTip).outerHeight());
                    var oTipLeft = position.left;
                    $(oToolTip).css("top", oTipTop + "px").css("left", oTipLeft + "px");

                    $(this).mouseleave(function () {
                        $(oToolTip).remove();
                    });

                    if (options.dblclickToRemove) {
                        $(this).dblclick(function () {
                    		options.dblclickAjax($(this).html());
                            $(oToolTip).remove();
                            $(this).remove();
                        });
                    }
                });
            });
            return this;
        }
    });
})(jQuery);