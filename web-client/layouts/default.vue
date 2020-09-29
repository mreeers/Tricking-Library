<template>
  <v-app dark>
    <v-app-bar app dense>
      <v-toolbar-title>Tricking Library</v-toolbar-title>
      <v-spacer></v-spacer>
      <v-menu left bottom>
        <template v-slot:activator="{ on, attrs }">
          <v-btn depressed @click="toggleActivity">
            Upload
          </v-btn>
        </template>
        <v-list>
          <v-list-item @click="toggleActivity" >
            <v-list-item-title>Trick</v-list-item-title>
          </v-list-item>
          <v-list-item>
            <v-list-item-title>Submission</v-list-item-title>
          </v-list-item>
        </v-list>
      </v-menu>
    </v-app-bar>
    <v-main>
      <nuxt/>
    </v-main>
  </v-app>
</template>

<script>
  import {mapMutations} from 'vuex'
  export default {
    methods: mapMutations('videos', 'toggleActivity')
  }
</script>
