<script lang="ts" setup>
    import { ref } from 'vue';

    const emit = defineEmits(['longPress']);
    const props = defineProps(['duration', 'increment', 'progressSize']);

    const current = ref(0 as number);
    const loading = ref(false);

    let interval: NodeJS.Timeout;

    function startProgress(pointerEvent: PointerEvent) {
        (pointerEvent.target as HTMLButtonElement).setPointerCapture(pointerEvent.pointerId);

        loading.value = true;

        incrementProgress();

        interval = setInterval(incrementProgress, props.increment);
    }

    function stopProgress() {
        resetProgress();
    }

    function resetProgress() {
        clearInterval(interval);

        loading.value = false;
        current.value = 0;
    }

    function incrementProgress() {
        current.value = current.value + props.increment;

        if (current.value >= props.duration) {
            emit('longPress');

            resetProgress();
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
        @pointerdown="startProgress"
        @pointerup="stopProgress">
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
        touch-action: none;
    }
</style>

<style>
    .v-progress-circular__underlay {
        stroke: #ebebeb !important;
        z-index: 1;
    }
</style>
