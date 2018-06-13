<template>
    <div>
        <table class="form">
            <colgroup>
                <col width="100" />
                <col />
            </colgroup>
            <tr>
                <td>收货人</td>
                <td><el-input v-model="name"></el-input></td>
            </tr>
            <tr>
                <td>收货地址</td>
                <td>
                    <el-cascader expand-trigger="hover"
                                 :options="options"
                                 v-model="address">
                    </el-cascader>
                </td>
            </tr>
            <tr>
                <td>详细地址</td>
                <td><el-input v-model="address1"></el-input></td>
            </tr>
            <tr>
                <td>联系电话</td>
                <td><el-input v-model="tel"></el-input></td>
            </tr>
            <tr>
                <td>备注</td>
                <td><el-input type="textarea" v-model="note"></el-input></td>
            </tr>
        </table>
        <el-table :data="orders">
            <el-table-column label="商品">
                <template slot-scope="scope">
                    <img :src="scope.row.url" style="height:80px" />
                    <h3 style="margin:0">{{ scope.row.commodityName }}</h3>
                </template>
            </el-table-column>
            <el-table-column label="单价" prop="price"></el-table-column>
            <el-table-column label="数量" prop="count"></el-table-column>
            <el-table-column label="小计">
                <template slot-scope="scope">
                    {{scope.row.price*scope.row.count}}
                </template>
            </el-table-column>
        </el-table>
        <h2 class="text-right">合计：<span class="price">￥{{totalPrice}}</span></h2>
        <div class="text-right">
            <el-button type="primary" @click="pay">提交订单</el-button>
        </div>
    </div>
</template>

<script>
    export default {
        props: ['orders', 'from'],
        data() {
            return {
                address: [],
                address1: '',
                tel: '',
                note: '',
                name: '',
                options: [
                    { label: '湖北省', value: '湖北省', children: [{ label: '武汉市', value: '武汉市' }] },
                    { label: '湖南省', value: '湖南省', children: [{ label: '长沙市', value: '长沙市' }] }
                ]
            }
        },
        computed: {
            totalPrice() {
                var sum = 0

                if (this.orders.length !== 0) {
                    this.orders.forEach(m => sum += m.price * m.count)
                }
                return sum
            }
        },
        methods: {
            pay() {
                if (this.tel && this.name && this.address1 && this.address.length != 0) {
                    var addr = ''
                    this.address.forEach(m => addr += m)
                    addr += this.address1

                    this.axios.post(`/api/order?from=${this.from}`, this.orders.map(m => ({
                        commodityId: m.commodityId,
                        count: m.count,
                        tel: this.tel,
                        name: this.name,
                        address: addr,
                        note: this.note
                    }))).then(res => {
                        this.$message.success('购买成功')
                        this.$router.push('/home')
                    })
                } else {
                    this.$message.error('请填写完整信息')
                }
            }
        }
    }
</script>

<style>
    .form {
        width: 100%;
    }

        .form .el-cascader {
            width: 100%;
        }

        .form td {
            border-bottom: 1px solid #ebeef5;
        }

            .form td:first-child {
                padding: 0 10px;
                color: gray;
            }

        .form input, .form textarea {
            width: 100%;
            border: none;
        }
</style>