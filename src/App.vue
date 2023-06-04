<template>
  <div v-if="!this.$store.state.appLoaded">Loading data..</div>
  <div v-if="this.$store.state.appLoaded">
    <div class="testing">

      <div class="key-wrapper">

      <entryPicker
        :dataSet="this.$store.state.dungeonList"
        itemBind="dungeon_name"
        itemName="Dungeon"
        imageBind="era_img"
        pickerTitle="Select Dungeon.."
        @itemSelected="selectDungeon"
        controlWidth="500px"
      />

      <keyPicker
        @keySelected="selectKeyLevel"
      />

    </div>

      <div class="affix-wrapper">

        <entryPicker
        :dataSet="this.$store.state.affixList"
        itemBind="affix_name"
        itemName="Affix"
        imageBind="affix_icon"
        pickerTitle="Select Affix 1"
        @itemSelected="selectAffix"
      />
      <entryPicker
        :dataSet="this.$store.state.affixList"
        itemBind="affix_name"
        itemName="Affix"
        imageBind="affix_icon"
        pickerTitle="Select Affix 2"
        @itemSelected="selectAffix"
      />
      <entryPicker
        :dataSet="this.$store.state.affixList"
        itemBind="affix_name"
        itemName="Affix"
        imageBind="affix_icon"
        pickerTitle="Select Affix 3"
        @itemSelected="selectAffix"
      />
      </div>

      <entryPicker
        :dataSet="this.$store.state.playerChars"
        itemBind="charName"
        itemName="Dungeon"
        imageBind="spec_img"
        pickerTitle="Who is the keystone owner?"
        @itemSelected="selectPlayer"
        controlWidth="500px"
      />
    </div>
  </div>

  <div v-if="apiError">{{ apiError }}</div>
</template>

<script>
//get axios ref
import axios from "axios";
//component refs
import keyPicker from "./components/keyPicker.vue";
import entryPicker from "./components/entryPicker.vue";

//main app stuff here
export default {
  //component dependencies
  components: {
    keyPicker,
    entryPicker,
  },

  //on initializaion of main app..
  mounted() {
    //check all resolved
    let playersLoaded = this.loadPlayerChars().then(
      (res) =>
        (this.$store.state.playerChars = JSON.parse(
          JSON.parse(res.data.d).message
        ))
    );
    let affixesLoaded = this.loadAffixList().then(
      (res) =>
        (this.$store.state.affixList = JSON.parse(
          JSON.parse(res.data.d).message
        ))
    );
    let dungeonsLoaded = this.loadDungeonList().then(
      (res) =>
        (this.$store.state.dungeonList = JSON.parse(
          JSON.parse(res.data.d).message
        ))
    );
    let specsLoaded = this.loadSpecs().then(
      (res) =>
        (this.$store.state.specList = JSON.parse(
          JSON.parse(res.data.d).message
        ))
    );

    //Only begin once all data is loaded
    Promise.all([
      playersLoaded,
      affixesLoaded,
      dungeonsLoaded,
      specsLoaded,
    ]).then((res) => {
      this.$store.state.appLoaded = true;
      // console.log(
      //   this.$store.state.playerChars,
      //   this.$store.state.dungeonList,
      //   this.$store.state.affixList,
      //   this.$store.state.specList
      // );
    });
  },

  //V-model bindings here for the selected options
  data() {
    return {
      //selected client items
      selectedDungeon: null,
      selectedKeyStoneLevel: 0,
      selectedAffix1: 0,
      selectedAffix2: 0,
      selectedAffix3: 0,
      selectedKeyOwner: null,

      //returned Group Info from API
      groupCompData: null,
      apiError: null,
    };
  },

  //Main app methods and stuff..
  methods: {
    //API call to the player Characters Google Doc
    async loadPlayerChars() {
      return await axios.post(this.$store.state.webServiceURL + "playerChars", {
        contentType: "application/json",
      });
    },

    //API call to the dungeon list
    async loadDungeonList() {
      return await axios.post(this.$store.state.webServiceURL + "dungeonList", {
        contentType: "application/json",
      });
    },

    //API call to the Affix list
    async loadAffixList() {
      return await axios.post(this.$store.state.webServiceURL + "affixList", {
        contentType: "application/json",
      });
    },

    //API call to the Spec List
    async loadSpecs() {
      return await axios.post(this.$store.state.webServiceURL + "specStats", {
        contentType: "application/json",
      });
    },

    //Submit the group to the API
    async getGroup() {
      await axios
        .post(this.$store.state.webServiceURL + "computedGroup", {
          //apply the parameters
          contentType: "application/json",
          dungeonName: this.selectedDungeon,
          keystoneLevel: this.selectedKeyStoneLevel,
          affix_1_id: this.selectedAffix1,
          affix_2_id: this.selectedAffix2,
          affix_3_id: this.selectedAffix3,
          keystone_owner : this.selectedKeyOwner
        })
        .then((res) => {
          //setup the return response
          let response = JSON.parse(res.data.d);
          let response_status = response.status;
          let response_data = response.message;
          //check the status
          //success
          if (response_status == "success") {
            this.groupCompData = JSON.parse(response.message);
          }
          //error
          if (response_status == "fail") {
            this.apiError = response.message;
          }
        });
    },

    //Component eMit methods
    //DungeonSelector Component fires dungeon Selected Event
    selectDungeon(dungeon) {
      //set the selected Dungeon
      this.selectedDungeon = dungeon;
      //testing
      console.log(this.selectedDungeon);
    },
    //Keystone Selector Component fires the keystone selected event
    selectKeyLevel(keyLevel) {
      //set the selected key
      this.selectedKeyStoneLevel = keyLevel;
      //testing
      console.log(this.selectedKeyStoneLevel);
    },
    //Player selector component fires the selctedPlayer event
    selectPlayer(charname) {
      //set the selected key
      this.selectedKeyOwner = charname;
      //testing
      console.log(this.selectedKeyOwner);
    },
    //Affix selector component fires the selected Affix event
    selectAffix(affix) {
      //set the selected key
      this.selectedAffix1 = affix;
      //testing
      console.log(this.selectedAffix1);
    },

  },
};
</script>

<style lang="scss">
/* ===== Scrollbar CSS ===== */
/* Firefox */
* {
  scrollbar-width: thin;
  scrollbar-color: var(--a-accent-1) #333333;
}

/* Chrome, Edge, and Safari */
*::-webkit-scrollbar {
  width: 16px;
}

*::-webkit-scrollbar-track {
  background: #333333;
}

*::-webkit-scrollbar-thumb {
  background-color: var(--a-accent-1);
  border-radius: 10px;
  border: 0px none #333333;
}
/*root vars*/
:root {
  --a-dark-1: #121212;
  --a-dark-2: #3d3d3d;
  --a-dark-2-alternate: #494949;
  --a-dark-3: #1b1b1b;

  --a-accent-1: #ecdb6f;
  --a-accent-2: #837a3e;
  --a-accent-3: #686868;
}
body {
  background: var(--a-dark-1);
  margin: 0;
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
  color: white;
}
.testing {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  margin: 5rem;

  .affix-wrapper{
    display:flex;
    flex-direction: row;
    gap:3rem;
  }

  .key-wrapper{
    display:flex;
    flex-direction: row;
    gap:3rem;
  }
}

.fade-enter-from,
.fade-leave-to {
  opacity: 0;
}
.fade-enter-to,
.fade-leave-from {
  opacity: 1;
}
.fade-enter-active,
.fade-leave-active {
  transition: all 200ms ease;
}
</style>