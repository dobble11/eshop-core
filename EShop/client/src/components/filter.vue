<template>
    <div>
        <div>
            <dl>
                <dt>类别</dt>
                <dd>
                    <el-radio-group v-model="category" @change="change">
                        <el-radio-button label="全部"></el-radio-button>
                        <el-radio-button v-for="category in categories" :label="category.label"></el-radio-button>
                    </el-radio-group>
                </dd>
            </dl>
            <dl>
                <dt>价格</dt>
                <dd>
                    <el-radio-group v-model="jg" @change="change">
                        <el-radio-button label="不限"></el-radio-button>
                        <el-radio-button label="升序"></el-radio-button>
                        <el-radio-button label="降序"></el-radio-button>
                    </el-radio-group>
                </dd>
            </dl>
            <dl>
                <dt>销量</dt>
                <dd>
                    <el-radio-group v-model="xl" @change="change">
                        <el-radio-button label="不限"></el-radio-button>
                        <el-radio-button label="降序"></el-radio-button>
                        <el-radio-button label="升序"></el-radio-button>
                    </el-radio-group>
                </dd>
            </dl>
        </div>
        <div>
            <el-row :gutter="15">
                <el-col :span="6" v-for="item in page.items">
                    <commodity :commodity="item"></commodity>
                </el-col>
            </el-row>
            <el-pagination v-show="page.count/page.size>1"
                           @size-change="handleSizeChange"
                           @current-change="handleCurrentChange"
                           :current-page="page.curr"
                           :page-size="page.size"
                           :total="page.count"
                           :page-sizes="[8, 12, 16, 20]"
                           layout="total, prev, pager, next, sizes">
            </el-pagination>
        </div>
    </div>
</template>

<script>
    import Commodity from '../utils/commodity.vue'

    export default {
        components: { Commodity },
        data() {
            return {
                category: '全部',
                jg: '不限',
                xl: '不限',
                page: {},
                categories: []
            }
        },
        created() {
            this.axios.get('/api/filter').then(res => ({ categories: this.categories, page: this.page } = res.data))
        },
        methods: {
            change() {
                this.$nextTick(() => {
                    var filter = this.helper()
                    this.axios.get('/api/filter/page', { params: filter }).then(res => {
                        this.page = res.data
                        this.prevFilter = filter
                    })
                })
            },
            handleCurrentChange(val) {
                this.axios.get(`/api/filter/page/${this.page.size}/${val}`, { params: this.prevFilter })
                    .then(res => this.page = res.data)
            },
            handleSizeChange(val) {
                this.axios.get(`/api/filter/page/${val}`, { params: this.prevFilter })
                    .then(res => this.page = res.data)
            },
            helper() {
                const map = {
                    '不限': null,
                    '升序': 0,
                    '降序': 1
                }

                return {
                    categoryid: (this.categories.find(m => m.label === this.category) || {}).value,
                    jg: map[this.jg],
                    xl: map[this.xl]
                }
            }
        }
    }
</script>

<style>
    .el-radio-button__inner {
        border: none;
    }

    .el-radio-button:first-child .el-radio-button__inner {
        border-left: none;
    }


    dl > dt {
        width: 80px;
    }
</style>