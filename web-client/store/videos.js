const initState = () => ({
  uploadPromise: null,
  active: false
});

export const state = initState

export const mutations = {
  toggleActivity(state, {status}) {
    state.active = !state.active
  },
  setTask(state, {uploadPromise}) {
    state.uploadPromise = uploadPromise
  },
  reset(state){
    Object.assign(state, initState())
  }
};

export const actions = {
  startVideoUpload({commit, dispatch}, {form}){
    const uploadPromise = this.$axios.$post("/api/videos", form);
    commit("setTask", {uploadPromise})
  },
  async createTrick({commit, dispatch}, {trick}){
    await this.$axios.post("/api/tricks", trick)
    await dispatch('fetchTricks')
  }
};
