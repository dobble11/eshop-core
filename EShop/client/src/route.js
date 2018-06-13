const Home = () => import('./components/home.vue')
const Cart = () => import('./components/cart.vue')
const Detail = () => import('./components/detail.vue')
const Store = () => import('./components/store.vue')
const Filter = () => import('./components/filter.vue')
const SearchResult = () => import('./components/search-result.vue')
const OpenStore = () => import('./components/open-store.vue')
const ConfirmOrder = () => import('./components/confirm-order.vue')
const Order = () => import('./components/order.vue')
const Mining = () => import('./components/mining.vue')

const routes = [
    { path: '/home', component: Home },
    { path: '/cart', component: Cart },
    { path: '/detail/:id', component: Detail, props: route => route.params },
    { path: '/store', component: Store },
    { path: '/filter', component: Filter },
    { path: '/search-result', component: SearchResult, props: route => route.query },
    { path: '/open-store', component: OpenStore },
    { path: '/confirm-order', component: ConfirmOrder, props: route => ({ orders: window.orders, from: window.from }) },
    { path: '/order', component: Order },
    { path: '/mining', component: Mining },
]

export default routes
