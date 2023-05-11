<template>
  <button v-if="!props.link" :class="props.mode">
    <slot></slot>
  </button>
  <router-link v-else :to="props.to" :class="props.mode">
    <slot> </slot>
  </router-link>
</template>

<script setup lang="ts">
interface Props {
  link?: boolean;
  to?: string;
  mode?: string | null;
}

const props = withDefaults(defineProps<Props>(), {
  link: false,
  to: '/',
  mode: null,
});
</script>

<style scoped>
button,
a {
  background-color: #ff6000;
  border: 1px solid #ff6000;
  border-radius: 30px;
  cursor: pointer;
  color: white;
  font-weight: bold;
  padding: 0.75rem 1.5rem;
  text-decoration: none;
}

.outline {
  background-color: transparent;
  color: #ff6000;
}
</style>
