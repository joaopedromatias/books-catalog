<script setup lang="ts">
import { computed, ref } from 'vue'

const { book } = defineProps<{ book: Book }>()

const imageLoaded = ref(false)

const alt = computed(() => `Cover of ${book.title}`)
</script>

<template>
  <li>
    <article class="book-card">
      <div class="book-info">
        <div>
          <h1 class="book-title">{{ book.title }}</h1>
          <h2 class="book-subject">{{ book.subject }}</h2>
        </div>
        <div>
          <p class="book-author">{{ book.authorName }} ({{ book.publishYear }})</p>
        </div>
      </div>
      <div class="book-image-container">
        <img
          v-show="imageLoaded"
          :src="book.coverUri"
          :alt="alt"
          @load="imageLoaded = true"
          class="book-image"
        />
        <div v-show="!imageLoaded" class="img-skeleton"></div>
      </div>
    </article>
  </li>
</template>

<style>
.book-card {
  display: flex;
  flex-direction: row;
  justify-content: space-between;
  margin: 0 0 0 auto;
  padding: 15px;
  border-bottom: 1px solid #ddd;
  background-color: #fff;
  max-width: 90%;
}

.book-info {
  flex-basis: 70%;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  padding: 10px;
}

.book-title {
  font-size: 1.4rem;
  font-weight: bold;
  margin-bottom: 5px;
}

.book-subject {
  font-size: 1.1rem;
  color: #777;
}

.book-image-container {
  flex-grow: 1;
  position: relative;
  max-width: 150px;
  height: 200px;
}

.book-image {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 8px;
}

.img-skeleton {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background: linear-gradient(90deg, #f0f0f0 25%, #e0e0e0 50%, #f0f0f0 75%);
  animation: loading 1.5s infinite ease-in-out;
}

@keyframes loading {
  0% {
    background-position: -200px 0;
  }
  100% {
    background-position: 200px 0;
  }
}
</style>
