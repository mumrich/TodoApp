<template>
  <div class="todo-footer" v-show="displayFooter">
    <span class="mr-4 select-none">{{ itemsLeft }} items left</span>
    <div class="flex flex-row mr-4 justify-evenly">
      <template v-for="filterOption in filterOptions" :key="filterOption">
        <button type="button" class="filter-option" @click.stop.prevent="onSelectFilter(filterOption)" :class="{
          'filter-enabled': filterOption === selectedFilter
        }">
          {{ filterOption }}
        </button>
      </template>
    </div>
    <div>
      <button v-show="displayClearOption" @click.stop.prevent="onClear" class="cursor-pointer">
        Clear Completed
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import type { ModelValueType } from './TodoItem.vue'

const props = defineProps<{
  modelValue: ModelValueType[]
}>()

const emits = defineEmits<{
  'update:modelValue': [value: ModelValueType[]]
  'update:filteredModelValue': [value: ModelValueType[]]
}>()

const model = computed({
  get: () => props.modelValue,
  set: (v) => emits('update:modelValue', v)
})

const filterOptions = ['All', 'Active', 'Completed']
const selectedFilter = ref<string | null>(null)

const displayClearOption = computed(
  () => model.value.filter((todo) => todo.checked === true).length > 0
)
const itemsLeft = computed(() => model.value.filter((todo) => todo.checked === false).length)
const displayFooter = computed(() => model.value.length > 0)

watch(
  selectedFilter,
  (filterOption) => {
    let filteredTodos: ModelValueType[] = model.value

    if (filterOption === 'Active') {
      filteredTodos = model.value.filter((todo) => todo.checked === false)
    } else if (filterOption === 'Completed') {
      filteredTodos = model.value.filter((todo) => todo.checked === true)
    } else if (filterOption === 'All') {
      filteredTodos = model.value
    }

    emits('update:filteredModelValue', filteredTodos)
  },
  {
    immediate: true
  }
)

function onSelectFilter(filterOption: string) {
  selectedFilter.value = filterOption
}

function onClear() {
  for (const todoIndex in model.value) {
    const oldTodo = model.value[todoIndex]
    model.value.splice(Number(todoIndex), 1, { ...oldTodo, checked: false })
  }
}
</script>

<style scoped>
.todo-footer {
  @apply bg-white flex flex-row font-sans mt-1 p-3 justify-start;
}

.filter-option {
  @apply bg-white rounded border-transparent border-0.5 rounded cursor-pointer;
}

.filter-option:hover {
  @apply border-red-100 border-opacity-50;
}

.filter-enabled {
  @apply bg-red-50 shadow;
}
</style>
