<script lang="ts" setup>
    import { ref } from 'vue';

    const emit = defineEmits(['longPress']);
    const props = defineProps(['duration', 'increment', 'progressSize']);

    const current = ref(0 as number);
    const loading = ref(false);

    let interval: NodeJS.Timeout;

    function mousedown() {
        loading.value = true;

        increment();

        interval = setInterval(increment, props.increment);
    }

    function mouseup() {
        reset();
    }

    function reset() {
        clearInterval(interval);

        loading.value = false;
        current.value = 0;
    }

    function increment() {
        current.value = current.value + props.increment;

        if (current.value >= props.duration) {
            emit('longPress');

            reset();
        }
    }

    function progress() {
        if (current.value >= props.duration) {
            return 100;
        }

        return (current.value / props.duration) * 100;
    }
</script>

<template>
    <button
        :class="{ 'loading-button': loading }"
        @mousedown="mousedown"
        @mouseup="mouseup">
        <span v-show="!loading">
            <slot name="default"></slot>
        </span>

        <v-progress-circular
            v-if="loading"
            :size="props.progressSize"
            :model-value="progress()"></v-progress-circular>
    </button>
</template>

<style scoped>
    .loading-button {
        display: inline-flex;
        flex-direction: column;
        align-items: center;
    }
</style>

<style>
    .v-progress-circular__underlay {
        stroke: #ebebeb !important;
        z-index: 1;
    }
</style>
