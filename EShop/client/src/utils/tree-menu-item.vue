<template>
    <el-menu-item v-if="model.children.length===0" :index="index">
        {{model.ymmc}}
    </el-menu-item>
    <el-submenu v-else :index="model.ymid">
        <template slot="title">
            <span>{{model.ymmc}}</span>
        </template>
        <tree-menu-item v-for="model in model.children" :model="model"></tree-menu-item>
    </el-submenu>
</template>

<script>
    export default {
        name:'TreeMenuItem',
        props: ['model'],
        computed: {
            spec() {
                return this.$root.$children[0].spec
            },
            qs() {
                return `?cx=${this.$root.$children[0].inputcx}&ch=${this.$root.$children[0].inputch}`
            },
            index() {
                if (!this.model.url) {
                    return '/' + this.model.ymid
                } 
                return this.spec ? (this.model.url + this.qs) : this.model.url
            }
        }
    }
</script>