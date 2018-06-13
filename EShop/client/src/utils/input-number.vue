<template>
    <div :class="{'el-input':true,'is-disabled':disabled}">
        <input v-bind="$props" class="el-input__inner" ref="input" :value="value" @input="handleInput($event.target.value)" @blur="hanldeBlur" autocomplete="off" />
    </div>
</template>

<script>
    export default {
        name: 'InputNumber',
        props: {
            value: [String, Number],
            disabled: Boolean,
            placeholder: String
        },
        methods: {
            handleInput(val) {
                var formattedValue = val
                if (formattedValue) {
                    var reg = /\D/
                    if (reg.test(formattedValue)) {
                        formattedValue = formattedValue.replace(reg, '')
                    }
                }

                if (formattedValue !== val) {
                    this.$refs.input.value = formattedValue
                }
                this.$emit('input', formattedValue)
            },
            hanldeBlur(event) {
                this.$emit('blur', event)
            }
        }
    }
</script>