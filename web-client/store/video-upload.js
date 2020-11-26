﻿const initState = () => ({
  uploadPromise: null,
  uploadCancelSource: null,
  uploadCompleted: false,
  active: false,
  component: null
});

export const state = initState;

export const mutations = {
  activate(state, {component}) {
    state.active = true;
    state.component = component;
  },
  hide(state) {
    state.active = false;
  },
  setTask(state, {uploadPromise, source}) {
    state.uploadPromise = uploadPromise;
    state.uploadCancelSource = source;
  },
  completedUpload(state) {
    state.uploadCompleted = true;
  },
  reset(state){
    Object.assign(state, initState())
  }
};

export const actions = {
  startVideoUpload({commit, dispatch}, {form}){
    const source = this.$axios.CancelToken.source();
    const uploadPromise = this.$axios.$post("/api/videos", form, {
      progress: false,
      cancelToken: source.token
    })
      .then(({data}) => {
        commit('completedUpload');
        return data
      })
      .catch(err => {
        if(this.$axios.isCancel(err)){
          //todo popup notify
        }
      });

    commit("setTask", {uploadPromise, source});
  },
  async cancelUpload({state, commit}){
    if(state.uploadPromise){
      if(state.uploadCompleted){
        commit('hide');
        const video = await state.uploadPromise;
        await this.$axios.delete("/api/videos" + video)
      } else {
        state.uploadCancelSource.cancel()
      }
    }

    commit('reset');
  },
  async createSubmission({state, commit, dispatch}, {form}){
    if (!state.uploadPromise) {
      console.log("uploadPromise is null");
      return;
    }
    form.video = await state.uploadPromise;
    await this.createSubmission({
      form: this.form
    });
    await  dispatch('submissions/createSubmission', {form}, {root: true});
    commit('reset');
  }
};
