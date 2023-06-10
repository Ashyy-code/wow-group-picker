<template>
  <!--Player Selection Card stuff-->
  <div
    class="player"
    :lust="player.is_lust"
    :PI="player.is_PI"
    :CR="player.is_CR"
    :style="'background-image:url(' + card + ');'"
  >
    <!--Lust-->
    <span v-if="player.is_lust == 1" tp lust="1"
      ><img src="../assets/lust.jpg"
    /></span>
    <!--CR-->
    <span v-if="player.is_CR == 1" tp CR="1"
      ><img src="../assets/CR.jpg"
    /></span>
    <!--PI-->
    <span v-if="player.is_PI == 1" tp PI="1"
      ><img src="../assets/PI.jpg"
    /></span>
    <!--Useless class-->
    <span
      tp
      v-if="player.is_lust == 0 && player.is_CR == 0 && player.is_PI == 0"
      ><img src="../assets/none.jpg"
    /></span>
    <!--Player details-->
    <span rl>{{ player.role }}</span>
    <img :src="player.icon" />
    <span :style="'color:' + player.color" ch>{{ player.char }}</span>
  </div>
</template>

<script>
export default {
  //take the player from the prop
  props: ["player", "card"],
};
</script>

<style lang="scss" scoped>
.player {
  /*Main player card class*/
  background: var(--a-dark-3);
  background-repeat: no-repeat;
  background-size: cover;
  display: flex;
  flex-direction: column;
  justify-content: flex-start;
  align-items: center;
  gap: 0.75rem;
  border-radius: 0.5rem;
  width: 100%;
  position: relative;
  outline: solid 5px var(--a-accent-1);
  -webkit-box-shadow: 0px 0px 32px 3px rgba(0, 0, 0, 0.75);
  -moz-box-shadow: 0px 0px 32px 3px rgba(0, 0, 0, 0.75);
  box-shadow: 0px 0px 32px 3px rgba(0, 0, 0, 0.75);

  /*Inline span elements */
  span {
    padding: 0.25rem 1rem;
    width: 100%;
    text-align: center;

    /* added padding for the useless class */
    &[tp] {
      padding-top: 0.5rem;

      /*image fix */
      img {
        height: 40px;
        width: 40px;
        outline: solid 5px var(--a-accent-1);
        position: absolute;
        top: -20px;
        left: calc(50% - 20px);
      }
    }

    /*Independant span element styles  */
    &[rl] {
      font-size: 110%;
      color: var(--a-accent-1);
    }
    &[ch] {
      font-size: 150%;
      padding-bottom: 1rem;
    }
  }

  /*global styles for images in this component */
  img {
    border-radius: 50%;
  }
}
</style>