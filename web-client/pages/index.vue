<template>
<div>
  <v-file-input @change="handleFile" accept="video/*" />


      <div v-if="tricks">
        <p v-for="t in tricks">
          {{t.name}}
        </p>
      </div>
<div>
  <v-stepper v-model="step">
      <v-stepper-header>
        <v-stepper-step
          :complete="step > 1"
          step="1"
        >
          Upload
        </v-stepper-step>
  
        <v-divider></v-divider>
  
        <v-stepper-step
          :complete="step > 2"
          step="2"
        >
          Trick Information
        </v-stepper-step>
  
        <v-divider></v-divider>
  
        <v-stepper-step step="3">
          Confirmation
        </v-stepper-step>
      </v-stepper-header>
  
      <v-stepper-items>
        <v-stepper-content step="1">
          <v-input accept="video/*" @change="handleFile" />
        </v-stepper-content>
  
        <v-stepper-content step="2">
          <div>
          <v-text-field label="Tricking Name" v-model="trickName"/>
          <v-btn @click="saveTrick">Save Trick</v-btn>
        </div>
        </v-stepper-content>
  
        <v-stepper-content step="3">
          <div>Success</div>
        </v-stepper-content>
      </v-stepper-items>
    </v-stepper>
</div>
</div>
</template>

<script>
import {mapState, mapActions, mapMutations} from 'vuex';

export default {
  data: () => ({
    trickName: "",
    step: 1
  }),
  computed: {
   ...mapState('tricks', ['tricks']),
   ...mapState('video', ['uploadPromise']),
  },
  methods: {
    ...mapMutations('videos', {
      resetVideos: 'reset'
    }),
    ...mapActions('tricks', ['createTrick']),
    ...mapActions('video', ['startVideoUpload']),
    
    async handleFile(file) {
      if(!file) return;

      const form = new FormData();
      form.append("video", file)
      this.startVideoUpload({form});
      this.step++;
    },
    async saveTrick() {
      if(!this.uploadPromise) {
        console.log('uploadPromise is null')
        return;
      }
      const video = await this.uploadPromise;
      await this.createTrick({trick: {name: this.trickName}});
      this.trickName = "";
      this.step++;
      this.resetVideos();
    }
  } 
}
</script>
