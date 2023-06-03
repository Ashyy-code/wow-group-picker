<template>
  <div class="dungeon-picker-wrapper">
    <i class="bx bx-chevron-down"></i>
    <input
      type="text"
      @focus="
        showPopup = true;
        $event.target.select();
      "
      :showAsFocused="itemFocused"
      @blur="checkBlur()"
      placeholder="Start Typing.."
      v-model="selectedDungeon"
      aria-label="Select Dungeon"
      @keyup="handleKeyInput($event)"
    />
    <transition name="fade">
      <div v-if="showPopup" class="picker-options-wrap">
        <div class="picker-options">
          <div
            v-for="(dungeon, index) in filteredDungeonList"
            :key="index"
            class="option"
            tabindex="0"
            @mousedown="selectDungeon(dungeon)"
            @focus="itemFocused = true"
            @blur="
              itemFocused = false;
              checkBlur();
            "
            @keydown="handleKey($event, dungeon)"
            :aria-label="dungeon.dungeon_name"
          >
            <img :src="'https://ashypls.com/wowzers/img/' + dungeon.era + '.png'">
            {{ dungeon.dungeon_name }}
          </div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script>
export default {
  //props expeced for this component
  props: ["dungeonData"],

  //events
  emits: ["dungeonSelected"],

  //initialization
  mounted() {
    //set the initial list to all dungeons
    this.filteredDungeonList = this.dungeonData;
  },

  //local component variables
  data() {
    return {
      //filtered list
      filteredDungeonList: [],

      //UX
      showPopup: false,
      selectedDungeon: null,
      itemFocused: false,
    };
  },

  //events and methods and stuff lol
  methods: {
    //set the selected dungeon
    selectDungeon(dungeon) {
      this.selectedDungeon = dungeon.dungeon_name;
      this.showPopup = false;
      this.itemFocused = false;
      this.$emit("dungeonSelected", dungeon);
    },
    //accessibility stuff
    checkBlur() {
      setTimeout(() => {
        if (!this.itemFocused) {
          this.showPopup = false;
        }
      }, 300);
    },
    //more accessibility stuff
    handleKey(e, dungeon) {
      if (e.code == "Enter" || e.code == "NumpadEnter") {
        this.selectDungeon(dungeon);
      }
    },
    //checking filtering the list
    handleKeyInput(e) {
      //abort if accessibility keys are pressed
      if(e.code == "Tab" || e.code == "Enter" || e.code == "NumpadEnter"){
        return;
      }
      //accessibility stuff
      if(e.code == "ArrowDown"){
        document.querySelector('.option').focus();
        return;
      }
      //clear the initial
      this.filteredDungeonList = [];
      //push the matches
      this.dungeonData.forEach(dungeon =>{
        if (dungeon.dungeon_name.toLowerCase().includes(this.selectedDungeon.toLowerCase())){
          this.filteredDungeonList.push(dungeon);
        }
      })  
    },
  },
};
</script>

<style lang="scss" scoped>
.dungeon-picker-wrapper {
  max-width: 500px;
  position: relative;

  i {
    position: absolute;
    top: .25rem;
    right: .5rem;
    color: var(--a-dark-1);
    font-size: 200%;
    pointer-events: none;
  }

  input {
    padding: 0.75rem;
    background: var(--a-dark-2);
    border: 0;
    border-radius: 0.5rem;
    width: calc(100% - 3.75rem);
    cursor: pointer;
    outline: none;
    font-size: 100%;
    color: white;
    padding-right:3rem;

    &:hover{
      outline: solid 3px var(--a-accent-3);
    }

    &[showAsFocused="true"] {
      outline: solid 3px var(--a-accent-1);
    }

    &:focus {
      outline: solid 3px var(--a-accent-1);
    }
  }

  .picker-options-wrap {
    background: var(--a-dark-2);
    border-radius: 0.5rem;
    margin-top: 0.75rem;
    padding: 0.5rem;
    outline: solid 3px var(--a-accent-1);

    .picker-options {
      max-height: 200px;
      overflow-y: auto;
      background: var(--a-dark-2);
      width: 100%;

      .option {
        color: white;
        padding: 0.75rem;
        user-select: none;
        cursor: pointer;
        outline: 0;
        display:flex;
        align-items: center;
        gap:1rem;

        img{
          height:20px;
          width:20px;
        }

        &:focus {
          background: var(--a-accent-1);
          color: black;
        }

        &:hover {
          background: var(--a-dark-3);
        }

        &:nth-child(odd) {
          background: var(--a-dark-2-alternate);
          &:hover {
            background: var(--a-dark-3);
          }
          &:focus {
            background: var(--a-accent-1);
            color: black;
          }
        }
      }
    }
  }
}

</style>