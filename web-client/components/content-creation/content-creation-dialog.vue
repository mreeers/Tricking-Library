<template>
  <v-dialog :value="active" persistent >
    <template v-slot:activator="{on}">
      <v-btn depressed @click="toggleActivity">
        Upload
      </v-btn>
    </template>
    <trick-steps />
    <div class="d-flex justify-center my-4">
      <v-btn @click="toggleActivity">
        Close
      </v-btn>
    </div>
  </v-dialog>
</template>

<script>
  import {mapState, mapActions, mapMutations} from 'vuex';
  import {UPLOAD_TYPE} from "../../data/enum";
  import TrickSteps from "./trick-steps";

  export default {
    name: "content-creation-dialog",
    components: {TrickSteps},
    data: () => ({
      trickName: "",
      submission: "",
    }),
    computed: {
      ...mapState('video-upload', ['uploadPromise', 'active', 'step', 'type']),
      uploadType() {
        return UPLOAD_TYPE;
      }
    },
    methods: {
      ...mapMutations('video-upload', ['reset', 'incStep', 'toggleActivity', 'setType']),
      ...mapActions('video-upload', ['startVideoUpload', 'createTrick']),
    },
  }
</script>

<style scoped>

</style>
