<template>
  <h1>Selected Keystone</h1>
  <!--Keystone wrapp Flex-->
  <div class="keystone-wrapper">
    <!--Key Item-->
    <div class="ky-item" ks>
      <h3>Keystone</h3>
      <span klvl>{{ selectedKeyStoneLevel }}</span>
      <span dng>{{ selectedDungeon.dungeon_name }}</span>
    </div>

    <!--Key Item-->
    <div class="ky-item">
      <h3>Owner</h3>
      <span><img :src="selectedPlayer1.class_img" alt="" /></span>
      <span :Style="'color:' + selectedPlayer1.spec_color" pnm>{{ selectedPlayer1.charName }}</span>
    </div>

    <!--Key Item-->
    <div class="ky-item" gr>
      <h3>Affixes</h3>
      <!--row wrap for affixes-->
      <div class="affixes-wrap">
        <!--affix item-->
        <div class="affix">
          <img :src="selectedAffix1.affix_icon" />
          <span>{{ selectedAffix1.affix_name }}</span>
        </div>

        <!--affix item-->
        <div class="affix">
          <img :src="selectedAffix2.affix_icon" />
          <span>{{ selectedAffix2.affix_name }}</span>
        </div>

        <!--affix item-->
        <div class="affix">
          <img :src="selectedAffix3.affix_icon" />
          <span>{{ selectedAffix3.affix_name }}</span>
        </div>
      </div>
    </div>

    <!--Key Item-->
    <div class="ky-item">
      <h3>Players</h3>
      <span>{{ selectedPlayer2.name || "Any"  }}</span>
      <span>{{ selectedPlayer3.name || "Any"  }}</span>
      <span>{{ selectedPlayer4?.name || "Any" }}</span>
      <span>{{ selectedPlayer5?.name || "Any"  }}</span>
    </div>
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
.keystone-wrapper {
  display: flex;
  flex-direction: row;
  gap: 2rem;


  h3 {
    padding: 0;
    margin: 0;
    color: var(--a-accent-1);
    text-align: center;
  }

  .ky-item {
    background: var(--a-dark-1-ts);
    padding: 1rem;
    border-radius: 0.75rem;
    display: flex;
    flex-direction: column;
    gap: .5rem;
    outline: solid 5px var(--a-accent-1-ts);
    min-width:120px;
    justify-content: space-between;
    align-items: center;

    &[ks]{
      max-width:150px;
    }

    &[gr]{
      flex-grow:1;
    }

    span{
     text-transform: capitalize;
     text-align: center;


     &[klvl]{
      font-size:220%;
      font-weight: bold;
     }

     &[dng]{
      font-size:120%;
     }

     &[pnm]{
      font-size:120%;
     }

    }

    /*image size limiter */
    img {
      height: 50px;
      width: 50px;
      border-radius: 50%;
      outline:solid 5px rgba(0, 0, 0, 0.699);
    }
  }
}

.affixes-wrap{
  display:flex;
  flex-direction: row;
  justify-content: center;
  align-items: center;
  gap:2rem;

  .affix{
    display:flex;
    flex-direction: column;
    gap:1rem;
    justify-content: center;
    align-items: center;
  }
}
</style>