<template>
  <div class="input-group w-auto">
    <input
      type="text"
      class="form-control"
      placeholder="Enter a short path or url"
      v-model="input"
    />
    <button
      class="btn btn-primary"
      type="button"
      @click.prevent="GetOrPublishUrl(input.toString())"
    >
      Search
    </button>
  </div>
  <UrlDetail v-if="url.type == null" :url-with-detail="url" />
</template>

<script lang="ts">
import axios from "axios";
import { defineComponent, PropType } from "vue";
import Url from "../types/Url";
import UrlDetail from "./UrlDetail.vue";

export default defineComponent({
  name: "SearchBar",
  components: {
    UrlDetail,
  },
  data() {
    return {
      input: "",
      url: {
        required: false,
        type: Object as PropType<Url> | null,
        default: null,
      },
    };
  },
  methods: {
    GetOrPublishUrl(input: string) {
      axios
        .get(
          `${process.env.VUE_APP_API_GATEWAY_URL}/get_url/${encodeURIComponent(
            input
          )}`
        )
        .then((response) => {
          this.url = response.data;
          if (response.data["longUrl"] == null) {
            axios.post(`${process.env.VUE_APP_API_GATEWAY_URL}/publish_url`, {
              Url: input,
              ExpireDate: this.$store.state.number,
              IsPublic: this.$store.state.isPublic,
            }).then((response_from_post) => {
              this.url = response_from_post.data;
            });
          }
        });

    },
  },
});
</script>
