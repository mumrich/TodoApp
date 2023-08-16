<template>
  <div class="todo-item">
    <TodoItemCheckboxVue v-model="model.checked" />
    <input v-model="model.value" :class="inputClass" />
    <div class="delete-btn i-mdi-delete-forever" @click="onDelete"></div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import TodoItemCheckboxVue from './TodoItemCheckbox.vue'

export interface ModelValueType {
  value: string
  checked: boolean
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

const inputClass = computed(() => ({
  'line-through': model.value.checked
}))

function onDelete() {
  emits('delete-clicked')
}
</script>

<style scoped>
.todo-item {
  @apply flex flex-row p-2;
  @apply bg-white;
}

.delete-btn {
  @apply cursor-pointer text-3xl hidden hover:text-red-500;
}

input {
  @apply font-sans outline-none border-0 flex-1 text-lg;
}

.todo-item:hover > .delete-btn {
  @apply block;
}
</style>
