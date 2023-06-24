<template>
  <div class="card" :style="{ width: props.width }">
    <slot></slot>
  </div>
</template>

<script setup lang="ts">
interface Props {
  width: string;
}

const props = defineProps<Props>();
</script>

<style scoped>
.card {
  border-radius: 10px;
  box-shadow: 0 2px 6px #454545;
  margin: 1rem auto;
  max-width: 90%;
  padding: 1rem;
}
</style>
