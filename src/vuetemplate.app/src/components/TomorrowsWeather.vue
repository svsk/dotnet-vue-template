<script setup lang="ts">
import { ref } from 'vue';

const tomorrowsWeather = ref<{ summary: string; temperatureC: number } | null>(null);

const getWeather = async () => {
  const response = await fetch('/api/weatherForecast');
  if (response.status === 200) {
    tomorrowsWeather.value = (await response.json())[0];
  }
};

getWeather();
</script>

<template>
  <div v-if="tomorrowsWeather !== null">{{ tomorrowsWeather.summary }} and {{ tomorrowsWeather.temperatureC }}°C</div>
</template>
