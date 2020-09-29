const initState = () => ({
    uploadPromise: null
})

export const state = initState;

export const mutations = {
    setTask(state, {task}) {
        state.uploadPromise = uploadPromise
    },
    reset(state) {
        Object.assign(state, initState())
    }
}

export const actions = {
    startVideoUpload({commit, dispatch}, {trick}) {
        const uploadPromise = this.$axios.$post("http://localhost:5000/api/videos", form);
        commit("setTask", {uploadPromise})
    }
}