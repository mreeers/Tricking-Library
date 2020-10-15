export default {
  data: () => ({
    filter: "",
    tricks: [],
  }),


  computed: {

    filteredTricks() {
      if(!this.filter) return this.tricks;
      const normilze = this.filter.trim().toLowerCase();
      console.log(normilze)
      return this.tricks.filter(t => t.name.toLowerCase().includes(normilze) ||
        t.description.toLowerCase().includes(normilze));
    }
  }
}
