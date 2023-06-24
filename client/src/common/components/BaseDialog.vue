<template>
  <teleport to="body">
    <div v-if="show" @click="handleClose" class="backdrop"></div>
    <transition name="dialog">
      <dialog open v-if="props.show" :style="{ width: props.width }">
        <header>
          <slot name="header">
            <h2>{{ props.title }}</h2>
          </slot>
        </header>
        <section>
          <slot></slot>
        </section>
        <menu>
          <BaseButton id="close-btn" @click="handleClose">Close</BaseButton>
          <slot name="actions"></slot>
        </menu>
      </dialog>
    </transition>
  </teleport>
</template>

<script setup lang="ts">
interface Props {
  show: boolean;
  title: string;
  width: string;
}

const props = defineProps<Props>();
const emit = defineEmits(['close']);

const handleClose = () => {
  emit('close');
};
</script>

<style scoped>
#close-btn {
  margin-right: 0.5rem;
}

.backdrop {
  position: fixed;
  top: 0;
  left: 0;
  height: 100vh;
  width: 100%;
  background-color: rgba(0, 0, 0, 0.75);
  z-index: 10;
}

dialog {
  position: fixed;
  top: 10vh;
  z-index: 100;
  border-radius: 12px;
  border: none;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.26);
  padding: 0;
  margin: 0 auto;
  overflow: hidden;
  background-color: white;
}

header {
  background-color: #ff6000;
  color: white;
  width: 100%;
  padding: 1rem;
}

header h2 {
  margin: 0;
}

section {
  padding: 1rem;
}

menu {
  padding: 1rem;
  display: flex;
  justify-content: flex-end;
  margin: 0;
}

.dialog-enter-from,
.dialog-leave-to {
  opacity: 0;
  transform: scale(0.8);
}

.dialog-enter-active {
  transition: all 0.3s ease-out;
}

.dialog-leave-active {
  transition: all 0.3s ease-in;
}

.dialog-enter-to,
.dialog-leave-from {
  opacity: 1;
  transform: scale(1);
}
</style>
