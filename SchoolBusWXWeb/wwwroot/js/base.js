/* ===================== 全局变量 ===================== */
var globalData = {
    /**
     * 路径
     */
    path: {
        local: 'http://192.168.22.69:8000',
        server: 'http://192.168.22.146:8080',
        api: '/api'
    }
};
/* ===================== end ===================== */


/* ===================== 正则校验 ===================== */
var RE = {
    /**
     * 电话号码校验
     * @param phoneNum: number
     * @return Boolean
     */
    isPhone: function (phoneNum) {
        var reg = /^1[34578][0-9]{9}$/;
        return reg.test(phoneNum);
    }
};
/* ===================== end ===================== */


/* ===================== 正则校验 ===================== */
var util = {
    /**
     * http封装
     * @param $: jquery_object
     * @param option: object传递的借口参数
     * @return function or error_log
     */
    http: function ($, option) {
        // 必填参数
        option.data.timestamp = (new Date()).valueOf();
        option.data.client = 'pc';
        option.data.nonce_str = Math.random().toString(36).substr(0, 2);
        option.data.sing = 'xxxxxxxxxxx';
        option.data.apitoken = option.data.apitoken ? option.data.apitoken : '';

        $.ajax({
            url: globalData.path.server + globalData.path.api + option.url,
            data: option.data,
            type: option.type,
            dataType: 'json',
            crossDomain: true,
            xhrFields: {'Access-Control-Allow-Origin': '*'},
            scriptCharset: 'utf-8',
            success: function (res) {
                option.success(res);
            },
            error: function (msg) {
                console.error('http异常信息', msg);
            }
        })
    }
};
/* ===================== end ===================== */

/* ===================== echarts ===================== */
var echarts = {
    defaultChartOption: function () {
        return {
            // 色块
            color: ['#009688', '#1e9fff', '#5fb878', '#ffb980', '#d87a80'],

            // 标题
            // title: {},

            // 工具栏组件:导出图片，类型切换等功能
            // toolbox: {},

            // 图例组件:不同系列的标记，颜色和名字。
            legend: {
                textStyle: {
                    fontSize: 14,
                    color: '#333'
                },
                icon: 'circle',
                left: 'center',
                top: 0,
                data: []
            },

            // 提示框组件:鼠标移到图表中的线和展示
            tooltip: {
                textStyle: {
                    fontSize: 14
                },
                /*
                    item：数据项图形触发，主要在散点图，饼图等无类目轴的图表中使用。
                    axis：标轴触发，主要在柱状图，折线图等会使用类目轴的图表中使用。
                */
                trigger: 'axis',
                axisPointer: {
                    type: 'shadow' // line shadow none cross
                }
            },

            // 直角坐标系内绘图网格
            grid: {
                left: 5,
                right: 5,
                top: 55,
                bottom: 5,
                // 区域是否包含坐标轴的刻度标签
                containLabel: true,
            },

            // X轴线
            xAxis: [{
                /*
                    'value' 数值轴，适用于连续数据。
                    'category' 类目轴，适用于离散的类目数据，为该类型时必须通过 data 设置类目数据。
                */
                type: 'category',

                // 轴线
                axisLine: {
                    // 轴线的样式
                    lineStyle: {
                        width: 2,
                        color: '#009688'
                    }
                },

                // 坐标轴刻度
                axisTick: {
                    show: false
                },

                // 坐标轴刻度 *标签* 的相关设置。
                axisLabel: {
                    color: '#666',
                },

                // 坐标轴在 grid 区域中的分隔线。
                splitLine: {
                    show: true,
                    lineStyle: {
                        color: ['#f2f2f2', '#e6e6e6']
                    }
                },

                // 坐标轴在 grid 区域中的分隔区域，默认不显示。
                splitArea: {},

                // 控制图形的前后顺序。z值小的图形会被z值大的图形覆盖。
                z: 100,

                data: [],
            }],

            // y轴线
            yAxis: [{
                type: 'value',
                name: '个',
                axisLine: {
                    lineStyle: {
                        width: 2,
                        color: '#009688'
                    }
                },
                axisTick: {
                    show: false
                },
                axisLabel: {
                    color: '#666',
                    // formatter: '{value} ml'
                },
                splitLine: {
                    lineStyle: {
                        color: ['#e6e6e6', '#f2f2f2']
                    }
                },
                splitArea: {
                    show: true,
                    areaStyle: {
                        color: ['#fff', '#f9f9f9']
                    }
                }
            }],

            series: null
        };
    },
    createChart: function () {
        return echarts.init(document.getElementById(id));
    }
};
/* ===================== end ===================== */





