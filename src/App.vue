<template>
  <div v-if="!this.$store.state.appLoaded">Loading data..</div>
  <div v-if="this.$store.state.appLoaded">
    <button @click="getGroup()">Test the API</button>
    <div v-if="groupCompData">
      {{ groupCompData }}
    </div>
    <div v-if="apiError">
      {{ apiError }}
    </div>
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
    let playersLoaded = this.loadPlayerChars().then(
      (res) =>(this.$store.state.playerChars = JSON.parse(JSON.parse(res.data.d).message))
    );
    let affixesLoaded = this.loadAffixList().then(
      (res) =>(this.$store.state.affixList = JSON.parse(JSON.parse(res.data.d).message))
    );
    let dungeonsLoaded = this.loadDungeonList().then(
      (res) =>(this.$store.state.dungeonList = JSON.parse(JSON.parse(res.data.d).message))
    );
    let specsLoaded = this.loadSpecs().then(
      (res) =>(this.$store.state.specList = JSON.parse(JSON.parse(res.data.d).message))
    );

    //Only begin once all data is loaded
    Promise.all([playersLoaded, affixesLoaded, dungeonsLoaded, specsLoaded]).then((res) => {
      this.$store.state.appLoaded = true;
      //testing output
      console.log(this.$store.state.dungeonList,this.$store.state.affixList,this.$store.state.playerChars,this.$store.state.specList)
    });
  },

  //V-model bindings here for the selected options
  data() {
    return {
      //selected client items
      selectedDungeon: null,
      selectedKeyStoneLevel: 0,
      selectedAffix1_id: 0,
      selectedAffix2_id: 0,
      selectedAffix3_id: 0,
      selectedPlayerName1: null,
      selectedPlayerName2: null,
      selectedPlayerName3: null,
      selectedPlayerName4: null,
      selectedPlayerName5: null,

      //returned Group Info from API
      groupCompData: null,
      apiError:null,
    };
  },

  //Main app methods and stuff..
  methods: {
    //API call to the player Characters Google Doc
    async loadPlayerChars() {
      return await axios.post(this.$store.state.webServiceURL + "playerChars", {contentType: "application/json",});
    },

    //API call to the dungeon list
    async loadDungeonList() {
      return await axios.post(this.$store.state.webServiceURL + "dungeonList", {contentType: "application/json",});
    },

    //API call to the Affix list
    async loadAffixList() {
      return await axios.post(this.$store.state.webServiceURL + "affixList", {contentType: "application/json",});
    },

    //API call to the Spec List
    async loadSpecs(){
      return await axios.post(this.$store.state.webServiceURL + "specStats", {contentType: "application/json",});
    },

    //Submit the group to the API
    async getGroup() {
      await axios
        .post(this.$store.state.webServiceURL + "computedGroup", {
          //apply the parameters
          contentType: "application/json",
          dungeonName: this.selectedDungeon,
          keystoneLevel: this.selectedKeyStoneLevel,
          affix_1_id: this.selectedAffix1_id,
          affix_2_id: this.selectedAffix2_id,
          affix_3_id: this.selectedAffix3_id,
          player_1_name: this.selectedPlayerName1,
          player_2_name: this.selectedPlayerName2,
          player_3_name: this.selectedPlayerName3,
          player_4_name: this.selectedPlayerName4,
          player_5_name: this.selectedPlayerName5,
        })
        .then((res) => {
          //setup the return response
          let response = JSON.parse(res.data.d);
          let response_status = response.status;
          let response_data = response.message;
          //check the status
          //success
          if(response_status == "success"){this.groupCompData = JSON.parse(response.message)}
          //error
          if(response_status == "fail"){this.apiError = response.message}
        });
    },
  },
};
</script>

<style lang="scss">
</style>