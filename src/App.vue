<template>
  <!-- LOADER SECTION -->
  <div class="loader" v-if="!this.$store.state.appLoaded">
    <span><i class="bx bx-loader bx-spin"></i><br />Working on it..</span>
  </div>

  <!-- SELECTION SCREEN -->
  <div v-if="!groupCompData" class="picking-stage">
    <div v-if="this.$store.state.appLoaded">
      <h1>Ashy's Group Picker</h1>
      <div class="app-wrap">
        <!-- KEYSTONE SELECTION SECTION-->
        <div class="section">
          <h2><i class="bx bxs-key"></i>Keystone</h2>
          <div class="section-wrapper">
            <entryPicker
              :dataSet="this.$store.state.dungeonList"
              itemBind="dungeon_name"
              itemName="Dungeon"
              imageBind="era_img"
              pickerTitle="Dungeon"
              @itemSelected="selectDungeon"
              controlWidth="500px"
            />

            <keyPicker @keySelected="selectKeyLevel" controlWidth="240px" />
          </div>
        </div>
        <!--END KEYSTONE SELECTION SECTION-->

        <!--AFFIX SELECTION SECTION-->
        <div class="section">
          <h2><i class="bx bxs-ghost"></i>Affixes</h2>
          <div class="section-wrapper">
            <entryPicker
              :dataSet="this.$store.state.affixList"
              itemBind="affix_name"
              itemName="Affix"
              imageBind="affix_icon"
              pickerTitle="Affix 1"
              @itemSelected="selectAffix1"
              ref="affixpicker1"
            />
            <entryPicker
              :dataSet="this.$store.state.affixList"
              itemBind="affix_name"
              itemName="Affix"
              imageBind="affix_icon"
              pickerTitle="Affix 2"
              @itemSelected="selectAffix2"
              ref="affixpicker2"
            />
            <entryPicker
              :dataSet="this.$store.state.affixList"
              itemBind="affix_name"
              itemName="Affix"
              imageBind="affix_icon"
              pickerTitle="Affix 3"
              @itemSelected="selectAffix3"
              ref="affixpicker3"
            />
          </div>
        </div>
        <!--END AFFIX SELECTION SECTION-->

        <!--PLAYER SELECTION SECTION-->
        <div class="section">
          <h2><i class="bx bxs-joystick"></i>Players</h2>
          <div class="section-wrapper">
            <entryPicker
              :dataSet="this.$store.state.playerChars"
              itemBind="charName"
              itemName="Player"
              imageBind="spec_img"
              pickerTitle="Keystone Owner"
              @itemSelected="selectPlayer"
              controlWidth="200px"
            />
            <entryPicker
              :dataSet="this.$store.state.playerList"
              itemBind="name"
              itemName="Player"
              imageBind="img"
              pickerTitle="Player 2"
              @itemSelected="selectPlayer2"
              controlWidth="200px"
            />
            <entryPicker
              :dataSet="this.$store.state.playerList"
              itemBind="name"
              itemName="Player"
              imageBind="img"
              pickerTitle="Player 3"
              @itemSelected="selectPlayer3"
              controlWidth="200px"
            />
            <entryPicker
              :dataSet="this.$store.state.playerList"
              itemBind="name"
              itemName="Player"
              imageBind="img"
              pickerTitle="Player 4"
              @itemSelected="selectPlayer4"
              controlWidth="200px"
            />
            <entryPicker
              :dataSet="this.$store.state.playerList"
              itemBind="name"
              itemName="Player"
              imageBind="img"
              pickerTitle="Player 5"
              @itemSelected="selectPlayer5"
              controlWidth="200px"
            />
          </div>
        </div>
        <!--END PLAYER SELECTION SECTION-->      
        <!--GO BUTTON SECTION-->
        <div class="section" btn>
          <div class="section-wrapper" btn>
            <button @click="getGroup()">
              Big button to click and make things go
            </button>
          </div>
        </div>
        <!--END GO BUTTON SECTION-->
      </div>
    </div>
  </div>

  <!-- OUTPUT OFFERINGS -->
  <!--Main app wrap for the reasoning breakdown-->
  <div class="app-wrap" v-if="groupCompData">
    <!--Lots of test data on the outputkeystone component, ignore-->
    <outputKeystoneVue
      :selectedAffix1="selectedAffix1"
      :selectedAffix2="selectedAffix2"
      :selectedAffix3="selectedAffix3"
      :selectedDungeon="selectedDungeon"
      :selectedKeyStoneLevel="selectedKeyStoneLevel"
      :selectedPlayer1="selectedPlayer1"
      :selectedPlayer2="selectedPlayer2"
      :selectedPlayer3="selectedPlayer3"
      :selectedPlayer4="selectedPlayer4"
      :selectedPlayer5="selectedPlayer5"
    />
    <outputBreakdownVue :groupCompData="groupCompData" />
    <reasoningBreakdown :groupCompData="groupCompData" />
  </div>

  <!-- ERROR NOTIFICATIONS -->
  <div v-if="apiError">{{ apiError }}</div>
</template>

<script>
//get axios ref
import axios from "axios";
//component refs
import keyPicker from "./components/keyPicker.vue";
import entryPicker from "./components/entryPicker.vue";
import reasoningBreakdown from "./components/reasoningBreakdown.vue";
import outputBreakdownVue from "./components/outputBreakdown.vue";
import outputKeystoneVue from "./components/outputKeystone.vue";

//main app stuff here
export default {
  //component dependencies
  components: {
    keyPicker,
    entryPicker,
    reasoningBreakdown,
    outputBreakdownVue,
    outputKeystoneVue,
  },

  //on initializaion of main app..
  mounted() {
    //check all resolved
    let playersLoaded = this.loadPlayerChars().then((res) => {
      this.$store.state.playerChars = JSON.parse(
        JSON.parse(res.data.d).message
      );

      //build players dataSet from the chars dataSet
      //blank it
      this.$store.state.playerList = [];

      //add the players
      this.$store.state.playerChars.forEach((char) => {
        let player = { name: char.player, img: char.class_img };
        if (
          this.$store.state.playerList.filter(
            (item) => item.name == player.name
          ).length < 1
        ) {
          this.$store.state.playerList.push(player);
        }
      });
    });
    //check all loaded - affixes
    let affixesLoaded = this.loadAffixList().then(
      (res) =>
        (this.$store.state.affixList = JSON.parse(
          JSON.parse(res.data.d).message
        ))
    );
    //check all loaded - dungeons
    let dungeonsLoaded = this.loadDungeonList().then(
      (res) =>
        (this.$store.state.dungeonList = JSON.parse(
          JSON.parse(res.data.d).message
        ))
    );
    //check all loaded - specs
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
      setTimeout(() => {
        //set the current affixes
        this.setInitialAffixes();

        // //testing methods
        // this.getGroup().then((res) => console.log(this.groupCompData));
      }, 200);
      // console.log(
      //   this.$store.state.playerChars,
      //   this.$store.state.dungeonList,
      //   this.$store.state.affixList,
      //   this.$store.state.specList,
      //   this.$store.state.playerList
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
      selectedPlayer1: null,
      selectedPlayer2: null,
      selectedPlayer3: null,
      selectedPlayer4: null,
      selectedPlayer5: null,

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
      //set the loading state
      this.$store.state.appLoaded = false;
      await axios
        .post(this.$store.state.webServiceURL + "computedGroup", {
          //apply the parameters
          contentType: "application/json",
          dungeonName: this.selectedDungeon.dungeon_name,
          keystoneLevel: this.selectedKeyStoneLevel,
          affix_1: this.selectedAffix1.affix_name,
          affix_2: this.selectedAffix2.affix_name,
          affix_3: this.selectedAffix3.affix_name,
          player_1_name: this.selectedPlayer1.charName,
          player_2_name: this.selectedPlayer2?.name || "n/a",
          player_3_name: this.selectedPlayer3?.name || "n/a",
          player_4_name: this.selectedPlayer4?.name || "n/a",
          player_5_name: this.selectedPlayer5?.name || "n/a",
        })
        .then((res) => {
          //set loaded
          this.$store.state.appLoaded = true;
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
        }).catch(err =>{
          this.apiError = err.message;
          this.$store.state.appLoaded = true;
        });
    },

    //Component eMit methods
    //DungeonSelector Component fires dungeon Selected Event
    selectDungeon(dungeon) {
      //set the selected Dungeon
      this.selectedDungeon = dungeon;
    },
    //Keystone Selector Component fires the keystone selected event
    selectKeyLevel(keyLevel) {
      //set the selected key
      this.selectedKeyStoneLevel = keyLevel;
    },
    //Player selector component fires the selctedPlayer event
    selectPlayer(charname) {
      //set the selected key
      this.selectedPlayer1 = charname;
    },
    selectPlayer2(charname) {
      //set the selected key
      this.selectedPlayer2 = charname;
    },
    selectPlayer3(charname) {
      //set the selected key
      this.selectedPlayer3 = charname;
    },
    selectPlayer4(charname) {
      //set the selected key
      this.selectedPlayer4 = charname;
    },
    selectPlayer5(charname) {
      //set the selected key
      this.selectedPlayer5 = charname;
    },
    //Affix selector component fires the selected Affix event
    selectAffix1(affix) {
      //set the selected key
      this.selectedAffix1 = affix;
    },
    selectAffix2(affix) {
      //set the selected key
      this.selectedAffix2 = affix;
    },
    selectAffix3(affix) {
      //set the selected key
      this.selectedAffix3 = affix;
    },
    //set the initial affixes
    setInitialAffixes() {
      let currentAffixes = [];
      this.$store.state.affixList.forEach((affix) => {
        if (affix.is_current == "True") {
          currentAffixes.push(affix);
        }
      });
      //console.log(currentAffixes);
      this.$refs.affixpicker1.selectItem(currentAffixes[0]);
      this.$refs.affixpicker2.selectItem(currentAffixes[1]);
      this.$refs.affixpicker3.selectItem(currentAffixes[2]);
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
  --a-dark-1: #0c0c0c;
  --a-dark-2: #3d3d3d;
  --a-dark-2-alternate: #494949;
  --a-dark-3: #1b1b1b;
  --a-section: #161616d9;

  --a-accent-1: #ecdb6f;
  --a-accent-2: #837a3e;
  --a-accent-3: #686868;
  --a-accent-4: #ecdb6f;

  --a-accent-1-ts: #ecdb6f8b;
  --a-dark-1-ts: #0c0c0cab;
}
/*Body and APP main generics */
body {
  background: url("https://ashypls.com/wowzers/img/group-bg.jpg");
  background-size: cover;
  margin: 0;
  font-family: "Segoe UI", Tahoma, Geneva, Verdana, sans-serif;
  color: white;
  font-size: 1rem;
  height: 100vh;
  width: 100vw;
  overflow: hidden;
}
#app {
  height: 100vh;
  overflow-y: scroll;
}

/*Section or container wrappers */
.section {
  background: var(--a-section);
  padding: 1rem;
  border-radius: 1rem;
  outline: solid 3px var(--a-accent-4);

  &[rs] {
    padding: 0;
    overflow: hidden;
    outline: solid 5px var(--a-dark-1);
    margin-bottom: 3rem;
  }

  &[btn] {
    background: none;
    outline: none;
  }

  /*more H overrides on padding */
  h2 {
    padding: 0;
    margin: 1rem;
    margin-left: 0;
    margin-top: 0;
    color: var(--a-accent-1);
    display: flex;
    align-items: center;

    i {
      margin-right: 0.5rem;
    }
  }

  /*inline sections individual and stuff */
  .section-wrapper {
    display: flex;
    flex-direction: row;
    gap: 1rem;
    flex-wrap: wrap;
    align-items: center;

    &[rs] {
      display: block;
    }

    /*The big button that makes everything go */
    button {
      color: black;
      background: var(--a-accent-1);
      border: 0;
      padding: 0.75rem;
      border-radius: 0.5rem;
      font-size: 100%;
      text-transform: uppercase;
      outline: solid 5px rgba(0, 0, 0, 0.688);
      cursor: pointer;
      margin: auto;
      /*hover styles for it */
      &:hover {
        background: black;
        color: var(--a-accent-1);
      }
    }

    /*image size limiter */
    img {
      max-height: 50px;
    }
  }
}
/*Remove padding and margin on all H eles */
h1 {
  padding: 0;
  margin: 0;
  text-align: center;
  margin-top: 2rem;
  text-shadow: 2px 2px 2px #000000;
}
/*app wrap used in various components, kept here to prevent duplication of styles */
.app-wrap {
  width: 100%;
  max-width: 980px;
  margin: auto;
  margin-top: 1rem;
  display: flex;
  flex-direction: column;
  gap: 2rem;
}

/*transitional effects added in root, used in multiple components */
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

/*the main loader - this will be tweaked later to cater for various loaders */
.loader {
  width: 100vw;
  height: 100vh;
  position: absolute;
  top: 0;
  left: 0;
  background: var(--a-section);
  display: grid;
  place-items: center;
  font-size: 200%;

  span {
    max-width: 400px;
    text-align: center;
  }

  i {
    font-size: 200%;
    margin-right: 1rem;
  }
}
</style>