<template>
  <div class="component-wrap">
  <div class="title">
    {{ pickerTitle }}
  </div>
  <div class="picker-wrapper" :style="'width:' + controlWidth + 'max-width:100%;'">
    <i class="bx bx-chevron-down"></i>
    <img v-if="selectedItem" :src="selectedItem[imageBind]" />
    <input
      type="text"
      @focus="
        showPopup = true;
        $event.target.select();
      "
      :itsl="selectedItem != null"
      :showAsFocused="itemFocused"
      @blur="checkBlur()"
      placeholder="Start Typing.."
      v-model="selectedValue"
      :aria-label="'Select ' + itemName"
      @keyup="handleKeyInput($event)"
    />
    <transition name="fade">
      <div v-if="showPopup" class="picker-options-wrap">
        <div class="picker-options">
          <div
            v-for="(item, index) in filteredItemList"
            :key="index"
            class="option"
            tabindex="0"
            @mousedown="selectItem(item)"
            @focus="itemFocused = true"
            @blur="
              itemFocused = false;
              checkBlur();
            "
            @keydown="handleKey($event, item)"
            :aria-label="item[itemBind]"
          >
            <img :src="item[imageBind]" />
            {{ item[itemBind] }}
          </div>
        </div>
      </div>
    </transition>
  </div>
</div>
</template>

<script>
export default {
  //props expeced for this component
  props: ["dataSet", "itemName", "itemBind", "imageBind","pickerTitle","controlWidth"],

  //events
  emits: ["itemSelected"],

  //initialization
  mounted() {
    //set the initial list to all dungeons
    this.filteredItemList = this.dataSet;
  },

  //local component variables
  data() {
    return {
      //filtered list
      filteredItemList: [],

      //data
      selectedItem: null,
      selectedValue: "",
      selectedImage: "",

      //UX
      showPopup: false,
      itemFocused: false,
    };
  },

  //events and methods and stuff lol
  methods: {
    //set the selected item
    selectItem(item) {
      this.selectedItem = item;
      this.selectedValue = item[this.itemBind];
      this.selectedImage = item[this.imageBind];
      this.showPopup = false;
      this.itemFocused = false;
      this.$emit("itemSelected", item);
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
    handleKey(e, item) {
      if (e.code == "Enter" || e.code == "NumpadEnter") {
        this.selectItem(item);
      }
    },
    //checking filtering the list
    handleKeyInput(e) {
      //abort if accessibility keys are pressed
      if (e.code == "Tab" || e.code == "Enter" || e.code == "NumpadEnter") {
        return;
      }
      //accessibility stuff
      if (e.code == "ArrowDown") {
        document.querySelector(".option").focus();
        return;
      }
      //clear the initial
      this.filteredItemList = [];
      //push the matches
      this.dataSet.forEach((item) => {
        if (
          item[this.itemBind]
            .toLowerCase()
            .includes(this.selectedValue.toLowerCase())
        ) {
          this.filteredItemList.push(item);
        }
      });
    },
  },
};
</script>

<style lang="scss" scoped>
.component-wrap{
  display:flex;
  flex-direction: column;
  gap:1rem;
}
.picker-wrapper {
  position: relative;
  text-transform: capitalize;


  img {
    height: 25px;
    width: 25px;
    position:absolute;
    top:.5rem;
    left:.6rem;
    border-radius: 50%;
  }

  i {
    position: absolute;
    top: 0.25rem;
    right: 0.5rem;
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
    padding-right: 3rem;
    text-transform: capitalize;
    outline: solid 3px var(--a-accent-3);


    &[itsl="true"] {
      padding-left: 3rem;
      width: calc(100% - 6rem);
    }

    &:hover {
      outline: solid 3px var(--a-accent-2);
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
    position:absolute;
    z-index: 9999;
    width:calc(100% - 1rem);

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
        display: flex;
        align-items: center;
        gap: 1rem;

        img {
          height: 25px;
          width: 25px;
          position: static;
          border-radius: 50%;
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