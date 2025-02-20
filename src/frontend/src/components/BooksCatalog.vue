<script setup lang="ts">
import { ref } from 'vue'
import { INITIAL_STATE } from '@/constants'
import BookFilters from './Filters/BookFilters.vue'
import BooksList from './Books/BooksList.vue'

const filterState = ref({ ...INITIAL_STATE })

function resetFilters() {
  filterState.value.authorName = INITIAL_STATE.authorName
  filterState.value.publishYear = INITIAL_STATE.publishYear
  filterState.value.subjects = INITIAL_STATE.subjects
  filterState.value.title = INITIAL_STATE.title
  filterState.value.page = INITIAL_STATE.page
  filterState.value.pageSize = INITIAL_STATE.pageSize
}

function resetPage() {
  filterState.value.page = INITIAL_STATE.page
}

function nextPage() {
  filterState.value.page++
}

function prevPage() {
  filterState.value.page--
}
</script>

<template>
  <main class="container">
    <section class="filters">
      <BookFilters
        v-model:author-name="filterState.authorName"
        v-model:publish-year="filterState.publishYear"
        v-model:subjects="filterState.subjects"
        v-model:title="filterState.title"
        @reset-filters="resetFilters"
        @reset-page="resetPage"
      />
    </section>
    <section class="main-content">
      <BooksList @next-page="nextPage" @previous-page="prevPage" :filters="filterState" />
    </section>
  </main>
</template>

<style>
.container {
  display: flex;
  margin: 2vh 8vw;
}

.filters {
  flex-basis: 35%;
}

.main-content {
  flex: 1;
}
</style>
