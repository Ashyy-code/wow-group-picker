<template>
  <!-- Player cards section for main output -->
  <div class="output-desc">
      <h1>Suggested Group Formation:</h1>
      <div class="suggested-group-breakdown">
        <playerOutputCard
          v-for="player in groupCompData"
          :key="player.player"
          :player="player"
        />
      </div>
    </div>

</template>

<script>
//import required components
import playerOutputCard from "./playerOutputCard.vue";

export default {
    //expected props
    props:['groupCompData'],
    //external component thingys
    components:{
        playerOutputCard
    }
}
</script>

<style lang="scss" scoped>
/*the main output wrap */
.output-desc {
  display: flex;
  flex-direction: column;
  /*inner wrap  */
  .suggested-group-breakdown {
    display: flex;
    flex-direction: row;
    justify-content: center;
    align-items: center;
    margin-top: 3rem;
    gap: 2rem;
  }
}
</style>