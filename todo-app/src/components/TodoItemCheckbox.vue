<template>
  <div class="cursor-pointer mr-2 text-3xl" @click="onClickChechbox">
    <div class="text-green-400 i-mdi-checkbox-marked-circle hover:text-green-500" v-if="checkedModel" />
    <div class="text-gray-100 i-mdi-checkbox-blank-circle hover:text-gray-400" v-else />
    <input class="hidden" type="checkbox" v-model="checkedModel" />
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'

const props = defineProps<{
  modelValue: boolean
}>()

const emits = defineEmits<{
  'update:modelValue': [value: boolean]
}>()

const checkedModel = computed({
  get: () => props.modelValue,
  set: (v) => emits('update:modelValue', v)
})

function onClickChechbox() {
  checkedModel.value = !checkedModel.value
}
</script>
