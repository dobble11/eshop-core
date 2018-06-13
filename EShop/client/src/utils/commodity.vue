<template>
    <router-link :to="`detail/${commodity.id}`">
        <el-card>
            <img class="thumb" :src="commodity.url">
            <div>
                <div class="flex-endpoint" style="align-items:baseline">
                    <span class="price">￥{{commodity.price}}</span>
                    <span style="color:gray">{{commodity.sv}}人付款</span>
                </div>
                <p class="title">{{commodity.name}}</p>
                <a href="/" style="color:gray">{{commodity.storeName}}</a>
            </div>
        </el-card>
    </router-link>
</template>

<script>
    export default {
        props: {
            commodity: Object
        }
    }
</script>

<style>
    .title {
        margin: 0;
    }
</style>