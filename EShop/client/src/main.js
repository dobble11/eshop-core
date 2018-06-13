import Vue from 'vue'
import ElementUI from 'element-ui'
import 'element-ui/lib/theme-chalk/index.css'
import 'font-awesome-webpack'
import VueRouter from 'vue-router'
import VueAxios from 'vue-axios'
import Vuex from 'vuex'
import axios from 'axios'
import routes from './route'
import App from './App.vue'
import chart from './utils/chart.vue'
import echarts from 'echarts/lib/echarts'

import 'echarts/lib/component/title'
import 'echarts/lib/component/tooltip'
import 'echarts/lib/component/legendScroll'
import 'echarts/lib/chart/pie'
import 'echarts/lib/chart/bar'

Vue.use(ElementUI)
Vue.use(VueRouter)
Vue.use(Vuex)
Vue.use(VueAxios, axios)
Vue.component('chart', chart)

axios.interceptors.request.use(function(config) {
    var token = getCookie('token')

    if (token) {
        config.headers.Authorization = `Basic ${token}`
    }

    return config
}, function(error) {
    return Promise.reject(error)
})

axios.interceptors.response.use(function(response) {
    return response
}, function(error) {

    if (error.response) {
        if (error.response.status === 401) {
            store.commit('open')
        }
    } 
    return Promise.reject(error)
    })

const router = new VueRouter({ routes })
const store = new Vuex.Store({
    state: {
        show: false
    },
    mutations: {
        open(state) {
            state.show = true
        },
        close(state) {
            state.show = false
        }
    }
})

Vue.prototype.$echarts = echarts

new Vue({
    el: '#app',
    store,
    render: h => h(App),
    router
})

function getCookie(name) {
    var search = name + '='
    var returnvalue = ''
    if (document.cookie.length > 0) {
        var sd = document.cookie.indexOf(search);
        if (sd != -1) {
            sd += search.length
            var end = document.cookie.indexOf(';', sd)
            if (end == -1)
                end = document.cookie.length
            returnvalue = document.cookie.substring(sd, end)
        }
    }
    return returnvalue
}

