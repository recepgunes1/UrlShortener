<template>
  <table class="table table-bordered table-striped">
    <thead>
      <tr>
        <th>LongUrl</th>
        <th>ShortPath</th>
        <th>CreatedDate</th>
        <th>ExpireDate</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="(url, key) in fetchedUrls" :key="key">
        <td>
          <a :href="url.longUrl">{{ url.longUrl }}</a>
        </td>
        <td><a :href="`${currentUrl}${url.shortPath}`" target="_blank">{{ url.shortPath }}</a></td>
        <td>{{ url.createdDate }}</td>
        <td>{{ url.expireDate }}</td>
      </tr>
    </tbody>
  </table>
</template>

<script lang="ts">
import { defineComponent, PropType, ref, onMounted } from "vue";
import axios from "axios";

import Url from "../types/Url";

export default defineComponent({
  name: "UrlTable",
  props: {
    urls: {
      type: Array as PropType<Url[]>,
      default: () => ([]),
    }
  },
  setup() {
    const currentUrl = ref(window.location.href);
    const fetchedUrls = ref<Url[]>([]);

    const fetchUrls = async () => {
      try {
        const response = await axios.get(`${process.env.VUE_APP_API_GATEWAY_URL}/get_all_urls`);
        fetchedUrls.value = response.data;
      } catch (error) {
        console.error("Failed to fetch URLs:", error);
      }
    };
    
    onMounted(fetchUrls);

    return {
      currentUrl,
      fetchedUrls
    };
  },
});
</script>
