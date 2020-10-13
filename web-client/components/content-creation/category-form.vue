<template>
  <v-card>
    <v-card-title>Create Category</v-card-title>
    <v-card-text>
      <v-text-field label="Name" v-model=""></v-text-field>
      <v-text-field label="Description" v-model=""></v-text-field>
    </v-card-text>
    <v-card-actions></v-card-actions>
  </v-card>
</template>

<script>
  const initState = () => ({
    form: {
      name: "",
      description: "",
    },
  });

    export default {
      name: "category-form",
      data: initState,
      methods: {
        save() {
          this.$axios.post("/api/categories", this.form)
        }
      }
    }
</script>

<style scoped>

</style>
