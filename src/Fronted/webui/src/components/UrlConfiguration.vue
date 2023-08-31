<template>
  <div class="card">
    <div class="card-header fw-bold">Settings</div>
    <div class="container m-3">
      <div class="form-group row">
        <label for="numberInput" class="col-form-label col-sm-2"
          >Availability (InSecond):</label
        >

        <div class="col-sm-4">
          <input
            type="number"
            class="form-control"
            id="numberInput"
            v-model="this.$store.state.number"
            placeholder="Enter number"
          />
        </div>

        <div class="col-sm-5 offset-sm-1">
          <div class="form-check">
            <input
              class="form-check-input"
              type="checkbox"
              v-model="this.$store.state.isPublic"
              id="isPublic"
            />
            <label class="form-check-label" for="isPublic"> IsPublic </label>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent } from "vue";

export default defineComponent({
  name: "UrlConfiguration",
  data() {
    return {
      numberValue: 0,
      isPublic: true,
    };
  },
  watch: {
    numberValue(newVal) {
      if (newVal < 0) {
        this.numberValue = 0;
      }
    },
  },
});
</script>
