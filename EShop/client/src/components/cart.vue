<template>
    <div>
        <h2>我的购物车</h2>
        <el-table :data="data">
            <el-table-column label="商品">
                <template slot-scope="scope">
                    <img :src="scope.row.url" style="height:80px" />
                    <h3 style="margin:0">{{ scope.row.commodityName }}</h3>
                </template>
            </el-table-column>
            <el-table-column label="单价" prop="price"></el-table-column>
            <el-table-column label="数量">
                <template slot-scope="scope">
                    <el-input-number v-model="scope.row.count" @change="handleChange(scope.row)" :min="1"></el-input-number>
                </template>
            </el-table-column>
            <el-table-column label="小计">
                <template slot-scope="scope">
                    {{scope.row.price*scope.row.count}}
                </template>
            </el-table-column>
            <el-table-column label="操作">
                <template slot-scope="scope">
                    <el-button @click="remove(scope.$index,scope.row)">移除</el-button>
                </template>
            </el-table-column>
        </el-table>
        <h2 class="text-right">合计：<span class="price">￥{{totalPrice}}</span></h2>
        <div class="text-right">
            <el-button type="primary" @click="pay">结算</el-button>
        </div>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                data: []
            }
        },
        computed: {
            totalPrice() {
                var sum = 0

                if (this.data.length !== 0) {
                    this.data.forEach(m => sum += m.price * m.count)
                }
                return sum
            }
        },
        created() {
            this.axios.get('/api/cart').then(res => this.data = res.data)
        },
        methods: {
            remove(index, row) {
                this.axios.delete(`/api/cart/${row.commodityId}`).then(res => this.data.splice(index, 1))
            },
            handleChange(row) {
                this.axios.put(`/api/cart/${row.commodityId}/${row.count}`).catch(err => {
                    this.$message.error('操作失败')
                })
            },
            pay() {
                window.orders = this.data
                window.from = 'cart'
                this.$router.replace('/confirm-order')
            }
        }
    }
</script>