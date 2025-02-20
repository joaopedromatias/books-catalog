<script setup lang="ts">
import { ref, watch } from 'vue'
import axios from 'axios'
import { apiClient } from '@/clients'
import BookCard from './BookCard.vue'
import { DEBOUNCE_MS } from '@/constants'

defineEmits<{ (e: 'next-page'): void; (e: 'previous-page'): void }>()

const props = defineProps<{ filters: Filter }>()

const books = ref<Book[]>([])
const debounceTimeoutId = ref<number | null>(null)
const isLoadingBooks = ref(false)
const errorMessage = ref('')
const isFirstFetch = ref(true)

watch(
  props.filters,
  async (newFilters) => {
    if (debounceTimeoutId.value) {
      clearTimeout(debounceTimeoutId.value)
    }

    books.value = []
    errorMessage.value = ''
    isLoadingBooks.value = true

    debounceTimeoutId.value = setTimeout(
      () => {
        fetchBooks(newFilters)
      },
      isFirstFetch.value ? 0 : DEBOUNCE_MS,
    )
  },
  { immediate: true },
)

function fetchBooks(filters: Filter) {
  const params = new URLSearchParams()
  params.append('authorName', filters.authorName || '')
  params.append('publishYear', String(filters.publishYear || ''))
  params.append('title', filters.title || '')
  filters.subjects.forEach((x) => params.append('subjects', x))
  params.append('page', String(filters.page))
  params.append('pageSize', String(filters.pageSize))

  apiClient
    .get('/List', {
      params,
    })
    .then((response) => {
      books.value = response.data
    })
    .catch((error) => {
      if (!axios.isCancel(error)) {
        errorMessage.value = 'An error happened while fetching the books: ' + error.response.data
      }
    })
    .finally(() => {
      isLoadingBooks.value = false
      isFirstFetch.value = false
    })
}
</script>

<template>
  <div class="book-container">
    <div role="alert" v-show="errorMessage" class="error">{{ errorMessage }}</div>
    <div v-show="!errorMessage">
      <div v-show="isLoadingBooks" class="spinner-container">
        <div class="spinner"></div>
        <div>Searching books...</div>
      </div>
      <div v-show="!isLoadingBooks && books.length > 0">
        <ul>
          <BookCard v-for="book in books" :key="book.id" :book="book" />
        </ul>
        <div class="pagination-controls">
          <button
            role="button"
            aria-label="Go to previous page"
            @click="$emit('previous-page')"
            :disabled="filters.page == 1"
          >
            Previous
          </button>
          <span>Page {{ filters.page }}</span>
          <button
            role="button"
            aria-label="Go to next page"
            @click="$emit('next-page')"
            :disabled="books.length < filters.pageSize"
          >
            Next
          </button>
        </div>
      </div>
      <div v-show="!isLoadingBooks && books.length == 0" class="not-found">No books found</div>
    </div>
  </div>
</template>

<style>
.book-container {
  height: calc(95vh - var(--header-height));
  overflow-y: scroll;
}

.spinner-container {
  display: flex;
  flex-direction: column;
  align-items: center;
}

.spinner {
  border: 4px solid transparent;
  border-top: 4px solid var(--color-lightblue);
  border-radius: 50%;
  width: 50px;
  height: 50px;
  animation: spin 0.5s linear infinite;
  margin: 50px auto;
}

@keyframes spin {
  0% {
    transform: rotate(0deg);
  }
  100% {
    transform: rotate(360deg);
  }
}

.pagination-controls {
  display: flex;
  justify-content: center;
  align-items: center;
  margin-top: 20px;
}

.pagination-controls button {
  padding: 10px 20px;
  font-size: 1rem;
  margin: 0 10px;
  cursor: pointer;
  background-color: #007bff;
  color: #fff;
  border: none;
  border-radius: 5px;
  transition: background-color 0.3s;
}

.pagination-controls button:disabled {
  background-color: #ccc;
  cursor: not-allowed;
}

.pagination-controls span {
  font-size: 1rem;
  margin: 0 10px;
}

.not-found {
  padding: 20px;
  border-radius: 5px;
  margin: 50px;
  background-color: #eee;
  font-weight: bold;
  text-align: center;
}

.error {
  padding: 20px;
  border-radius: 5px;
  margin: 50px;
  color: red;
  background-color: rgb(239, 150, 150);
  font-weight: bold;
  text-align: center;
}
</style>
