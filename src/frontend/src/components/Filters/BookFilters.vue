<script setup lang="ts">
import { onMounted, ref } from 'vue'
import { apiClient } from '@/clients'

const emit = defineEmits<{ (e: 'reset-filters'): void; (e: 'reset-page'): void }>()

function resetPage() {
  emit('reset-page')
}

const title = defineModel<string | null>('title', {
  set(value: string[] | string | number | null) {
    resetPage()
    return value
  },
})
const authorName = defineModel<string | null>('author-name', {
  set(value: string[] | string | number | null) {
    resetPage()
    return value
  },
})
const subjects = defineModel<string[]>('subjects', {
  set(value: string[] | string | number | null) {
    resetPage()
    return value
  },
})
const publishYear = defineModel<number | string>('publish-year', {
  set(value: string[] | string | number | null) {
    resetPage()
    return value
  },
})

const availableSubjects = ref<string[]>([])
const isLoadingSubjects = ref(true)

onMounted(() => {
  getAvailableSubjects()
})

async function getAvailableSubjects() {
  return apiClient
    .get<string[]>('/Subjects')
    .then((response) => (availableSubjects.value = response.data))
    .catch((err) => console.error('Error while fetching subject data', err))
    .finally(() => (isLoadingSubjects.value = false))
}
</script>

<template v-if="!isLoadingSubjects">
  <form>
    <fieldset>
      <legend>Filter books</legend>
      <div class="filter-container">
        <div>
          <label for="title">Title</label>
          <input id="title" v-model.trim="title" />
        </div>
        <div>
          <label for="authorName">Author</label>
          <input id="authorName" v-model.trim="authorName" />
        </div>
        <div>
          <label for="subjects">Subject</label>
          <select id="subjects" v-model="subjects" multiple>
            <option v-for="subject in availableSubjects" :value="subject" :key="subject">
              {{ subject }}
            </option>
          </select>
        </div>
        <div>
          <label for="publishYear">Publish Year</label>
          <input id="publishYear" type="number" v-model.number="publishYear" />
        </div>
        <div class="btn-container">
          <button
            role="button"
            aria-label="Reset filters"
            class="reset-btn"
            type="button"
            @click="$emit('reset-filters')"
          >
            Reset Filters
          </button>
        </div>
      </div>
    </fieldset>
  </form>
</template>

<style>
fieldset {
  display: flex;
  flex-direction: column;
  padding: 25px 15px;
  background-color: var(--color-background-filters);
  border-radius: 8px;
  border: 1px solid #ccc;
}

.filter-container {
  display: flex;
  flex-direction: column;
  gap: 25px;
  padding: 0 2px;
}

.filter-container div {
  display: flex;
  flex-direction: column;
}

legend {
  font-size: 1.2em;
  font-weight: bold;
}

label {
  font-size: 0.95rem;
  font-weight: bold;
}

input,
select {
  padding: 8px;
  font-size: 0.95rem;
  border-radius: 8px;
  border: 1px solid #ccc;
  outline: none;
  transition: all 0.3s ease;
}

select {
  height: 200px;
}

input:focus {
  border-color: var(--color-lightblue);
  box-shadow: 0 0 5px rgba(0, 123, 255, 0.5);
}

.btn-container {
  display: flex;
  justify-content: center;
  align-items: center;
}

.reset-btn {
  border: 1px solid darkblue;
  background-color: white;
  color: darkblue;
  padding: 10px 20px;
  font-size: 1rem;
  border-radius: 4px;
  cursor: pointer;
  max-width: 50%;
}
</style>
