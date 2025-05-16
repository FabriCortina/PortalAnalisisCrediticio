<template>
  <div class="relative" :style="{ paddingBottom: `${aspectRatio}%` }">
    <img
      v-if="isLoaded"
      :src="src"
      :alt="alt"
      class="absolute inset-0 w-full h-full object-cover"
      :class="[rounded ? 'rounded-lg' : '']"
    />
    <div
      v-else
      class="absolute inset-0 bg-gray-200 animate-pulse"
      :class="[rounded ? 'rounded-lg' : '']"
    ></div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const props = defineProps({
  src: {
    type: String,
    required: true
  },
  alt: {
    type: String,
    default: ''
  },
  aspectRatio: {
    type: Number,
    default: 100
  },
  rounded: {
    type: Boolean,
    default: false
  }
})

const isLoaded = ref(false)

onMounted(() => {
  const img = new Image()
  img.src = props.src
  img.onload = () => {
    isLoaded.value = true
  }
})
</script> 