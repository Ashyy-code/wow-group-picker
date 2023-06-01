<template>
  <div v-if="this.$store.state.appLoaded">
  Data has finished loading now..
  {{ this.$store.state.playerChars }}
  {{ this.$store.state.dungeonList }}
  {{ this.$store.state.affixList }}
</div>
</template>

<script>
//get axios ref
import axios from "axios";

//main app stuff here
export default {
  //on initializaion of main app..
  mounted() {
    //check all resolved
    let playersLoaded = this.loadPlayerChars().then(res => this.$store.state.playerChars = JSON.parse(JSON.parse(res.data.d).message));
    let affixesLoaded = this.loadAffixList().then(res => this.$store.state.affixList = JSON.parse(JSON.parse(res.data.d).message));
    let dungeonsLoaded = this.loadDungeonList().then(res => this.$store.state.dungeonList = JSON.parse(JSON.parse(res.data.d).message));

    Promise.all([playersLoaded,affixesLoaded,dungeonsLoaded]).then(
      res => {
        this.$store.state.appLoaded = true;
      }
    )
  },
  //Main app methods and stuff..
  methods: {

    //API call to the player Characters Google Doc
    async loadPlayerChars() {
      return await axios.post(this.$store.state.webServiceURL + "playerChars",{contentType: "application/json"});
    },

    //API call to the dungeon list
    async loadDungeonList() {
      return await axios.post(this.$store.state.webServiceURL + "dungeonList",{contentType: "application/json"});
    },

    //API call to the Affix list
    async loadAffixList() {
      return await axios.post(this.$store.state.webServiceURL + "affixList",{contentType: "application/json"});
    },
  },
};
</script>

<style lang="scss">
</style>