<template>
  <div class="flex flex-row my-4 w-full justify-center">
    <span class="font-sans text-center text-red-500/15 text-8xl">{{ props.modelValue }}</span>
  </div>
</template>

<script setup lang="ts">
const props = withDefaults(
  defineProps<{
    modelValue?: string;
  }>(),
  {
    modelValue: "todos"
  }
);
</script>
