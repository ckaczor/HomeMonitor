<script setup lang="ts">
    import { computed, ref } from 'vue';
    import { useHomeAssistantStore } from '@/stores/homeAssistantStore';

    const ready = ref(false);

    const homeAssistantStore = useHomeAssistantStore();
    homeAssistantStore.start();

    const sortedAlarmEntities = computed(() => {
        return Object.entries(homeAssistantStore.alarmEntities)
            .map(([key, value]) => ({
                key,
                ...value
            }))
            .sort((a, b) => {
                const deviceClassA = a.attributes.device_class || '';
                const deviceClassB = b.attributes.device_class || '';

                if (deviceClassA === deviceClassB) {
                    const nameA = a.attributes.friendly_name || '';
                    const nameB = b.attributes.friendly_name || '';

                    return nameA.localeCompare(nameB);
                }

                return deviceClassA.localeCompare(deviceClassB);
            });
    });

    function translateAlarmState(state: string): string {
        switch (state) {
            case 'on':
                return 'Open';
            case 'off':
                return 'Closed';
            default:
                return 'Unknown';
        }
    }

    ready.value = true;
</script>

<template>
    <div
        class="alarm-overview"
        v-if="ready">
        <div class="alarm-header">Doors and Windows</div>
        <div v-for="alarmEntity in sortedAlarmEntities">
            <div
                class="alarm-device"
                v-if="alarmEntity.attributes.device_class === 'door'">
                <v-icon
                    class="kiosk-alarm-device-icon"
                    :icon="alarmEntity.state === 'on' ? 'mdi-door-open' : 'mdi-door-closed'"
                    :class="{ 'kiosk-alarm-device-open': alarmEntity.state === 'on' }" />

                {{ alarmEntity.attributes.friendly_name }}

                <div class="alarm-device-state">
                    {{ translateAlarmState(alarmEntity.state) }}
                </div>
            </div>
            <div
                class="alarm-device"
                v-if="alarmEntity.attributes.device_class === 'window'">
                <v-icon
                    class="kiosk-device-icon"
                    :icon="alarmEntity.state === 'on' ? 'mdi-window-open' : 'mdi-window-closed'"
                    :class="{ 'kiosk-alarm-device-open': alarmEntity.state === 'on' }" />

                {{ alarmEntity.attributes.friendly_name }}

                <div class="alarm-device-state">
                    {{ translateAlarmState(alarmEntity.state) }}
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
    .alarm-overview {
        background-color: #121212;
        border-radius: 10px;
        display: flex;
        flex: 1;
        flex-direction: column;
        padding: 10px;
    }

    .alarm-header {
        font-size: 1.15em;
        padding-bottom: 2px;
        text-align: center;
    }

    .kiosk-alarm-device-open {
        color: #d09f27;
    }

    .alarm-device {
        padding: 4px 0;
    }

    .alarm-device-state {
        float: right;
    }
</style>
