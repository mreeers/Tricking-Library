<template>
  <div>
    <div>
      <p class="mb-1" v-html="comment.htmlContent"></p>
      <v-btn text v-if="!replying" @click="replying = true">reply</v-btn>
      <v-btn text v-if="$listeners['load-replies']" @click="$emit('load-replies')">Load replies</v-btn>
    </div>
    <comment-input v-if="replying" @send="(c) => $emit('send', c)" @cancel="replying = false" />
  </div>
</template>

<script>
  import CommentInput from "./comment-input";
  export default {
    name: "comment-body",
    components: {CommentInput},
    props: {
      comment: {
        required: true,
        type: Object
      }
    },
    data: () => ({
      replying: false,
    })
  }
</script>

<style scoped>

</style>
