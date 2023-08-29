<template>
  <div class="my-5">
    <div class="input-group w-auto">
      <input type="text" class="form-control" placeholder="Enter a short path or url"/>
      <button class="btn btn-primary" type="button">Search</button>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

import "bootstrap/dist/css/bootstrap.css";
import "bootstrap/dist/js/bootstrap.js";

export default defineComponent({
  name: "SearchBar",
  setup() {
    return [];
  },
});
</script>
