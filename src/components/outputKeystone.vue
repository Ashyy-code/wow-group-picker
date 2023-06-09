<template>
  <h1>Selected Keystone</h1>
  <div class="section">
    {{ selectedAffix1 }}<br />
    {{ selectedAffix2 }}<br />
    {{ selectedAffix3 }}<br />
    {{ selectedDungeon }}<br />
    {{ selectedPlayer1 }}<br />
    {{ selectedPlayer2 }}<br />
    {{ selectedPlayer3 }}<br />
    {{ selectedPlayer4 }}<br />
    {{ selectedPlayer5 }}<br />
  </div>
</template>

<script>
export default {
  //required external data
  props: [
    "selectedDungeon",
    "selectedKeyStoneLevel",
    "selectedAffix1",
    "selectedAffix2",
    "selectedAffix3",
    "selectedPlayer1",
    "selectedPlayer2",
    "selectedPlayer3",
    "selectedPlayer4",
    "selectedPlayer5",
  ],
};
</script>

<style lang="scss" scoped>
</style>