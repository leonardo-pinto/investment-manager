<template>
  <v-select
    label="Select trading country"
    :items="countryOptions"
    item-title="name"
    item-value="value"
    v-model="usePositionsStore().currentCountry"
  >
    <template #item="{ item, props }">
      <v-list-item v-bind="props">
        <template #title>
          <span class="d-flex align-center"
            ><img class="mr-2" style="width: 32px" :src="item.raw.icon" />
            {{ item.raw.name }}</span
          >
        </template>
      </v-list-item>
    </template>
  </v-select>
</template>
<script setup lang="ts">
import { TradingCountry } from '../../enums';
import { usePositionsStore } from '../../stores/positionsStore';

const countryOptions = [
  {
    name: 'United States',
    value: TradingCountry.US,
    icon: 'src/assets/us-flag-icon.png',
  },
  {
    name: 'Brazil',
    value: TradingCountry.BR,
    icon: 'src/assets/br-flag-icon.png',
  },
];
</script>
