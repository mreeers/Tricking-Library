﻿const initState = () => ({
  tricks: [],
  categories: [],
  difficulties: []
});

export const state = initState;

export  const getter = {
  trickItems: state => state.tricks.map(x => ({
    text: x.name,
    value: x.id
  }))
  };


export const mutations = {
  setTricks(state, {tricks, difficulties, categories}) {
      state.tricks = tricks,
      state.difficulties = difficulties,
      state.categories = categories
  },
  reset(state){
    Object.assign(state, initState())
  }
};

export const actions = {
  async fetchTricks({commit}){
    const tricks = await this.$axios.$get("/api/tricks");
    const difficulties = await this.$axios.$get("/api/difficulties");
    const categories = await this.$axios.$get("/api/categories");
    commit("setTricks", {tricks, difficulties, categories})
  },
  createTrick({state, commit, dispatch}, {form}){
        return this.$axios.$post("/api/tricks", form)
  }

};
