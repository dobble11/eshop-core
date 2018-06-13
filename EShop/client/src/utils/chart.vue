<template>
    <div class="chart" v-bind="$props" ref="myChart"></div>
</template>

<script>
    const events = ['click', 'contextmenu']

    export default {
        name: 'Chart',
        props: {
            option: {
                type: Object,
                default: null
            }
        },
        watch: {
            option(val) {
                if (this.chart) {
                    this.renderChart()
                }
            }
        },
        mounted() {
            //初始化时延迟渲染，避免在组件切换中，耗时较大的垃圾回收任务导致echart获取不到容器实际高度
            setTimeout(() => {
                this.chart = this.$echarts.init(this.$refs.myChart)
                this.renderChart()

                events.forEach(name => this.chart.on(name, (params) => this.$emit(name, params)))
                window.addEventListener('resize', this.onResize, false)
            }, 300)
        },
        destroyed() {
            window.removeEventListener('resize', this.onResize, false)
            this.chart.dispose()
        },
        methods: {
            onResize() {
                if (this.chart) this.chart.resize()
            },
            renderChart() {
                this.chart.setOption(this.option, true)
            }
        }
    }
</script>

<style>
    .chart {
        height: 100%;
        width: 100%;
    }
</style>