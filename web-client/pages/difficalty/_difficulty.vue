<template>
  <div class="d-flex mt-3 justify-center align-start">
    <div class="mx-2">
      <v-text-field v-model="filter" outlined label="Search" placeholder="e.g. cork/flip/kick" prepend-inner-icon="mdi-magnify" />
      <div v-for="t in tricks">
        {{t.id}} - {{t.name}} - {{t.description}}
      </div>
    </div>
    <v-sheet class="pa-3 mx-2 stricky" v-id="difficulty">
      <div class="text-h6">
        {{difficulty.name}}
      </div>
      <v-divider class="my-1" />
      <div class="text-body-2">
        {{difficulty.description}}
      </div>
    </v-sheet>
  </div>
</template>

<script>
  import {mapGetters} from 'vuex'
  import trickList from "../../mixins/trickList";

  export default {
    mixins: [trickList],
    data: () => ({
      difficulty: null,


    }),
    computed: {
      ...mapGetters('tricks', ['difficultyById']),

    },
    async fetch() {
      const difficultyId = this.$route.params.difficulty;
      this.difficulty = this.difficultyById(difficultyId);
      this.tricks = await this.$axios.$get(`/api/difficulties/${difficultyId}/tricks`);
    },
    head() {
      if(!this.difficulty) return {}
      return {
        title: this.difficulty.name,
        meta: [
          { hid: 'description', name: 'description', content: this.difficulty.description}
        ]
      }
    }
  }
</script>

<style scoped>

</style>
