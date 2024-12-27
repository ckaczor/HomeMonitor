<script lang="ts" setup>
    const props = defineProps(['pressureDifference']);

    function rotationClass(pressureDifference: number | undefined) {
        if (!pressureDifference) {
            return '';
        } else if (Math.abs(pressureDifference) <= 1.0) {
            return '';
        } else if (pressureDifference > 1.0 && pressureDifference <= 2.0) {
            return 'up-low';
        } else if (pressureDifference > 2.0) {
            return 'up-high';
        } else if (pressureDifference < -1.0 && pressureDifference >= -2.0) {
            return 'down-low';
        } else if (pressureDifference < -2.0) {
            return 'down-high';
        }

        return '';
    }
</script>

<template>
    <span
        class="pressure-trend-arrow"
        :class="rotationClass(props.pressureDifference)"
        :title="'3 Hour Change: ' + props.pressureDifference.toFixed(1)">
        ➝
    </span>
</template>

<style scoped>
    .pressure-trend-arrow {
        display: inline-block;
    }

    .down-high {
        transform: rotate(60deg);
    }

    .down-low {
        transform: rotate(25deg);
    }

    .up-high {
        transform: rotate(-60deg);
    }

    .up-low {
        transform: rotate(-25deg);
    }
</style>
