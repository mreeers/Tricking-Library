﻿const initState = () => ({
  tricks: [],
  categories: [],
  difficulties: [],
});

export const state = initState;

export const getters = {
  trickById: state => id => state.tricks.find(x => x.slug === id),
  categoryById: state => id => state.categories.find(x => x.slug === id),
  difficultyById: state => id => state.difficulties.find(x => x.slug === id),
  trickItems: state => state.tricks.map(x => ({
    text: x.name,
    value: x.slug
  })),
  categoryItems: state => state.categories.map(x => ({
    text: x.name,
    value: x.slug
  })),
  difficultyItems: state => state.difficulties.map(x => ({
    text: x.name,
    value: x.slug
  }))
};

export const mutations = {
  setTricks(state, {tricks, difficulties, categories}) {
    state.tricks = tricks;
    state.difficulties = difficulties;
    state.categories = categories;
  },
  reset(state) {
    Object.assign(state, initState())
  }
};

export const actions = {
  async fetchTricks({commit}) {
    try{
      const tricks = await this.$axios.$get("/api/tricks");
      const difficulties = await this.$axios.$get("/api/difficulties");
      const categories = await this.$axios.$get("/api/categories");
      console.log(tricks, difficulties, categories);
      commit("setTricks", {tricks, difficulties, categories})
    } catch (e) {
      console.log(e)
    }

  },
  createTrick({state, commit, dispatch}, {form}) {
    return this.$axios.$post("/api/tricks", form)
  },
  updateTrick({state, commit, dispatch}, {form}) {
    return this.$axios.$put("/api/tricks", form)
  }
};
