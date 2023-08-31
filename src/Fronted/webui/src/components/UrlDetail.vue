<template>
  <div
    class="alert alert-warning alert-dismissible fade show mt-5"
    role="alert"
  >
  <table class="table table-warning">
    <thead class="thead-dark text-center">
        <tr>
            <th scope="col" colspan="2">URL Details</th>
        </tr>
    </thead>
    <tbody>
        <tr>
            <th scope="row">Long Url</th>
            <td><a :href="urlWithDetail.longUrl">{{ urlWithDetail.longUrl }}</a></td>
        </tr>
        <tr>
            <th scope="row">Short Path</th>
            <td>{{ urlWithDetail.shortPath }}</td>
        </tr>
        <tr>
            <th scope="row">Created Date</th>
            <td>{{ urlWithDetail.createdDate }}</td>
        </tr>
        <tr>
            <th scope="row">Expire DateTime</th>
            <td>{{ urlWithDetail.expireDate }}</td>
        </tr>
    </tbody>
</table>
    <button
      type="button"
      class="btn-close"
      data-bs-dismiss="alert"
      aria-label="Close"
    ></button>
  </div>
</template>
  
<script lang="ts">
import { defineComponent, PropType } from "vue";
import Url from "../types/Url";

export default defineComponent({
  name: "UrlDetail",
  props: {
    urlWithDetail: {
      required: true,
      type: Object as PropType<Url>,
    },
  },
});
</script>
