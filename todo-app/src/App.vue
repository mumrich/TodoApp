<template>
  <section>
    <AppTitleVue />
    <div class="flex flex-row justify-center">
      <div class="todo-container">
        <TodoInputVue class="mb-1" v-model="newTodo" @keyup:enter="onEnter" />
        <Draggable v-model="todos" item-key="value" group="todos">
          <template #item="item">
            <TodoItemVue
              :model-value="item.element"
              @update:model-value="onUpdate(item.index, $event)"
              @delete-clicked="onDelete(item.index)"
              class="mb-0.5"
            />
          </template>
        </Draggable>
        <TodoFooterVue v-model="todos" @update:filtered-model-value="onUpdateFilter" />
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import AppTitleVue from './components/AppTitle.vue'
import TodoInputVue from './components/TodoInput.vue'
import { ref } from 'vue'
import TodoItemVue, { type ModelValueType } from './components/TodoItem.vue'
import TodoFooterVue from './components/TodoFooter.vue'
import Draggable from 'vuedraggable'

const newTodo = ref<string>()
const todos = ref<ModelValueType[]>([])
const filteredTodos = ref<ModelValueType[]>([])

function onUpdateFilter(m: ModelValueType[]) {
  filteredTodos.value = m
}

function onEnter() {
  const text = newTodo.value ?? ''
  newTodo.value = ''
  todos.value.push({
    value: text,
    checked: false
  })
}

function onUpdate(i: number, v: ModelValueType) {
  todos.value.splice(i, 1, v)
}

function onDelete(i: number) {
  todos.value.splice(i, 1)
}
</script>

<style>
:root,
html {
  height: 100%;
  margin: 0;
  padding: 0;
  @apply bg-gray-100;
}

body {
  margin: 0;
}

.todo-container {
  @apply w-1/3;
  @apply flex flex-col;
  @apply shadow-2xl;
}
</style>
