<script setup lang="ts">
    import { usePowerStore } from '@/stores/powerStore';

    const powerStore = usePowerStore();
    powerStore.start();
</script>

<template>
    <DashboardItem title="Power">
        <div className="power-current">
            <div v-if="!powerStore.current">Loading...</div>
            <table v-else>
                <tbody>
                    <tr>
                        <td className="power-current-header">Generation</td>
                        <td>
                            {{
                                powerStore.current!.Generation < 0
                                    ? 0
                                    : powerStore.current!.Generation
                            }}
                            W
                        </td>
                    </tr>
                    <tr>
                        <td className="power-current-header">Consumption</td>
                        <td>
                            {{ powerStore.current!.Consumption }}
                            W
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </DashboardItem>
</template>

<style scoped>
    .power-current {
        font-size: 14px;
        padding: 6px 12px;
    }

    .power-current-header {
        font-weight: 500;
        text-align: right;
        padding-right: 10px;
    }
</style>
