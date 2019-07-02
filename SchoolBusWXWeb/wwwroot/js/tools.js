$(function () {
    // 与学生关系选择
    $("#relationshipname").click(function () {
        var picker = new mui.PopPicker();
        picker.setData([{
            value: '父亲',
            text: '父亲'
        }, {
            value: '母亲',
            text: '母亲'
        }, {
            value: '爷爷',
            text: '爷爷'
        }, {
            value: '奶奶',
            text: '奶奶'
        }, {
            value: '姥爷',
            text: '姥爷'
        }, {
            value: '姥姥',
            text: '姥姥'
        }, {
            value: '其他',
            text: '其他'
        }]);
        picker.show(function (selectItems) {
            $("#relationship").val(selectItems[0].text);
        })
    });
});

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
