<template>
    <div>
        <el-carousel height="300px">
            <el-carousel-item style="background-color:#111088">
                <img src="/assets/1.jpg" class="img-re" />
            </el-carousel-item>
            <el-carousel-item style="background-color:#bbc2cc">
                <img src="/assets/2.jpg" class="img-re" />
            </el-carousel-item>
            <el-carousel-item style="background-color:#e6c76c">
                <img src="/assets/3.jpg" class="img-re" />
            </el-carousel-item>
            <el-carousel-item style="background-color:#96aaa9">
                <img src="/assets/4.jpg" class="img-re" />
            </el-carousel-item>
        </el-carousel>
        <template v-if="data3&&data3.length!==0">
            <p class="section">猜你喜欢</p>
            <el-row :gutter="15">
                <el-col :span="6" v-for="commodity in data3">
                    <commodity :commodity="commodity"></commodity>
                </el-col>
            </el-row>
        </template>
        <p class="section">最新商品</p>
        <el-row :gutter="15">
            <el-col :span="6" v-for="commodity in data1">
                <commodity :commodity="commodity"></commodity>
            </el-col>
        </el-row>
        <p class="section">热门商品</p>
        <el-row :gutter="15">
            <el-col :span="6" v-for="commodity in data2">
                <commodity :commodity="commodity"></commodity>
            </el-col>
        </el-row>
    </div>
</template>

<script>
    import Commodity from '../utils/commodity.vue'

    export default {
        components: { Commodity },
        data() {
            return {
                data1: [],
                data2: [],
                data3: []
            }
        },
        created() {
            this.axios.get('/api/commodity').then(res => ({ data1: this.data1, data2: this.data2, data3: this.data3 } = res.data))
        }
    }
</script>

<style>
    .thumb {
        height: 230px
    }

    .el-col {
        margin: 7.5px 0;
    }

    .img-re {
        display: block;
        height: 300px;
        margin: 0 auto;
    }

    .section {
        margin: 16px 0 0;
        padding: 8px 16px;
        border-left: 5px solid #409EFF;
        border-radius: 4px;
        background-color: #ecf8ff;
    }
</style>