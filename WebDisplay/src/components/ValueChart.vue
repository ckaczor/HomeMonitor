<script setup lang="ts">
    import { ApexOptions } from 'apexcharts';

    const props = defineProps({
        type: { type: String, required: true },
        title: { type: String, required: true },
        unit: { type: String, required: true },
        categories: { type: Array<number>, required: true },
        series: { type: Array<{ name: string; data: Array<number> }>, required: true },
        yAxisDecimalPoints: { type: Number, required: false, default: 0 },
        valueDecimalPoints: { type: Number, required: false, default: 2 },
        group: { type: String, required: false, default: undefined },
        stepline: { type: Boolean, required: false, default: false },
        ready: { type: Boolean, required: true },
        yAxisMinimum: { type: Number, required: false, default: undefined },
        yAxisMaximum: { type: Number, required: false, default: undefined },
        tickAmount: { type: Number, required: false, default: undefined },
        lineSize: { type: Number, required: false, default: 2 },
        markerSize: { type: Number, required: false, default: 0 },
        yAxisLabelFormatter: { type: Function, required: false, default: undefined },
        yAxisValueFormatter: { type: Function, required: false, default: undefined }
    });

    var chartOptions: ApexOptions = {
        chart: {
            id: props.title,
            animations: {
                enabled: false
            },
            group: props.group,
            toolbar: {
                tools: {
                    pan: false
                }
            }
        },
        title: {
            text: props.title,
            align: 'left'
        },
        legend: {
            itemMargin: {
                vertical: 5
            },
            showForSingleSeries: true
        },
        tooltip: {
            x: {
                format: 'MMMM d h:mm TT'
            },
            y: {
                formatter: (value) => {
                    if (props.yAxisValueFormatter) {
                        return props.yAxisValueFormatter(value);
                    }

                    return `${value.toFixed(props.valueDecimalPoints)}${props.unit}`;
                }
            }
        },
        xaxis: {
            type: 'datetime',
            categories: props.categories,
            tooltip: {
                enabled: false
            },
            labels: {
                datetimeUTC: false,
                datetimeFormatter: {
                    day: 'MMM d',
                    hour: 'h:mm TT'
                }
            }
        },
        yaxis: {
            labels: {
                formatter: (value) => {
                    if (props.yAxisLabelFormatter) {
                        return props.yAxisLabelFormatter(value);
                    }

                    return `${value.toFixed(props.yAxisDecimalPoints)}${props.unit}`;
                }
            },
            tickAmount: props.tickAmount,
            min: props.yAxisMinimum,
            max: props.yAxisMaximum
        },
        stroke: {
            width: props.lineSize,
            curve: props.stepline ? 'stepline' : 'smooth'
        },
        markers: {
            size: props.markerSize
        },
        dataLabels: {
            enabled: false
        }
    };

    var chartSeries: ApexAxisChartSeries = props.series;
</script>

<template>
    <div class="chart">
        <v-container
            v-show="!props.ready"
            class="fill-height loading">
            <v-responsive class="align-center text-center fill-height">
                <v-progress-circular
                    :size="50"
                    :width="5"
                    color="primary"
                    indeterminate></v-progress-circular>
            </v-responsive>
        </v-container>
        <apexchart
            width="100%"
            height="250"
            :type="props.type"
            :options="chartOptions"
            :series="chartSeries"></apexchart>
    </div>
</template>

<style scoped>
    .loading {
        width: 100%;
        min-height: 250px;
        position: absolute;
        top: 0;
        left: 0;
        right: 0;
        bottom: 0;
        z-index: 99;
        background-color: #eeeeee;
    }

    .chart {
        position: relative;
        border: 1px solid lightgray;
        background-color: white;
        padding-top: 10px;
        padding-right: 10px;
        border-radius: 3px;
    }
</style>
