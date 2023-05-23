<template>
  <div class="card" :style="{ width: props.cardWidth }">
    <slot></slot>
  </div>
</template>

<script setup lang="ts">
interface Props {
  cardWidth?: string;
}

const props = withDefaults(defineProps<Props>(), {
  cardWidth: '80%',
});
</script>

<style scoped>
.card {
  border-radius: 10px;
  box-shadow: 0 2px 6px #454545;
  margin: 1rem auto;
  max-width: 80%;
  padding: 1rem;
}
</style>
