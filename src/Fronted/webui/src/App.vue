<template>
  <div class="container border m-5 py-3 px-5 rounded bg-light">
    <SearchBar/>
    <UrlTable />
  </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';
import SearchBar from './components/SearchBar.vue';
import UrlTable from './components/UrlTable.vue';

export default defineComponent({
  name: 'App',
  components: {
    SearchBar,
    UrlTable
  }
});
</script>
