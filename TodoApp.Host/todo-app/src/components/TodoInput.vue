<template>
  <input
    class="outline-none border-0 shadow text-lg py-4 px-16"
    placeholder="What needs to be done?"
    :value="props.modelValue"
    @input="onInput"
    @keyup.enter="onEnter"
  />
</template>

<script setup lang="ts">
const props = defineProps<{
  modelValue?: string;
}>();

const emits = defineEmits<{
  "update:modelValue": [value: string];
  "keyup:enter": [];
}>();

function onInput(e: Event) {
  emits("update:modelValue", (e?.target as HTMLInputElement)?.value ?? "");
}

function onEnter() {
  emits("keyup:enter");
}
</script>
