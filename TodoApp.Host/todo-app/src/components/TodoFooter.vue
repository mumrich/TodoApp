<template>
  <div class="todo-footer" v-show="displayFooter">
    <div class="flex flex-col justify-center">
      <span class="mr-4 select-none">{{ itemsLeft }} items left</span>
    </div>
    <div class="flex flex-row mr-4 justify-evenly">
      <template v-for="filterOption in filterOptions" :key="filterOption">
        <button
          type="button"
          class="filter-option"
          @click.stop.prevent="onSelectFilter(filterOption)"
          :class="{
            'filter-enabled': filterOption === selectedFilter
          }"
        >
          {{ filterOption }}
        </button>
      </template>
    </div>
    <div>
      <button type="button" v-show="displayClearOption" @click.stop.prevent="onClearAll" class="filter-option">Clear Completed</button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from "vue";
import type { TodoItem } from "./TodoItem.vue";

const props = defineProps<{
  filterOptions?: string[];
  todos: TodoItem[];
  selectedFilter: string | null;
}>();

const emits = defineEmits<{
  "update:todos": [value: TodoItem[]];
  "update:selectedFilter": [value: string | null];
}>();

const filterOptions = computed(() => props.filterOptions ?? []);
const displayClearOption = computed(() => props.todos.filter((todo) => todo.checked === true).length > 0);
const itemsLeft = computed(() => props.todos.filter((todo) => todo.checked === false).length);
const displayFooter = computed(() => props.todos.length > 0);

function onSelectFilter(filterOption: string) {
  emits("update:selectedFilter", filterOption);
}

function onClearAll() {
  const todosCopy = [...props.todos];

  for (const todoIndex in todosCopy) {
    const oldTodo = todosCopy[todoIndex];
    todosCopy.splice(Number(todoIndex), 1, { ...oldTodo, checked: false });
  }

  emits("update:todos", todosCopy);
}
</script>

<style scoped>
.todo-footer {
  @apply bg-white flex flex-row font-sans mt-1 p-3 justify-start;
}

.filter-option {
  @apply bg-white border-transparent rounded border-0.5 py-1 px-2;
  @apply cursor-pointer;
}

.filter-option:hover {
  @apply border-red-100 border-opacity-50;
}

.filter-enabled {
  @apply bg-red-50 shadow;
}
</style>
