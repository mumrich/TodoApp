<template>
  <div class="bg-white flex flex-row mt-1 p-3 justify-between" v-show="displayFooter">
    <span class="mr-4 select-none">{{ itemsLeft }} items left</span>
    <div class="flex flex-row mr-4 justify-around">
      <template v-for="filterOption in filterOptions" :key="filterOption">
        <label :for="filterOption" class="cursor-pointer mr-3">
          <input :id="filterOption" type="radio" :value="filterOption" v-model="selectedFilter" />
          {{ filterOption }}
        </label>
      </template>
    </div>
    <button v-show="displayClearOption" @click.stop.prevent="onClear" class="cursor-pointer">
      Clear Completed
    </button>
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
  (v) => {
    let filteredTodos: ModelValueType[] = model.value
    if (v === 'Active') {
      filteredTodos = model.value.filter((todo) => todo.checked === false)
    } else if (v === 'Completed') {
      filteredTodos = model.value.filter((todo) => todo.checked === true)
    } else if (v === 'All') {
      filteredTodos = model.value
    }

    emits('update:filteredModelValue', filteredTodos)
  },
  {
    immediate: true
  }
)

function onClear() {
  for (const todoIndex in model.value) {
    const oldTodo = model.value[todoIndex]
    model.value.splice(Number(todoIndex), 1, { ...oldTodo, checked: false })
  }
}
</script>
