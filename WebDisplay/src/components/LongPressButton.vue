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
        clearInterval(interval);

        loading.value = false;
        current.value = 0;
    }

    function incrementProgress() {
        current.value = current.value + props.increment;

        if (current.value >= props.duration) {
            emit('longPress');

            stopProgress();
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
        class="long-press-button"
        :class="{ loading: loading }"
        @pointerdown="startProgress"
        @pointerup="stopProgress"
        @pointercancel="stopProgress">
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
    .long-press-button {
        touch-action: none;
    }

    .loading {
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
