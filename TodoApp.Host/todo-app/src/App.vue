<template>
  <section>
    <AppTitleVue />
    <div class="flex flex-row justify-center">
      <div class="todo-container">
        <div class="bg-white flex shadow mb-1 justify-center">
          <button class="history-btn" @click="undo" :disabled="canUndo === false">
            <div class="i-mdi-undo"></div>
          </button>
          <button class="history-btn" @click="redo" :disabled="canRedo === false">
            <div class="i-mdi-redo"></div>
          </button>
        </div>
        <TodoInputVue class="mb-1" v-model="newTodo" @keyup:enter="onEnter" />
        <Draggable v-model="filteredTodos" animation="150" item-key="value" group="todos" handle=".dragger">
          <template #item="item">
            <TodoItemVue
              :model-value="item.element"
              @update:model-value="onUpdate(item.index, $event)"
              @delete-clicked="onDelete(item.index)"
              class="mb-0.5"
            />
          </template>
        </Draggable>
        <TodoFooterVue v-model:todos="todos" v-model:selected-filter="selectedFilter" :filter-options="filterOptions" />
      </div>
    </div>
  </section>
</template>

<script setup lang="ts">
import AppTitleVue from "./components/AppTitle.vue";
import Draggable from "vuedraggable";
import TodoFooterVue from "./components/TodoFooter.vue";
import TodoInputVue from "./components/TodoInput.vue";
import TodoItemVue, { type TodoItem } from "./components/TodoItem.vue";
import { ref, computed } from "vue";
import { useStorage, useManualRefHistory } from "@vueuse/core";
import { v4 as uuidv4 } from "uuid";

interface TodoStorage {
  todos: TodoItem[];
  filter: string | null;
}

const filterOptions = ["All", "Active", "Completed"];

const todoStorage = useStorage<TodoStorage>("todos", {
  todos: [],
  filter: null
});

const { commit, undo, redo, canUndo, canRedo } = useManualRefHistory(todoStorage);

const newTodo = ref<string>("");
const todos = computed<TodoItem[]>({
  get: () => todoStorage.value.todos,
  set: (v) => {
    todoStorage.value = { ...todoStorage.value, todos: v };
    commit();
  }
});
const selectedFilter = computed<string | null>({
  get: () => todoStorage.value.filter,
  set: (v) => (todoStorage.value = { ...todoStorage.value, filter: v })
});
const filteredTodos = computed({
  get: () => {
    switch (selectedFilter.value) {
      case "All":
        return todos.value;
      case "Active":
        return todos.value.filter((todo) => todo.checked === false);
      case "Completed":
        return todos.value.filter((todo) => todo.checked === true);
    }

    return todos.value;
  },
  set: (sortedTodos) => {
    const forbiddenIds = sortedTodos.map((todo) => todo.id);
    const remainingTodos = todos.value.filter((todo) => forbiddenIds.findIndex((id) => id === todo.id) === -1);

    todos.value = [...sortedTodos, ...remainingTodos];
  }
});

function onEnter() {
  todos.value = [...todos.value, { id: uuidv4(), text: newTodo.value ?? "", checked: false }];
  newTodo.value = "";
}

function onUpdate(i: number, v: TodoItem) {
  todos.value.splice(i, 1, v);
}

function onDelete(i: number) {
  todos.value.splice(i, 1);
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

.history-btn {
  @apply cursor-pointer;
  @apply rounded bg-blue-50 border-0 m-2 shadow py-1 px-3;
  @apply text-2xl text-gray-500;
  @apply flex flex-row;
}

.history-btn:not([disabled]):hover {
  @apply bg-red-50 shadow-md;
}

.history-btn:is([disabled]) {
  @apply cursor-not-allowed;
  @apply bg-gray-50;
}
</style>
