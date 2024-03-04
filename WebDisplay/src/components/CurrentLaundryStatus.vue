<script setup lang="ts">
    import { useLaundryStore } from '@/stores/laundryStore';

    const laundryStore = useLaundryStore();
    laundryStore.start();
</script>

<template>
    <DashboardItem title="Laundry">
        <div className="laundry-current">
            <div v-if="!laundryStore.current">Loading...</div>
            <table v-else>
                <tbody>
                    <tr v-if="laundryStore.current.dryer != null">
                        <td className="laundry-current-header">Dryer</td>
                        <td :className="laundryStore.current.dryer.toString()">
                            {{ laundryStore.current.dryer ? 'On' : 'Off' }}
                        </td>
                    </tr>
                    <tr v-if="laundryStore.current.washer != null">
                        <td className="laundry-current-header">Washer</td>
                        <td :className="laundryStore.current.washer.toString()">
                            {{ laundryStore.current.washer ? 'On' : 'Off' }}
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </DashboardItem>
</template>

<style>
    .laundry-current {
        font-size: 14px;
        padding: 6px 12px;
    }

    .laundry-current-header {
        font-weight: 500;
        text-align: right;
        padding-right: 10px;
    }

    .true {
        color: darkgoldenrod;
    }

    .false {
        color: darkgreen;
    }
</style>
