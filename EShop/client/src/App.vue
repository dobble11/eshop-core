<template>
    <el-container id="app">
        <el-header height="61px">
            <div class="flex-endpoint container" style="height:60px">
                <el-menu :default-active="$route.path"
                         :router="true"
                         mode="horizontal">
                    <el-menu-item index="/home">首页</el-menu-item>
                    <el-menu-item index="/filter">分类</el-menu-item>
                    <el-menu-item index="/cart">购物车</el-menu-item>
                </el-menu>
                <div class="header-right">
                    <el-autocomplete v-model="qs" class="input-with-select" :fetch-suggestions="querySearchAsync">
                        <el-select v-model="cate" slot="prepend">
                            <el-option label="全部" :value="0"></el-option>
                            <el-option label="商品" :value="1"></el-option>
                            <el-option label="店铺" :value="2"></el-option>
                        </el-select>
                        <el-button slot="append" icon="el-icon-search" @click="search"></el-button>
                    </el-autocomplete>
                    <template v-if="!user">
                        <el-button type="text" @click="signupVisible=true">注册</el-button> |
                        <el-button type="text" @click="$store.commit('open')" style="margin-left:0">登录</el-button>
                    </template>
                    <template v-else>
                        <el-dropdown>
                            <span class="el-dropdown-link">
                                {{user.name}}<i class="el-icon-arrow-down el-icon--right"></i>
                            </span>
                            <el-dropdown-menu slot="dropdown">
                                <el-dropdown-item>个人中心</el-dropdown-item>
                                <el-dropdown-item><a href="#/order">我的订单</a></el-dropdown-item>
                                <el-dropdown-item divided><a @click="signout">注销</a></el-dropdown-item>
                            </el-dropdown-menu>
                        </el-dropdown>
                    </template>
                    <template v-if="user&&user.role===2">
                        <el-button type="primary" @click="$router.push('/store')" round>我的店铺</el-button>
                    </template>
                    <template v-else>
                        <el-button type="primary" @click="$router.push('/open-store')" round>我要开店</el-button>
                    </template>
                </div>
            </div>
        </el-header>

        <el-main>
            <transition name="el-fade-in-linear">
                <router-view class="container"></router-view>
            </transition>
        </el-main>

        <el-dialog title="登录"
                   :visible="signinVisible"
                   :before-close="()=>this.$store.commit('close')"
                   width="500px">
            <el-form :model="signinForm" @keydown.enter="submit('signinForm')" :rules="rules1" ref="signinForm" label-width="80px">
                <el-form-item label="邮箱" prop="email">
                    <el-input v-model="signinForm.email"></el-input>
                </el-form-item>
                <el-form-item label="密码" prop="pass">
                    <el-input v-model="signinForm.pass" type="password"></el-input>
                </el-form-item>
            </el-form>
            <span slot="footer" class="dialog-footer">
                <el-button type="text" @click="signupVisible=true; $store.commit('close') ">现在去注册</el-button>
                <el-button type="primary" @click="submit('signinForm')">登录</el-button>
            </span>
        </el-dialog>
        <el-dialog title="注册"
                   :visible.sync="signupVisible"
                   width="500px">
            <el-form :model="signupForm" @keydown.enter="submit('signupForm')" :rules="rules2" ref="signupForm" status-icon label-width="80px">
                <el-form-item label="昵称" prop="name">
                    <el-input v-model="signupForm.name"></el-input>
                </el-form-item>
                <el-form-item label="邮箱" prop="email">
                    <el-input v-model="signupForm.email"></el-input>
                </el-form-item>
                <el-form-item label="密码" prop="pass">
                    <el-input v-model="signupForm.pass" type="password"></el-input>
                </el-form-item>
                <el-form-item label="确认密码" prop="checkPass">
                    <el-input v-model="signupForm.checkPass" type="password"></el-input>
                </el-form-item>
            </el-form>
            <span slot="footer" class="dialog-footer">
                <el-button type="primary" @click="submit('signupForm')">确认注册</el-button>
            </span>
        </el-dialog>
    </el-container>
</template>

<script>
    export default {
        data() {
            var validatePass = (rule, value, callback) => {
                if (value === '') {
                    callback(new Error('请输入密码'))
                } else {
                    if (this.signupForm.checkPass !== '') {
                        this.$refs.signupForm.validateField('checkPass')
                    }
                    callback()
                }
            }
            var validatePass2 = (rule, value, callback) => {
                if (value === '') {
                    callback(new Error('请再次输入密码'))
                } else if (value !== this.signupForm.pass) {
                    callback(new Error('两次输入密码不一致!'))
                } else {
                    callback()
                }
            }

            return {
                user: null,
                cate: 0,
                qs: '',
                signupVisible: false,
                signinForm: {
                    email: '',
                    pass: ''
                },
                signupForm: {
                    name: '',
                    email: '',
                    pass: '',
                    checkPass: ''
                },
                rules1: {
                    email: [
                        { required: true, message: '请输入邮箱地址', trigger: 'blur' },
                        { type: 'email', message: '请输入正确的邮箱地址', trigger: 'blur,change' }
                    ],
                    pass: [
                        { required: true, message: '请输入密码', trigger: 'blur' }
                    ]
                },
                rules2: {
                    name: [
                        { required: true, message: '请输入昵称', trigger: 'blur' }
                    ],
                    email: [
                        { required: true, message: '请输入邮箱地址', trigger: 'blur' },
                        { type: 'email', message: '请输入正确的邮箱地址', trigger: 'blur,change' }
                    ],
                    pass: [
                        { validator: validatePass, trigger: 'blur' }
                    ],
                    checkPass: [
                        { validator: validatePass2, trigger: 'blur' }
                    ]
                }
            }
        },
        computed: {
            signinVisible() {
                return this.$store.state.show
            }
        },
        created() {
            this.axios.get('/api/index').then(res => this.user = res.data)
        },
        methods: {
            submit(formName) {
                this.$refs[formName].validate((valid) => {
                    if (valid) {
                        if (formName === 'signupForm') {
                            this.axios.post('/api/account/signup', this.signupForm).then(res => {
                                location.reload()
                            }).catch(err => {
                                this.$message('邮箱已被注册')
                            })
                        } else if (formName === 'signinForm') {
                            this.axios.post('/api/account/signin', this.signinForm).then(res => {
                                location.reload()
                            }).catch(err => {
                                this.$message.error('账号或密码错误')
                            })
                        }
                    } else {
                        this.$message.warining('请填写正确信息')
                        return false
                    }
                })
            },
            querySearchAsync(qs, cb) {
                this.axios.get('/api/index/suggest', { params: { cate: this.cate, qs: this.qs.toLowerCase() } })
                    .then(res => cb(res.data))
            },
            search() {
                if (this.qs) {
                    this.$router.push(`/search-result?cate=${this.cate}&qs=${this.qs}`)
                }
            },
            signout() {
                this.axios.post('/api/account/signout').then(res => location.reload())
            }
        }
    }
</script>

<style>
    body {
        margin: 0;
    }

    a {
        display: block;
        color: inherit;
        text-decoration: none;
    }

    img {
        max-width: 100%;
    }

    dl > * {
        display: table-cell;
    }

    .container {
        max-width: 1200px;
        margin: 0 auto;
    }

    .price {
        color: orangered;
        font-size: 24px;
    }

    .text-right {
        text-align: right;
    }

    .flex {
        display: flex;
    }

    .el-menu--horizontal {
        border-bottom: none;
    }

    .el-header {
        border-bottom: solid 1px #e6e6e6;
    }

    .flex-endpoint {
        display: flex;
        justify-content: space-between;
    }

    .header-right {
        line-height: 60px;
    }

    .input-with-select {
        width: 350px;
        margin-right: 35px;
    }

        .input-with-select .el-input__inner {
            line-height: normal;
        }

        .input-with-select .el-select {
            width: 80px;
        }

    .el-dropdown-link {
        padding: 12px 8px;
        cursor: pointer;
        color: #409EFF;
    }

    .el-select {
        width: 100%;
    }

    .form-inline .form-group {
        display: inline-block;
        margin-bottom: 10px;
        margin-right: 8px;
    }
</style>
