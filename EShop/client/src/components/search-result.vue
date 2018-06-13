<template>
    <div>
        <el-row :gutter="15">
            <el-col :span="6" v-for="item in data">
                <commodity :commodity="item"></commodity>
            </el-col>
        </el-row>
    </div>
</template>

<script>
    import Commodity from '../utils/commodity.vue'

    export default {
        components: { Commodity },
        props: ['cate', 'qs'],
        data() {
            return {
                data: []
            }
        },
        created() {
            this.axios.get('/api/index/search', { params: { cate: this.cate, qs: this.qs } }).then(res => {
                this.data = res.data
            })
        }
    }
</script>