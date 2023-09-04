<template>
  <div class="text-center">
    <div class="input-group w-auto">
      <input type="text" class="form-control" placeholder="Enter a short path or url" v-model="input" />
      <button class="btn btn-primary" type="button" @click.prevent="GetOrPublishUrl(input.toString())">
        Search
      </button>
    </div>
    <UrlDetail v-if="url != null && url.longUrl" :url-with-detail="url" @detail-shown="refreshTable" />
    <img v-if="isStarted" class="mt-4 img-fluid" src="../assets/loading-gif.gif" style="width: 48px" />
  </div>
</template>

<script lang="ts">
import axios from "axios";
import { defineComponent } from "vue";
import Url from "../types/Url";
import UrlTableRef from "../types/UrlTableRef";

import UrlDetail from "./UrlDetail.vue";

export default defineComponent({
  name: "SearchBar",
  components: {
    UrlDetail,
  },
  data() {
    return {
      input: "",
      isStarted: false,
      url: null as Url | null,
    };
  },
  methods: {
    refreshTable() {
  this.$emit('request-refresh');
},
    GetOrPublishUrl(input: string) {
      this.isStarted = true; // Start loading

      axios
        .get(
          `${process.env.VUE_APP_API_GATEWAY_URL}/get_url/${encodeURIComponent(
            input
          )}`
        )
        .then((response) => {
          this.url = response.data;
          if (this.url && this.url.longUrl == null) {
            return axios.post(
              `${process.env.VUE_APP_API_GATEWAY_URL}/publish_url`,
              {
                Url: input,
                ExpireDate: this.$store.state.number,
                IsPublic: this.$store.state.isPublic,
              }
            );
          } else {
            this.isStarted = false; // Stop loading
          }
        })
        .then((response_from_post) => {
          if (response_from_post && response_from_post.status == 200) {
            setTimeout(() => {
              axios
                .get(
                  `${process.env.VUE_APP_API_GATEWAY_URL
                  }/get_url/${encodeURIComponent(input)}`
                )
                .then((final_response) => {
                  this.url = final_response.data;
                  this.isStarted = false; // Stop loading
                });
            }, 6000);
          }
        })
        .catch((error) => {
          console.error("Error while fetching or publishing URL", error);
          this.isStarted = false; // Stop loading
        });
    },
  },
});
</script>
