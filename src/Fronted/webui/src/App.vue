<template>
  <div class="container border mt-5 rounded bg-light">
    <div class="row mt-5">
      <div class="col">
        <SearchBar @request-refresh="refreshUrlTable" />
      </div>
    </div>
    <div class="row my-5">
      <div class="col">
        <UrlConfiguration />
      </div>
    </div>
    <div class="row">
      <div class="col">
        <UrlTable ref="UrlTableRef" />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";

import SearchBar from "./components/SearchBar.vue";
import UrlTable from "./components/UrlTable.vue";
import UrlConfiguration from "./components/UrlConfiguration.vue";

import "bootstrap/dist/css/bootstrap.css";
import "bootstrap/dist/js/bootstrap.js";

export default defineComponent({
  name: "App",
  methods: {
    refreshUrlTable() {
      const urlTableRef = this.$refs.UrlTableRef as any; // or a more specific type if you have one
      if (urlTableRef && typeof urlTableRef.fetchUrls === 'function') {
        urlTableRef.fetchUrls();
      }
    }
  },
  components: {
    SearchBar,
    UrlTable,
    UrlConfiguration,
  },
});
</script>
