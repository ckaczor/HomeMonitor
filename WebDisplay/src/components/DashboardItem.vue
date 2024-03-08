<script setup lang="ts">
    defineProps(['title']);
</script>

<template>
    <div class="bg-primary dashboard-item-header">
        {{ title }}
    </div>
    <div class="dashboard-item-content">
        <slot></slot>
    </div>
</template>

<style scoped>
    .dashboard-item-header {
        padding: 2px 10px;
        border-top-left-radius: 3px;
        border-top-right-radius: 3px;
    }

    .dashboard-item-content {
        background-color: white;
        border: 1px solid lightgray;
        border-top: 0px;
        border-bottom-left-radius: 3px;
        border-bottom-right-radius: 3px;
    }
</style>
