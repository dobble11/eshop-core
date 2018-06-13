<template>
    <div class="flex">
        <div class="left">
            <img class="display" :src="curr" />
            <nav class="thumblist">
                <img v-for="url in commodity.thumbs" :class="{active:url===curr}" :src="url" @click="curr = url" />
            </nav>
        </div>
        <div class="right">
            <h2>{{commodity.name}}</h2>
            <dl>
                <dt>价格</dt>
                <dd class="price">￥{{commodity.price}}</dd>
            </dl>
            <dl>
                <dt>评分</dt>
                <dd>
                    <el-rate v-model="commodity.rate"
                             disabled
                             show-score
                             text-color="#ff9900"
                             score-template="{value}">
                    </el-rate>
                </dd>
            </dl>
            <dl>
                <dt>商品详细</dt>
                <dd>{{commodity.description}}</dd>
            </dl>
            <dl>
                <dt>销量</dt>
                <dd>{{commodity.total-commodity.surplus}}</dd>
            </dl>
            <dl>
                <dt>数量</dt>
                <dd>
                    <el-input-number v-model="count" :min="1" :max="commodity.surplus" ontrols-position="right"></el-input-number> 件
                    <span>库存{{commodity.surplus}}件</span>
                </dd>
            </dl>
            <div>
                <el-button type="primary" @click="pay" plain>立即购买</el-button>
                <el-button type="primary" @click="addcart">
                    <i class="fa fa-shopping-cart"></i>
                    加入购物车
                </el-button>
            </div>
        </div>
    </div>
</template>

<script>
    export default {
        props: ['id'],
        data() {
            return {
                commodity: {},
                curr: '',
                count: 1,
                rate: null
            }
        },
        created() {
            this.axios.get(`/api/commodity/${this.id}`).then(res => {
                this.commodity = res.data
                this.curr = this.commodity.thumbs[0]
            })
        },
        methods: {
            addcart() {
                this.axios.post('/api/cart', { count: this.count, commodityid: this.commodity.id }).then(res => this.$message({ message: '已添加购物车', type: 'success' }))
            },
            pay() {
                window.orders = [{ commodityId: this.commodity.id, commodityName: this.commodity.name, url: this.commodity.thumbs[0], count: this.count, price: this.commodity.price }]
                this.$router.replace('/confirm-order')
            }
        }
    }
</script>

<style>
    .right {
        flex: 1;
        margin-left: 20px;
    }

        .right > * {
            margin: 15px 0;
        }

    .left .display {
        height: 420px;
        width:400px;
    }

    .left .thumblist > img {
        width: 60px;
        height: 100%;
        margin: 10px 10px;
        background-color: transparent;
        border: 2px solid transparent;
    }

    .thumblist > img.active {
        border: 2px solid orangered;
    }


    dl > dt {
        width: 100px;
        color: gray;
    }
</style>