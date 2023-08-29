<template>
  <table class="table table-bordered table-stripped">
    <thead v:if="urls">
      <tr>
        <th>LongUrl</th>
        <th>ShortPath</th>
        <th>CreatedDate</th>
        <th>LastReqestedDate</th>
        <th>RequestCounter</th>
        <th>ExpireDate</th>
      </tr>
    </thead>
  </table>
</template>


<script lang="ts">
import { defineComponent } from "vue";
import axios from "axios";


export default defineComponent({
  name: "UrlTable",
  setup() {
    return {
        urls: []
    };
  },
  created() {
      axios.get(`${process.env.VUE_APP_API_GATEWAY_URL}/get_all_urls`).then((response) => (this.urls = response.data))
  },
});
</script>
