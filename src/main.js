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

      //UX
      appLoaded:false,
    }
  },
})

const app = createApp(App)
app.use(store)


app.mount('#app')
