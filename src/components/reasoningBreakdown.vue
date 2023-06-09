<template>
    <!-- Reasoning section for individual reasons -->
    <h1>Reasoning:</h1>
    <div class="section" rs>
      <div class="section-wrapper" rs>
        <table>
          <thead>
            <tr>
              <td>Player</td>
              <td>Reasoning</td>
            </tr>
          </thead>
          <tbody>
            <tr v-for="player in groupCompData" :key="player.player">
              <td charname>
                <div class="reasons">
                  <img :src="player.icon" />
                  <div class="player-lst">
                    <span ply>{{ player.player }}</span>
                    <span :style="'color:' + player.color">{{
                      player.char
                    }}</span>
                  </div>
                </div>
              </td>
              <td reasons>
                <div class="reasons">
                  <div
                    class="reason"
                    v-for="reason in JSON.parse(player.reasoning)"
                    :key="reason.reason"
                    :isKeyOwner="reason.reason == 'Keystone Owner'"
                  >
                    <span>{{ reason.reason }}</span>
                    <img :src="'src/assets/' + reason.reason_icon" />
                  </div>
                </div>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>

</template>

<script>


export default {
    //Data supplied to the component
    props:['groupCompData'],
};
</script>

<style lang="scss" scoped>
/*table style management - include mobile version when completed */
table {
  width: 100%;
  border-collapse: collapse;
  tr:nth-child(even) {
    background: rgba(0, 0, 0, 0.253);
  }
  td {
    padding: 0.75rem;
    border: solid 1px black;
    &[charname] {
      font-size: 120%;
    }
  }
  thead {
    background: rgba(0, 0, 0, 0.562);
    td {
      border: 0;
      border-right: solid 1px black;
    }
  }
}
/*reason flex container and icon distribution thingy */
.reasons {
  display: flex;
  flex-direction: row;
  justify-content: flex-start;
  align-items: center;
  gap: 1rem;

  /*Each individual reason is not its own component, change this */
  .reason {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    background: var(--a-dark-1);
    border-radius: 1rem;
    gap: 0.5rem;
    padding: 0.5rem;

    &[isKeyOwner="true"] {
      background: #e48015;
    }

    span {
      font-size: 80%;
    }
  }

  /*image size limiter */
  img {
    border-radius: 50%;
    height: 40px;
  }
}
/*player lst is not a list of players, its just a container that lets the name and class sit on top of each other */
.player-lst {
  display: flex;
  flex-direction: column;

  /*override the size on the name to be smaller */
  span {
    &[ply] {
      font-size: 80%;
    }
  }
}
</style>