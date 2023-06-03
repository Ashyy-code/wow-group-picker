<template>
  <div class="picker-wrap">
    <i class="bx bx-chevron-down"></i>
    <div
      class="picker"
      @mousedown="showPicker = true"
      @blur="showPicker = false"
      tabindex="1"
    >
      <span v-if="!selectedOption">{{ preSelectedItem }}</span>
      <span v-if="selectedOption"><img :src="selectedOption[imageBind]" /> </span>
      <span selected class="option" v-if="selectedOption">{{ selectedOption[itemBind] }}</span>
    </div>
    <transition name="fade">
      <div class="picker-options-wrap" v-if="showPicker">
        <div class="options-expanded">
          <div
            v-for="(option, index) in dataSet"
            :key="index"
            class="option"
            @mousedown="
              selectedOption = option;
              showPicker = false;
            "
          >
          <span><img :src=option[imageBind] /></span>
          <span>{{ option[itemBind] }}</span>
            
          </div>
        </div>
      </div>
    </transition>
  </div>
</template>

<script>
export default {
  //external props
  props: ["dataSet", "itemBind", "preSelectedItem", "imageBind"],

  //initialize
  mounted(){
    this.selectedOption = this.dataSet.find(item => item[this.itemBind].toLowerCase() == this.preSelectedItem.toLowerCase());
  },

  //local component data
  data() {
    return {
      showPicker: false,
      selectedOption: null,
      selectedOptionImage:null,
    };
  },
};
</script>

<style lang="scss" scoped>
.picker-wrap {
  position: relative;
  max-width: 500px;

  .picker-options-wrap{
    background: var(--a-dark-2);
    border-radius: 0.5rem;
    margin-top: 0.75rem;
    padding: 0.5rem;
    outline: solid 3px var(--a-accent-1);
  }

  .options-expanded {
    max-height: 200px;
    overflow-y: auto;
    background: var(--a-dark-2);
    width: 100%;

    .option {
      color: white;
      padding: 0.25rem;
      user-select: none;
      cursor: pointer;
      outline: 0;
      display: flex;
      align-items: center;
      gap: 1rem;
      display:flex;
      align-items: center;
      text-transform: capitalize;


      img {
        height: 25px;
        width: 25px;
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

  i {
    position: absolute;
    top: 0.25rem;
    right: 0.5rem;
    color: var(--a-dark-1);
    font-size: 200%;
    pointer-events: none;
  }
  .picker {
    width: 100%;
    padding: 0.65rem;
    background: var(--a-dark-2);
    border: 0;
    border-radius: 0.5rem;
    width: calc(100% - 3.75rem);
    cursor: pointer;
    outline: none;
    font-size: 100%;
    color: white;
    padding-right: 3rem;
    white-space: nowrap;
    text-transform: capitalize;
    display:flex;
    align-items: center;
    gap:.5rem;
    padding-top:.75rem;

    img{
        width:25px;
        height:25px;
        border-radius: 50%;
    }

    &:hover {
      outline: solid 3px var(--a-accent-3);
    }

    &:focus {
      outline: solid 3px var(--a-accent-1);
    }
  }
}

</style>