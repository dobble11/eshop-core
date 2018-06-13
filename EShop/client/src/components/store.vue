<template>
    <div>
        <div style="width:400px; margin:0 auto;text-align:center">
            <img style="width: 100px; height: 100px;border-radius: 50%;margin:0 0 10px" :src="store.imgUrl" />
            <h3 style="margin:0 0 10px">{{store.name}}</h3>
            <p style="margin:0 0 10px">{{store.description}}</p>
        </div>
        <div style="text-align:right">
            <el-button type="plain" @click="$router.push('/mining')">统计分析</el-button>
        </div>
        <el-tabs>
            <el-tab-pane label="商品管理">
                <div>
                    <el-button type="primary" style="margin-top:10px" @click="addVisible=true">添加商品</el-button>
                </div>
                <el-table :data="page1.items">
                    <el-table-column prop="name" label="商品名"></el-table-column>
                    <el-table-column prop="categoryName" label="分类"></el-table-column>
                    <el-table-column prop="price" label="价格"></el-table-column>
                    <el-table-column prop="total" label="总数"></el-table-column>
                    <el-table-column prop="surplus" label="库存"></el-table-column>
                    <el-table-column prop="datetime" label="上架时间"></el-table-column>
                    <el-table-column>
                        <template slot-scope="scope">
                            <el-button size="mini"
                                       @click="edit(scope.row)">编辑</el-button>
                            <el-button size="mini"
                                       type="danger"
                                       @click="remove(scope.row)">删除</el-button>
                        </template>
                    </el-table-column>
                </el-table>
                <el-pagination v-show="page1.count/page1.size>1"
                               @size-change="handleSizeChange1"
                               @current-change="handleCurrentChange1"
                               :current-page="page1.index"
                               :page-size="page1.size"
                               :total="page1.count"
                               :page-sizes="[10, 20, 30, 40]"
                               layout="total, prev, pager, next, sizes">
                </el-pagination>
            </el-tab-pane>
            <el-tab-pane label="订单管理">
                <el-table :data="page2.items">
                    <el-table-column prop="commodityName" label="商品名"></el-table-column>
                    <el-table-column prop="price" label="价格"></el-table-column>
                    <el-table-column prop="count" label="数量"></el-table-column>
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
                            <el-button v-show="scope.row.state==1" size="mini"
                                       @click="updateOrder(scope.row)">发货</el-button>
                        </template>
                    </el-table-column>
                </el-table>
                <el-pagination v-show="page2.count/page2.size>1"
                               @size-change="handleSizeChange2"
                               @current-change="handleCurrentChange2"
                               :current-page="page2.index"
                               :page-size="page2.size"
                               :total="page2.count"
                               :page-sizes="[10, 20, 30, 40]"
                               layout="total, prev, pager, next, sizes">
                </el-pagination>
            </el-tab-pane>
        </el-tabs>
        <el-dialog title="商品-修改" :visible.sync="updateVisible">
            <el-form :model="selectCommodity" label-width="80px" style="width:500px">
                <el-form-item label="名称">
                    <el-input v-model="selectCommodity.name"></el-input>
                </el-form-item>
                <el-form-item label="分类">
                    <el-select v-model="selectCommodity.categoryId">
                        <el-option v-for="category in categories" :value="category.id" :label="category.name"></el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="描述">
                    <el-input type="textarea" v-model="selectCommodity.description" autosize></el-input>
                </el-form-item>
                <el-form-item label="价格">
                    <input-number v-model="selectCommodity.price"></input-number>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="update">修改</el-button>
                    <el-button native-type="reset">重置</el-button>
                </el-form-item>
            </el-form>
        </el-dialog>
        <el-dialog title="商品-添加" :visible.sync="addVisible">
            <el-form :model="form" label-width="80px" style="width:500px">
                <el-form-item label="名称">
                    <el-input v-model="form.name"></el-input>
                </el-form-item>
                <el-form-item label="分类">
                    <el-select v-model="form.categoryId">
                        <el-option v-for="category in categories" :value="category.id" :label="category.name"></el-option>
                    </el-select>
                </el-form-item>
                <el-form-item label="描述">
                    <el-input type="textarea" v-model="form.description" autosize></el-input>
                </el-form-item>
                <el-form-item label="价格">
                    <el-input v-model="form.price"></el-input>
                </el-form-item>
                <el-form-item label="数量">
                    <el-input v-model="form.total"></el-input>
                </el-form-item>
                <el-form-item label="标签">
                    <el-tag :key="tag"
                            v-for="tag in dynamicTags"
                            closable
                            :disable-transitions="false"
                            @close="handleClose(tag)">
                        {{tag}}
                    </el-tag>
                    <el-input class="input-new-tag"
                              v-if="inputVisible"
                              v-model="inputValue"
                              ref="saveTagInput"
                              size="small"
                              @keyup.enter.native="handleInputConfirm"
                              @blur="handleInputConfirm">
                    </el-input>
                    <el-button v-else class="button-new-tag" size="small" @click="showInput">+ New Tag</el-button>
                </el-form-item>
                <el-form-item label="图片">
                    <el-upload name="files"
                               action="/api/upload"
                               :on-success="success"
                               list-type="picture" multiple>
                        <el-button size="small" type="primary">点击上传</el-button>
                    </el-upload>
                </el-form-item>
                <el-form-item>
                    <el-button type="primary" @click="create">立即发布</el-button>
                    <el-button native-type="reset">重置</el-button>
                </el-form-item>
            </el-form>
        </el-dialog>
    </div>
</template>

<script>
    import InputNumber from '../utils/input-number.vue'

    export default {
        components: { InputNumber },
        data() {
            return {
                dynamicTags: [],
                inputVisible: false,
                addVisible: false,
                updateVisible: false,
                inputValue: '',
                selectCommodity: {},
                form: {
                    id: '',
                    name: '',
                    decription: '',
                    categoryId: null,
                    storeId: '',
                    price: null,
                    total: null,
                    tags: '',
                    thumbs: []
                },
                categories: [],
                store: {},
                page1: {},
                page2: {}
            }
        },
        created() {
            this.fetchData()
        },
        methods: {
            fetchData() {
                this.axios.get(`/api/store`).then(res => ({ store: this.store, page1: this.page1, page2: this.page2, categories: this.categories } = res.data))
            },
            handleCurrentChange1(val) {
                this.axios.get(`/api/store/commodity/page/${this.page1.size}/${val}`, { params: { storeId: this.store.id } })
                    .then(res => this.page1 = res.data)
            },
            handleSizeChange1(val) {
                this.axios.get(`/api/store/commodity/page/${val}`, { params: { storeId: this.store.id } })
                    .then(res => this.page1 = res.data)
            },
            handleCurrentChange2(val) {
                this.axios.get(`/api/store/order/page/${this.page2.size}/${val}`, { params: { storeId: this.store.id } })
                    .then(res => this.page2 = res.data)
            },
            handleSizeChange2(val) {
                this.axios.get(`/api/store/order/page/${val}`, { params: { storeId: this.store.id } })
                    .then(res => this.page2 = res.data)
            },
            create() {
                this.form.tags = this.dynamicTags.join(',')
                this.form.storeId = this.store.id

                this.axios.post('/api/commodity', this.form).then(res => {
                    this.$message.success('添加成功')
                    this.addVisible = false
                    this.fetchData()
                })
            },
            update() {
                this.axios.put('/api/commodity', this.selectCommodity).then(res => {
                    this.$message.success('修改成功')
                    this.updateVisible = false
                })
            },
            remove(row) {
                this.$confirm('此操作将永久删除该商品, 是否继续?', '提示', {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning'
                }).then(() => {
                    this.axios.delete(`/api/commodity/${row.id}`).then(res => {
                        this.$message.success('删除成功')
                        this.fetchData()
                    })
                })
            },
            edit(row) {
                this.selectCommodity = row
                this.updateVisible = true
            },
            updateOrder(row) {
                this.axios.put('/api/order', { id: row.id, state: 2, rate: row.rate }).then(res => {
                    this.$message.success('发货成功')
                    row.state = 2
                })
            },
            handleClose(tag) {
                this.dynamicTags.splice(this.dynamicTags.indexOf(tag), 1)
            },
            showInput() {
                this.inputVisible = true
                this.$nextTick(_ => {
                    this.$refs.saveTagInput.$refs.input.focus()
                })
            },
            handleInputConfirm() {
                let inputValue = this.inputValue
                if (inputValue) {
                    this.dynamicTags.push(inputValue)
                }
                this.inputVisible = false
                this.inputValue = ''
            },
            success(res) {
                this.form.id = res.id
                this.form.thumbs = res.names.map(m => ({ commodityId: res.id, fileName: m }))
            }
        }
    }
</script>