// 微信js
wx.ready(function () {
    // 转发到朋友圈
    wx.onMenuShareTimeline({
        title: '鲸卫士校车联盟-绑定乘车卡,关注孩子乘车安全',
        link: 'http://wx.360wll.cn/SchoolBusHome/index?type=0',
        imgUrl: 'http://wx.360wll.cn/img/pic1.jpg',
        success: function () {
            console.log("转发到朋友圈成功");
        },
        cancel: function () {
            console.log("转发到朋友圈失败");
        }
    });
    // 转发好友
    wx.onMenuShareAppMessage({
        title: '鲸卫士校车联盟',
        desc: '绑定乘车卡,关注孩子乘车安全',
        link: 'http://wx.360wll.cn/SchoolBusHome/index?type=0',
        imgUrl: 'http://wx.360wll.cn/img/pic1.jpg',
        type: '',
        dataUrl: '',
        success: function () {
            console.log("转发好友成功");
        }
    });
});