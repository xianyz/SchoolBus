(function () {
    // 监听返回按钮
    pushHistory();
    window.addEventListener("popstate", function (e) {
        //关闭当前浏览器
        wx.closeWindow();
    }, false);
    function pushHistory() {
        var state = {
            title: "title",
            url: "#"
        };
        window.history.pushState(state, "title", "#");
    }
})();

// 通用ajax+防止csrf
function getAjax(url, parm, callBack, cmBack) {
    var form = $("#frm");
    var token = $('input[name="__RequestVerificationToken"]', form).val();
    parm.__RequestVerificationToken = token;
    $.ajax({
        type: 'post',
        url: url,
        data: parm,
        //async: false,
        success: function (msg) {
            callBack(msg);
        },
        error: function (result) {
        },
        complete: function (XMLHttpRequest, textStatus) {
            cmBack();
        }
    });
}