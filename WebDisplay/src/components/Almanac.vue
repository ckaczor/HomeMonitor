<script lang="ts" setup>
    import DashboardItem from './DashboardItem.vue';
    import { useAlmanacStore } from '@/stores/almanacStore';
    import { format, formatDuration, intervalToDuration } from 'date-fns';

    const almanacStore = useAlmanacStore();
    almanacStore.load();

    const dayLength = (): string => {
        const duration = intervalToDuration({
            start: almanacStore.sunTimes!.sunrise,
            end: almanacStore.sunTimes!.sunset
        });

        return formatDuration(duration, { format: ['hours', 'minutes'] });
    };

    const moonPhaseName = (): string => {
        const phase = almanacStore.moonIllumination!.phase;

        if (phase === 0) {
            return 'New Moon';
        } else if (phase < 0.25) {
            return 'Waxing Crescent';
        } else if (phase === 0.25) {
            return 'First Quarter';
        } else if (phase < 0.5) {
            return 'Waxing Gibbous';
        } else if (phase === 0.5) {
            return 'Full Moon';
        } else if (phase < 0.75) {
            return 'Waning Gibbous';
        } else if (phase === 0.75) {
            return 'Last Quarter';
        } else if (phase < 1.0) {
            return 'Waning Crescent';
        }

        return '';
    };

    const moonPhaseLetter = (): string => {
        const phase = almanacStore.moonIllumination!.phase;

        if (phase === 0) {
            return '0';
        } else if (phase < 0.25) {
            return 'D';
        } else if (phase === 0.25) {
            return 'G';
        } else if (phase < 0.5) {
            return 'I';
        } else if (phase === 0.5) {
            return '1';
        } else if (phase < 0.75) {
            return 'Q';
        } else if (phase === 0.75) {
            return 'T';
        } else if (phase < 1.0) {
            return 'W';
        }

        return '';
    };
</script>

<template>
    <DashboardItem title="Almanac">
        <div className="almanac-content">
            <div v-if="!almanacStore.sunTimes || !almanacStore.moonIllumination">Loading...</div>
            <table v-else>
                <tbody>
                    <tr>
                        <td className="almanac-table-header">Sunrise</td>
                        <td colSpan="{2}">
                            {{ format(almanacStore.sunTimes.sunrise, 'hh:mm:ss aa') }}
                        </td>
                    </tr>
                    <tr>
                        <td className="almanac-table-header">Sunset</td>
                        <td colSpan="{2}">
                            {{ format(almanacStore.sunTimes.sunset, 'hh:mm:ss aa') }}
                        </td>
                    </tr>
                    <tr>
                        <td className="almanac-table-header">Day length</td>
                        <td colSpan="{2}">{{ dayLength() }}</td>
                    </tr>
                    <tr>
                        <td className="almanac-table-header">Moonrise</td>
                        <td colSpan="{2}">
                            {{ almanacStore.moonTimes?.rise ? format(almanacStore.moonTimes.rise, 'hh:mm:ss aa') : 'None' }}
                        </td>
                    </tr>
                    <tr>
                        <td className="almanac-table-header">Moonset</td>
                        <td colSpan="{2}">
                            {{ almanacStore.moonTimes?.set ? format(almanacStore.moonTimes.set, 'hh:mm:ss aa') : 'None' }}
                        </td>
                    </tr>
                    <tr>
                        <td className="almanac-table-header">Moon</td>
                        <td>
                            {{ moonPhaseName() }}
                            <br />
                            {{ (almanacStore.moonIllumination.fraction * 100).toFixed(1) }}% illuminated
                        </td>
                        <td>
                            <div className="moon-phase">
                                {{ moonPhaseLetter() }}
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </div>
    </DashboardItem>
</template>

<style scoped>
    @font-face {
        font-family: moon;
        src: url(/src/assets/moon_phases.ttf) format('opentype');
    }

    .almanac-content {
        font-size: 14px;
        padding: 6px 12px;
    }

    .almanac-table-header {
        font-weight: 500;
        text-align: right;
        padding-right: 10px;
        white-space: nowrap;
    }

    .moon-phase {
        font-family: moon;
        font-size: 28px;
        margin-left: 10px;
        display: block;
        margin-top: 1px;
    }
</style>
