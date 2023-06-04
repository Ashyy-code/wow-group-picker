<template>
  <div class="component-wrap">
  <div class="title">
    Keystone Level
  </div>
  <div class="key-picker-wrapper" :style="'width:' + controlWidth">
    <i class="bx bx-chevron-down"></i>
    <div
      class="key-picker"
      @mousedown="showPicker = true"
      @blur="showPicker = false"
      tabindex="1"
    >
      <span ph v-if="!selectedKey">-- Select --</span>
      <span selected class="option" v-if="selectedKey">{{ selectedKey }}</span>
    </div>
    <transition name="fade">
      <div v-if="showPicker" class="options-expanded">
        <div
          v-for="key in keyRange"
          :key="key"
          class="option"
          @mousedown="
            selectedKey = key;
            showPicker = false;
            this.$emit('keySelected',key);
          "
        >
          {{ key }}
        </div>
      </div>
    </transition>
  </div>
</div>
</template>

<script>
export default {
  //events
  emits: ["keySelected"],

  //props
  props: ["controlWidth"],

  //local component data
  data() {
    return {
      selectedKeyLevel:'',
      keyRange: [
        "<10",
        "+10-15",
        "+16",
        "+17",
        "+18",
        "+19",
        "+20",
        "+21",
        "+22",
        "+23",
        "+24",
      ],
      selectedKey: null,
      showPicker: false,
    };
  },
};
</script>

<style lang="scss" scoped>
.component-wrap{
  display:flex;
  flex-direction: column;
  gap:1rem;
}
.key-picker-wrapper {
  position: relative;

  i {
    position: absolute;
    top: 0.25rem;
    right: .5rem;
    color: var(--a-dark-1);
    font-size: 200%;
    pointer-events: none;
  }
  .key-picker {
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
    padding-right:3rem;
    white-space: nowrap;
    outline: solid 3px var(--a-accent-3);

    &:hover {
      outline: solid 3px var(--a-accent-2);
    }

    &:focus {
      outline: solid 3px var(--a-accent-1);
    }
  }

  .options-expanded {
    background: var(--a-dark-2);
    border-radius: 0.5rem;
    margin-top: 0.75rem;
    padding: 0.5rem;
    outline: solid 3px var(--a-accent-1);

    display: flex;
    flex-direction: row;
    flex-wrap: wrap;
    justify-content: stretch;
    gap: 1rem;

    position:absolute;
    width:100%;
    z-index: 9999;
  }
  .option {
    padding: 0.25rem 1rem;
    background: var(--a-dark-3);
    border-radius: 0.5rem;
    cursor: pointer;
    user-select: none;

    &[selected] {
      padding: .25rem 1rem;
      background: var(--a-accent-1);
      color: black;
      white-space: nowrap;
      &:hover {
        outline: none;
      }
    }

    &:hover {
      outline: solid 3px var(--a-accent-1);
    }
  }
}
span[ph]{
  color:#717171;
}
</style>