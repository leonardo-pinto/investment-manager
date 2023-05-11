<template>
  <div class="card">
    <slot></slot>
  </div>
</template>

<style scoped>
.card {
  border-radius: 10px;
  box-shadow: 0 2px 6px #454545;
  margin: 1rem auto;
  max-width: 80%;
  padding: 1rem;
}
</style>
