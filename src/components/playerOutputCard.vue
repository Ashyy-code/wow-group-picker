<template>
  <div
          class="player"
          :lust="player.is_lust"
          :PI="player.is_PI"
          :CR="player.is_CR"
        >
          <span v-if="player.is_lust == 1" tp lust="1"
            ><img src="../assets/lust.jpg"
          /></span>
          <span v-if="player.is_CR == 1" tp CR="1"
            ><img src="../assets/CR.jpg"
          /></span>
          <span v-if="player.is_PI == 1" tp PI="1"
            ><img src="../assets/PI.jpg"
          /></span>
          <span
            tp
            v-if="player.is_lust == 0 && player.is_CR == 0 && player.is_PI == 0"
            ><img src="../assets/none.jpg"
          /></span>
          <span rl>{{ player.role }}</span>
          <img :src="player.icon" />
          <span :style="'color:' + player.color" ch>{{ player.char }}</span>
        </div>
</template>

<script>
export default {
    props:['player']

}
</script>

<style>

</style>