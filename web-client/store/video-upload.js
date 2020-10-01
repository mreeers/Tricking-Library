import {UPLOAD_TYPE} from "~/data/enum";

const initState = () => ({
  uploadPromise: null,
  active: false,
  type: "",
  step: 1,
})



export const state = initState

export const mutations = {
  toggleActivity(state) {
    state.active = !state.active
    if(!state.active){
      Object.assign(state, initState())
    }
  },
  setType(state, {type}) {
    state.type = type
    if(type === UPLOAD_TYPE.TRICK) {
      state.step++
    } else if(type === UPLOAD_TYPE.SUBMISSION) {
      state.step += 2;
    }

  },
  setTask(state, {uploadPromise}) {
    state.uploadPromise = uploadPromise
    state.step++
  },
  reset(state){
    Object.assign(state, initState())
  }
}

export const actions = {
  startVideoUpload({commit, dispatch}, {form}){
    const uploadPromise = this.$axios.$post("/api/videos", form);
    commit("setTask", {uploadPromise})
  },
  async createTrick({commit, dispatch}, {trick}){
    await this.$axios.post("/api/tricks", trick)
    await dispatch('tricks/fetchTricks')
    state().step++
  }
}
