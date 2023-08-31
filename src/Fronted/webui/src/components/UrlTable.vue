<template>
  <table class="table table-bordered table-stripped">
    <thead>
      <tr>
        <th>LongUrl</th>
        <th>ShortPath</th>
        <th>CreatedDate</th>
        <th>ExpireDate</th>
      </tr>
    </thead>
    <tbody>
      <tr v-for="(url, key) in urls" :key="key">
        <td>
          <a :href="url.longUrl">{{ url.longUrl }}</a>
        </td>
        <td>{{ url.shortPath }}</td>
        <td>{{ url.createdDate }}</td>
        <td>{{ url.expireDate }}</td>
      </tr>
    </tbody>
  </table>
</template>


<script lang="ts">
import { defineComponent, PropType } from "vue";

import Url from "../types/Url";

export default defineComponent({
  name: "UrlTable",
  props: {
    urls: {
      required: true,
      type: Array as PropType<Url[]>,
    }
  }
});
</script>
