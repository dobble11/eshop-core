<template>
    <div>
        <h2>我的订单</h2>
        <el-table :data="data">
            <el-table-column label="商品">
                <template slot-scope="scope">
                    <img :src="scope.row.url" style="height:80px" />
                    <h3 style="margin:0">{{ scope.row.commodityName }}</h3>
                </template>
            </el-table-column>
            <el-table-column label="单价" prop="price"></el-table-column>
            <el-table-column label="数量" prop="count"></el-table-column>
            <el-table-column label="合计">
                <template slot-scope="scope">
                    {{scope.row.price*scope.row.count}}
                </template>
            </el-table-column>
            <el-table-column label="收货人" prop="name"></el-table-column>
            <el-table-column label="收货地址" prop="address"></el-table-column>
            <el-table-column label="备注" prop="note"></el-table-column>
            <el-table-column label="购买时间" prop="datetime"></el-table-column>
            <el-table-column label="交易状态" prop="state" :formatter="(row,col,val)=>(val===1&&'已付款')||(val===2&&'已发货')||(val===4&&'交易成功')"></el-table-column>
            <el-table-column label="操作">
                <template slot-scope="scope">
                    <el-button v-show="scope.row.state===2" type="plain" @click="click(scope.row)">确认收货</el-button>
                </template>
            </el-table-column>
        </el-table>
        <el-dialog title="确认收货"
                   :visible.sync="visible"
                   width="400px">
            请对此商品做出评分
            <el-rate v-model="rate"
                     show-score
                     text-color="#ff9900"
                     score-template="{value}">
            </el-rate>
            <span slot="footer" class="dialog-footer">
                <el-button type="primary" @click="submit">提交评分</el-button>
            </span>
        </el-dialog>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                data: [],
                rate: null,
                visible: false,
                selectRow: {}
            }
        },
        created() {
            this.axios.get('/api/order').then(res => this.data = res.data)
        },
        methods: {
            click(row) {
                this.visible = true
                this.selectRow = row
            },
            submit() {
                this.axios.put('/api/order', { id: this.selectRow.id, rate: this.rate, state: 4 }).then(res => {
                    this.visible = false
                    this.selectRow.state=4
                    this.$message.success('收货成功')
                })
            }
        }
    }
</script>