import { Component, OnInit } from '@angular/core';
import { Chart } from 'angular-highcharts';
import { SeriesLineOptions, SeriesWindbarbOptions, SeriesColumnOptions } from 'highcharts';
import { HttpClient } from '@angular/common/http';
import { WeatherReadingGrouped } from '../../../models/weather/weather-reading-grouped';
import { WindHistoryGrouped } from '../../../models/weather/wind-history-grouped';

import * as moment from 'moment';

import * as Highcharts from 'highcharts';
import HC_exporting from 'highcharts/modules/exporting';
import HC_windbarb from 'highcharts/modules/windbarb';
HC_exporting(Highcharts);
HC_windbarb(Highcharts);

enum TimeSpan {
    Last24Hours,
    Day,
    Custom
}

enum ChartType {
    Weather,
    Wind
}

@Component({
    selector: 'app-weather-charts',
    templateUrl: './weather-charts.component.html',
    styleUrls: ['./weather-charts.component.scss']
})
export class WeatherChartsComponent implements OnInit {

    public chart: Chart;
    public loading = true;
    public timeSpanItems: { [value: number]: string } = {};
    public timeSpans: typeof TimeSpan = TimeSpan;
    public maxDate: moment.Moment = moment().endOf('day');
    public selectedChartType: ChartType = ChartType.Weather;
    public ChartType = ChartType;

    private selectedTimeSpanValue: TimeSpan = TimeSpan.Last24Hours;
    private selectedDateValue: moment.Moment = moment().startOf('day');

    constructor(private httpClient: HttpClient) { }

    ngOnInit() {
        this.timeSpanItems[TimeSpan.Last24Hours] = 'Last 24 hours';
        this.timeSpanItems[TimeSpan.Day] = 'Day';

        this.loadChart();
    }

    public get selectedTimeSpan() {
        return this.selectedTimeSpanValue;
    }

    public set selectedTimeSpan(value) {
        this.selectedTimeSpanValue = value;

        this.loadChart();
    }

    public get selectedDate() {
        return this.selectedDateValue;
    }

    public set selectedDate(value) {
        this.selectedDateValue = value;

        this.loadChart();
    }

    public handleDateArrowClick(value: number) {
        this.selectedDate = moment(this.selectedDate).add(value, 'day');
    }

    public isSelectedDateToday(): boolean {
        const isToday = moment(this.selectedDate).startOf('day').isSame(moment().startOf('day'));

        return isToday;
    }

    public resetToToday() {
        this.selectedDate = moment().startOf('day');
    }

    public getSelectedDateDisplayString(): string {
        return moment(this.selectedDate).format('LL');
    }

    public chartTypeChange(value: ChartType) {
        if (this.selectedChartType === value) {
            return;
        }

        this.selectedChartType = value;

        this.loadChart();
    }

    private loadWeatherChart(start: moment.Moment, end: moment.Moment) {
        const startString = start.toISOString();
        const endString = end.toISOString();

        const request = this.httpClient.get<WeatherReadingGrouped[]>(`/api/weather/readings/history-grouped?start=${startString}&end=${endString}&bucketMinutes=30`);

        request.subscribe(data => {
            const seriesData: Array<SeriesLineOptions | SeriesColumnOptions> = [];

            seriesData.push({ type: 'line', name: 'Temperature', data: [], yAxis: 0, marker: { enabled: false }, tooltip: { valueSuffix: '°F' } } as SeriesLineOptions);
            seriesData.push({ type: 'line', name: 'Pressure', data: [], yAxis: 1, marker: { enabled: false }, tooltip: { valueSuffix: '"' } } as SeriesLineOptions);
            seriesData.push({ type: 'line', name: 'Humidity', data: [], yAxis: 2, marker: { enabled: false }, tooltip: { valueSuffix: '%' } } as SeriesLineOptions);
            seriesData.push({ type: 'line', name: 'Light', data: [], yAxis: 2, marker: { enabled: false }, tooltip: { valueSuffix: '%' } } as SeriesLineOptions);
            seriesData.push({ type: 'column', name: 'Rain', data: [], yAxis: 3, marker: { enabled: false }, tooltip: { valueSuffix: '"' } } as SeriesColumnOptions);

            data.forEach(dataElement => {
                const date = Date.parse(dataElement.bucket);
                seriesData[0].data.push([date, dataElement.averagePressureTemperature]);
                seriesData[1].data.push([date, dataElement.averagePressure / 33.864 / 100]);
                seriesData[2].data.push([date, dataElement.averageHumidity]);
                seriesData[3].data.push([date, dataElement.averageLightLevel * 100]);
                seriesData[4].data.push([date, dataElement.rainTotal]);
            });

            const title = this.selectedTimeSpan === TimeSpan.Last24Hours ? this.timeSpanItems[TimeSpan.Last24Hours] : this.getSelectedDateDisplayString();

            this.chart = new Chart({
                chart: {
                    zoomType: 'x'
                },
                title: {
                    text: title
                },
                credits: {
                    enabled: true
                },
                xAxis: {
                    type: 'datetime',
                    dateTimeLabelFormats: {
                        millisecond: '%H:%M:%S.%L',
                        second: '%H:%M:%S',
                        minute: '%H:%M',
                        hour: '%l:%M %P',
                        day: '%b %e',
                        week: '%e. %b',
                        month: '%b \'%y',
                        year: '%Y'
                    }
                },
                yAxis: [
                    {
                        labels: {
                            format: '{value:.2f}°F',
                        },
                        title: {
                            text: 'Temperature'
                        }
                    },
                    {
                        labels: {
                            format: '{value:.2f}"'
                        },
                        title: {
                            text: 'Pressure'
                        },
                        opposite: true
                    },
                    {
                        visible: true,
                        labels: {
                            format: '{value:.2f}%'
                        },
                        title: {
                            text: 'Percentage'
                        },
                        min: 0,
                        max: 100
                    },
                    {
                        labels: {
                            format: '{value:.2f}"'
                        },
                        title: {
                            text: 'Rain'
                        },
                        min: 0,
                        max: 0.25,
                        visible: true,
                        opposite: true
                    }
                ],
                time: {
                    useUTC: false
                },
                tooltip: {
                    valueDecimals: 2,
                    shared: true,
                    dateTimeLabelFormats: {
                        day: '%A, %b %e, %Y',
                        hour: '%A, %b %e, %H:%M',
                        millisecond: '%A, %b %e, %H:%M:%S.%L',
                        minute: '%A, %b %e, %l:%M %P',
                        month: '%B %Y',
                        second: '%A, %b %e, %H:%M:%S',
                        week: 'Week from %A, %b %e, %Y',
                        year: '%Y'
                    }
                },
                series: seriesData,
                legend: {
                    enabled: true
                },
                exporting: {
                    enabled: true
                }
            });

            this.loading = false;
        });
    }

    private loadWindChart(start: moment.Moment, end: moment.Moment) {
        const startString = start.toISOString();
        const endString = end.toISOString();

        const request = this.httpClient.get<WindHistoryGrouped[]>(`/api/weather/readings/wind-history-grouped?start=${startString}&end=${endString}&bucketMinutes=30`);

        request.subscribe(data => {
            const seriesData: Array<SeriesLineOptions | SeriesWindbarbOptions> = [];

            seriesData.push({ type: 'line', name: 'Minimum', data: [], yAxis: 0, marker: { enabled: false }, tooltip: { valueSuffix: ' MPH' } } as SeriesLineOptions);
            seriesData.push({ type: 'line', name: 'Average', data: [], yAxis: 0, marker: { enabled: false }, tooltip: { valueSuffix: ' MPH' } } as SeriesLineOptions);
            seriesData.push({ type: 'line', name: 'Maximum', data: [], yAxis: 0, marker: { enabled: false }, tooltip: { valueSuffix: ' MPH' } } as SeriesLineOptions);
            seriesData.push({ type: 'windbarb', name: 'Direction', data: [], marker: { enabled: false }, tooltip: { valueSuffix: ' MPH' } } as SeriesWindbarbOptions);

            data.forEach(dataElement => {
                const date = Date.parse(dataElement.bucket);
                seriesData[0].data.push([date, dataElement.minimumSpeed]);
                seriesData[1].data.push([date, dataElement.averageSpeed]);
                seriesData[2].data.push([date, dataElement.maximumSpeed]);
                seriesData[3].data.push([date, dataElement.averageSpeed, dataElement.averageDirection]);
            });

            const title = this.selectedTimeSpan === TimeSpan.Last24Hours ? this.timeSpanItems[TimeSpan.Last24Hours] : this.getSelectedDateDisplayString();

            this.chart = new Chart({
                chart: {
                    zoomType: 'x'
                },
                title: {
                    text: title
                },
                credits: {
                    enabled: true
                },
                xAxis: {
                    type: 'datetime',
                    dateTimeLabelFormats: {
                        millisecond: '%H:%M:%S.%L',
                        second: '%H:%M:%S',
                        minute: '%H:%M',
                        hour: '%l:%M %P',
                        day: '%b %e',
                        week: '%e. %b',
                        month: '%b \'%y',
                        year: '%Y'
                    },
                    offset: 50
                },
                yAxis: [
                    {
                        labels: {
                            format: '{value:.2f} MPH',
                        },
                        title: {
                            text: 'Wind Speed'
                        }
                    }
                ],
                time: {
                    useUTC: false
                },
                tooltip: {
                    valueDecimals: 2,
                    shared: true,
                    dateTimeLabelFormats: {
                        day: '%A, %b %e, %Y',
                        hour: '%A, %b %e, %H:%M',
                        millisecond: '%A, %b %e, %H:%M:%S.%L',
                        minute: '%A, %b %e, %l:%M %P',
                        month: '%B %Y',
                        second: '%A, %b %e, %H:%M:%S',
                        week: 'Week from %A, %b %e, %Y',
                        year: '%Y'
                    }
                },
                series: seriesData,
                legend: {
                    enabled: true
                },
                exporting: {
                    enabled: true
                }
            });

            this.loading = false;
        });
    }

    public loadChart() {
        let start: moment.Moment;
        let end: moment.Moment;

        this.loading = true;

        if (this.chart) {
            this.chart.ref$.subscribe(o => o.showLoading());
        }

        switch (this.selectedTimeSpan) {
            case TimeSpan.Last24Hours: {
                start = moment().subtract(24, 'hour');
                end = moment();

                break;
            }

            case TimeSpan.Day: {
                start = moment(this.selectedDate).startOf('day');
                end = moment(this.selectedDate).endOf('day');

                break;
            }

            default: {
                return;
            }
        }

        switch (this.selectedChartType) {
            case ChartType.Weather:
                this.loadWeatherChart(start, end);
                break;

            case ChartType.Wind:
                this.loadWindChart(start, end);
                break;
        }
    }
}
