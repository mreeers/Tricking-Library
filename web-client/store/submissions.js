const initState = () => ({
  submissions: []
});

export const state = initState;

export const mutations = {
  setSubmissions(state, {submissions}) {
    state.submissions = submissions
  },
  reset(state){
    Object.assign(state, initState())
  }
};

export const actions = {
  async fetchSubmissionsForTrick({commit}, {trickId}){
    const submissions = await this.$axios.$get(`http://localhost:5000/api/tricks/${trickId}/submissions`);
    commit("setSubmissions", {submissions})
  },

};
