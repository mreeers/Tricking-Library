<template>
  <v-row justify="center" align="center">
    <v-col cols="12" sm="8" md="6">
      <div class="text-center">
        <logo />
        <vuetify-logo />
      </div>
      <div v-if="tricks">
        <p v-for="t in tricks">
          {{t.name}}
        </p>
      </div>
      <div>
        <v-input label="Tricking Name" v-model="trickName"/>
      </div>
      {{message}}
      <v-btn @click="reset">Reset Message</v-btn>
      <v-btn @click="resetTricks">Reset Tricks</v-btn>
    </v-col>
  </v-row>
</template>

<script>
import Logo from '~/components/Logo.vue';
import VuetifyLogo from '~/components/VuetifyLogo.vue';
import Axios from "axios";
import {mapState, mapActions, mapMutations} from 'vuex';

export default {
  components: {
    Logo,
    VuetifyLogo
  },
  //data: () => ({
  //  message: ""
  //}),
  computed: {
   ...mapState({
    message: state => state.message
   }),
   ...mapState('tricks', {
    tricks: state => state.tricks
   }),
  },
  methods: {
    ...mapMutations(['reset']),
    ...mapMutations({
      resetTricks: 'reset'
    }),
    ...mapActions('tricks', 'createTricks'),
    async saveTrick() {
      await this.createTrick(this.trickName);
    }

  } 
  //async fetch() {
  //  await this.$store.dispatch('fetchMessage')
  //}
  //asyncData(payLoad) {
  //  return Axios.get("http://localhost:5000/api/home")
  //  .then(({data}) => {
  //    return {message: data}
  //    })
  //}
}
</script>
