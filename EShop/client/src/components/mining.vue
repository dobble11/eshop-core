<template>
    <div>
        <div class="form-inline">
            <div class="form-group">
                <label>订单时间</label>
                <el-date-picker v-model="filter.start"
                                value-format="yyyy-MM-dd HH:mm:ss"
                                type="datetime">
                </el-date-picker> -
                <el-date-picker v-model="filter.end"
                                value-format="yyyy-MM-dd HH:mm:ss"
                                type="datetime">
                </el-date-picker>
            </div>
            <div class="form-group">
                <el-button type="primary" @click="query"><i class="fa fa-search"></i> 开始统计</el-button>
            </div>
        </div>
        <el-tabs>
            <el-tab-pane label="饼图">
                <chart :option="option1" style="width:500px; height:500px; display:inline-block"></chart>
            </el-tab-pane>
            <el-tab-pane label="柱状图">
                <chart :option="option2" style="width:600px; height:400px; display:inline-block"></chart>
            </el-tab-pane>
        </el-tabs>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                data1: [],
                data2: [],
                filter: {
                    start: '',
                    end: ''
                }
            }
        },
        computed: {
            option1() {
                return {
                    title: {
                        text: '用户爱好统计图',
                        x: 'center'
                    },
                    tooltip: {
                        trigger: 'item',
                        formatter: "{a} <br/>{b} : {c} ({d}%)"
                    },
                    legend: {
                        type: 'scroll',
                        orient: 'vertical',
                        left: 'left',
                        data: this.data1.map(m => m.name)
                    },
                    series: [
                        {
                            name: '分类',
                            type: 'pie',
                            radius: '50%',
                            center: ['50%', '50%'],
                            data: this.data1,
                            emphasis: {
                                itemStyle: {
                                    shadowBlur: 10,
                                    shadowOffsetX: 0,
                                    shadowColor: 'rgba(0, 0, 0, 0.5)'
                                }
                            },
                            label: {
                                formatter: '{b} {d}% '
                            }
                        }
                    ]
                }
            },
            option2() {
                return {
                    color: ['#3398DB'],
                    title: {
                        text: '各类商品平均消费水平',
                        x: 'center'
                    },
                    tooltip: {
                        trigger: 'axis',
                        axisPointer: {
                            type: 'shadow'
                        }
                    },
                    grid: {
                        left: '3%',
                        right: '4%',
                        bottom: '3%',
                        containLabel: true
                    },
                    xAxis: [
                        {
                            type: 'category',
                            data: this.data2.map(m => m.name),
                            axisTick: {
                                alignWithLabel: true
                            },
                            axisLabel: {
                                interval: 0
                            }
                        }
                    ],
                    yAxis: [
                        {
                            type: 'value',
                            axisLabel: {
                                formatter: '￥{value}'
                            }
                        }
                    ],
                    series: [
                        {
                            name: '金额',
                            type: 'bar',
                            barWidth: '60%',
                            label: {
                                normal: {
                                    show: true,
                                    position: 'top'
                                }
                            },
                            data: this.data2.map(m => m.value)
                        }
                    ]
                }
            }
        },
        created() {
            this.axios.get('/api/mining').then(res => ({ data: { data1: this.data1, data2: this.data2 }, start: this.filter.start, end: this.filter.end } = res.data))
        },
        methods: {
            query() {
                this.axios.get('/api/mining/filter', { params: this.filter }).then(res => ({ data1: this.data1, data2: this.data2 } = res.data))
            }
        }
    }
</script>