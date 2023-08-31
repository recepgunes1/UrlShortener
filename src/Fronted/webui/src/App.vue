<template>
  <div class="container border mt-5 rounded bg-light">
    <div class="row mt-5">
      <div class="col">
        <SearchBar />
      </div>
    </div>
    <div class="row my-5">
      <div class="col">
        <UrlConfiguration />
      </div>
    </div>
    <div class="row">
      <div class="col">
        <UrlTable :urls="urls" />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref } from "vue";

import axios from "axios";

import SearchBar from "./components/SearchBar.vue";
import UrlTable from "./components/UrlTable.vue";
import UrlConfiguration from "./components/UrlConfiguration.vue";

import "bootstrap/dist/css/bootstrap.css";
import "bootstrap/dist/js/bootstrap.js";

export default defineComponent({
  name: "App",
  components: {
    SearchBar,
    UrlTable,
    UrlConfiguration,
  },
  setup() {
    const urls = ref([]);

    axios
      .get(`${process.env.VUE_APP_API_GATEWAY_URL}/get_all_urls`)
      .then((response) => {
        urls.value = response.data;
      });
    return {
      urls,
    };
  },
});
</script>
