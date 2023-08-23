<template>
  <div class="todo-item">
    <div class="dragger i-mdi-drag-vertical"></div>
    <TodoItemCheckboxVue v-model="model.checked" />
    <div class="input-wrapper" :class="inputClass" ref="inputWrapper" @dblclick="onDoubleClick">
      <input v-if="editable" v-model="modelText" />
      <span v-else>{{ model.text }}</span>
    </div>
    <div class="delete-btn i-mdi-delete-forever" @click="onDelete"></div>
  </div>
</template>

<script setup lang="ts">
import TodoItemCheckboxVue from './TodoItemCheckbox.vue'
import { computed, ref } from 'vue'
import { onClickOutside, useDebounceFn } from '@vueuse/core'

export interface TodoItem {
  id: string,
  checked: boolean,
  text: string,
}

const props = defineProps<{
  modelValue: TodoItem
}>()

const emits = defineEmits<{
  'update:modelValue': [value: TodoItem]
  'delete-clicked': []
}>()

const model = computed({
  get: () => props.modelValue,
  set: (v) => emits('update:modelValue', v)
})

const debouncedModelTextSetter = useDebounceFn((v) => model.value.text = v, 500)

const modelText = computed({
  get: () => model.value.text,
  set: (v) => {
    debouncedModelTextSetter(v)
  }
})

const inputWrapper = ref<HTMLDivElement | null>(null)
const editable = ref(false)

const inputClass = computed(() => ({
  'line-through': model.value.checked
}))

function onDelete() {
  const answer = confirm(`Really delete '${model.value.text}'?`)
  if (answer === true) {
    emits('delete-clicked')
  }
}

function onDoubleClick() {
  editable.value = true;
}

onClickOutside(inputWrapper, () => {
  editable.value = false;
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
