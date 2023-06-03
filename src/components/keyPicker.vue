<template>
  <div class="key-picker-wrapper">
    <i class="bx bx-chevron-down"></i>
    <div
      class="key-picker"
      @mousedown="showPicker = true"
      @blur="showPicker = false"
      tabindex="1"
    >
      <span v-if="!selectedKey">Select Key Level</span>
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
</template>

<script>
export default {
  //events
  emits: ["keySelected"],

  //local component data
  data() {
    return {
      selectedKeyLevel: "Select Key Level",
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
.key-picker-wrapper {
  position: relative;
  max-width: 500px;

  i {
    position: absolute;
    top: 0.5rem;
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

    &:hover {
      outline: solid 3px var(--a-accent-3);
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
</style>