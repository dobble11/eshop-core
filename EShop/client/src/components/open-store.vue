<template>
    <div>
        <el-form :model="form" label-width="80px" style="width:500px">
            <el-form-item>
                <div class="form-title">申请表单</div>
            </el-form-item>
            <el-form-item label="头像">
                <el-upload class="avatar-uploader"
                           name="files"
                           action="/api/upload"
                           :show-file-list="false"
                           :on-success="success">
                    <img v-if="form.imgUrl" :src="form.imgUrl" class="avatar">
                    <i v-else class="el-icon-plus avatar-uploader-icon"></i>
                </el-upload>
            </el-form-item>
            <el-form-item label="名称">
                <el-input v-model="form.name"></el-input>
            </el-form-item>
            <el-form-item label="描述">
                <el-input type="textarea" v-model="form.description" autosize></el-input>
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
            <el-form-item>
                <el-button type="primary" @click="submit">提交</el-button>
                <el-button native-type="reset">重置</el-button>
            </el-form-item>
        </el-form>
    </div>
</template>

<script>
    export default {
        data() {
            return {
                dynamicTags: [],
                inputVisible: false,
                inputValue: '',
                form: {
                    id: '',
                    imgUrl: '',
                    name: '',
                    description: '',
                    tags: ''
                }
            }
        },
        methods: {
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
            submit() {
                this.form.tags = this.dynamicTags.join(',')
                this.axios.post('/api/store', this.form).then(res => location.reload())
            },
            success(res) {
                this.form.id = res.id
                this.form.imgUrl = `/assets/${res.id}/${res.names[0]}`
            }
        }
    }
</script>

<style>
    .form-title {
        font-size: 20px;
        line-height: 50px;
        text-align: center;
        font-weight: bold;
    }

    .el-tag {
        margin-right: 10px;
    }

    .button-new-tag {
        height: 32px;
        line-height: 30px;
        padding-top: 0;
        padding-bottom: 0;
    }

    .input-new-tag {
        width: 90px;
        margin-left: 10px;
        vertical-align: bottom;
    }

    .avatar-uploader .el-upload {
        border: 1px dashed #d9d9d9;
        border-radius: 6px;
        cursor: pointer;
        position: relative;
        overflow: hidden;
    }

        .avatar-uploader .el-upload:hover {
            border-color: #409EFF;
        }

    .avatar-uploader-icon {
        font-size: 28px;
        color: #8c939d;
        width: 178px;
        height: 178px;
        line-height: 178px;
        text-align: center;
    }

    .avatar {
        width: 178px;
        height: 178px;
        display: block;
    }
</style>