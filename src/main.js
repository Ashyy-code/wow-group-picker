import { createApp } from 'vue'
import App from './App.vue'

//Vuex is better than Pinia
import { createStore } from 'vuex'

// Global Variables
const store = createStore({
  state () {
    return {
      //Globals
      webServiceURL:'https://ashypls.com/endpoints/keyCheck.asmx/',

      //DataSets
      playerChars:[],
      dungeonList:[],
      affixList:[],
      specList:[],
      playerList:[],

      //selections
      selectedPlayer1:[],
      selectedPlayer2:[],
      selectedPlayer3:[],
      selectedPlayer4:[],
      selectedPlayer5:[],

      //UX
      appLoaded:false,

      
    }
  },
  getters:{
    playerListFiltered() {
      let newArr =  store.state.playerList;
      newArr = newArr.filter(
        (item) => item != store.state.selectedPlayer1
      );
      newArr = newArr.filter(
        (item) => item !=  store.state.selectedPlayer2
      );
      newArr = newArr.filter(
        (item) => item !=  store.state.selectedPlayer3
      );
      newArr = newArr.filter(
        (item) => item !=  store.state.selectedPlayer4
      );
      newArr = newArr.filter(
        (item) => item !=  store.state.selectedPlayer5
      );
      return newArr;
    },
  }
})

const app = createApp(App)
app.use(store)


app.mount('#app')
