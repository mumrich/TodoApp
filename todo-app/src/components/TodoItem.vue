<template>
  <div class="todo-item">
    <div class="dragger i-mdi-drag-vertical"></div>
    <TodoItemCheckboxVue v-model="model.checked" />
    <div class="input-wrapper" :class="inputClass" ref="inputWrapper" @dblclick="onDoubleClick">
      <input v-if="model.editable" v-model.lazy="model.value" />
      <span v-else>{{ model.value }}</span>
    </div>
    <div class="delete-btn i-mdi-delete-forever" @click="onDelete"></div>
  </div>
</template>

<script setup lang="ts">
import TodoItemCheckboxVue from './TodoItemCheckbox.vue'
import { computed, ref } from 'vue'
import { onClickOutside } from '@vueuse/core'

export interface ModelValueType {
  checked: boolean,
  editable: boolean,
  value: string,
}

const props = defineProps<{
  modelValue: ModelValueType
}>()

const emits = defineEmits<{
  'update:modelValue': [value: ModelValueType]
  'delete-clicked': []
}>()

const model = computed({
  get: () => props.modelValue,
  set: (v) => emits('update:modelValue', v)
})

const inputWrapper = ref<HTMLDivElement | null>(null)

const inputClass = computed(() => ({
  'line-through': model.value.checked
}))

function onDelete() {
  const answer = confirm(`Really delete '${model.value.value}'?`)
  if (answer === true) {
    emits('delete-clicked')
  }
}

function onDoubleClick() {
  model.value = { ...model.value, editable: true }
}

onClickOutside(inputWrapper, () => {
  if (model.value.editable === true) {
    model.value = { ...model.value, editable: false }
  }
})
</script>

<style scoped>
.todo-item {
  @apply p-2 bg-white;
  @apply flex flex-row;
}

.dragger {
  @apply cursor-grab text-3xl;
}

.delete-btn {
  @apply cursor-pointer text-3xl hidden;
}

.delete-btn:hover {
  @apply text-red-500;
}

.input-wrapper {
  @apply flex flex-row flex-1;
  @apply select-none;
}

.input-wrapper>* {
  @apply font-sans text-lg;
  @apply flex-1;
  @apply cursor-text;
}

.todo-item:hover>.delete-btn {
  @apply block;
}
</style>
