<template>
  <div class="dungeon-picker-wrapper">
    <i class='bx bx-chevron-down'></i>
    <input
      type="text"
      @focus="showPopup = true; $event.target.select()"
      :showAsFocused="itemFocused"
      @blur="checkBlur()"
      placeholder="Start Typing.."
      v-model="selectedDungeon"
      aria-label="Select Dungeon"
    />
    <transition name="fade">
      <div v-if="showPopup" class="picker-options-wrap">
        <div class="picker-options">
          <div
            v-for="(dungeon, index) in dungeonData"
            :key="index"
            class="option"
            tabindex="0"
            @mousedown="selectDungeon(dungeon)"
            @focus="itemFocused = true"
            @blur="itemFocused = false;checkBlur();"
            @keydown="handleKey($event, dungeon)"
            :aria-label="dungeon.dungeon_name"
          >
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
  mounted() {},

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
      this.$emit("dungeonSelected",dungeon);
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
    handleKey(e,dungeon) {
      if (e.code == "Enter" || e.code == "NumpadEnter") {
       this.selectDungeon(dungeon);
      }
    },
  },
};
</script>

<style lang="scss" scoped>
.dungeon-picker-wrapper {
  max-width: 500px;
  padding: 2rem;
  position:relative;

  i{
    position:absolute;
    top:2.25rem;
    right:2.25rem;
    color:var(--a-dark-1);
    font-size:200%;
  }

  input {
    padding: 0.75rem;
    background: var(--a-dark-2);
    border: 0;
    border-radius: 0.5rem;
    width: calc(100% - 1.5rem);
    cursor: pointer;
    outline: none;
    font-size: 100%;
    color: white;

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
        padding: 0.5rem;
        user-select: none;
        cursor: pointer;
        outline: 0;

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
</style>