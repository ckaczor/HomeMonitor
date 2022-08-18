import { Component, OnInit } from '@angular/core';
import { TimeSpan } from 'src/app/models/time-span';
import { Chart } from 'angular-highcharts';
import { SeriesLineOptions, SeriesWindbarbOptions, SeriesColumnOptions } from 'highcharts';
import { HttpClient } from '@angular/common/http';
import { WeatherReadingGrouped } from 'src/app/models/weather/weather-reading-grouped';
import { WindHistoryGrouped } from 'src/app/models/weather/wind-history-grouped';

import * as moment from 'moment';

import * as Highcharts from 'highcharts';
import HC_exporting from 'highcharts/modules/exporting';
import HC_windbarb from 'highcharts/modules/windbarb';
HC_exporting(Highcharts);
HC_windbarb(Highcharts);

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

    public chart: Chart | undefined;
    public loading = true;

    public selectedChartType: ChartType = ChartType.Weather;
    public ChartType = ChartType;

    private timeSpanValue: TimeSpan = TimeSpan.Last24Hours;
    private dateValue: moment.Moment = moment().startOf('day');

    public get timeSpan(): TimeSpan {
        return this.timeSpanValue;
    }
    public set timeSpan(value: TimeSpan) {
        if (this.timeSpanValue === value) {
            return;
        }
        this.timeSpanValue = value;
        this.load();
    }

    public get date(): moment.Moment {
        return this.dateValue;
    }
    public set date(value: moment.Moment) {
        if (this.dateValue === value) {
            return;
        }
        this.dateValue = value;
        this.load();
    }

    private timeInterval = 15;

    constructor(private httpClient: HttpClient) { }

    ngOnInit() {
        this.load();
    }

    public chartTypeChange(value: ChartType) {
        if (this.selectedChartType === value) {
            return;
        }

        this.selectedChartType = value;

        this.load();
    }

    private loadWeatherChart(start: moment.Moment, end: moment.Moment) {
        const startString = start.toISOString();
        const endString = end.toISOString();

        const request = this.httpClient.get<WeatherReadingGrouped[]>(`/api/weather/readings/history-grouped?start=${startString}&end=${endString}&bucketMinutes=${this.timeInterval}`);

        request.subscribe(data => {
            const seriesData: Array<SeriesLineOptions | SeriesColumnOptions> = [];

            seriesData.push({ type: 'line', name: 'Temperature', data: [], yAxis: 0, marker: { enabled: false }, tooltip: { valueSuffix: '°F' } } as SeriesLineOptions);
            seriesData.push({ type: 'line', name: 'Pressure', data: [], yAxis: 1, marker: { enabled: false }, tooltip: { valueSuffix: '"' } } as SeriesLineOptions);
            seriesData.push({ type: 'line', name: 'Humidity', data: [], yAxis: 2, marker: { enabled: false }, tooltip: { valueSuffix: '%' } } as SeriesLineOptions);
            seriesData.push({ type: 'line', name: 'Light', data: [], yAxis: 4, marker: { enabled: false }, tooltip: { valueSuffix: ' lx' } } as SeriesLineOptions);
            seriesData.push({ type: 'column', name: 'Rain', data: [], yAxis: 3, marker: { enabled: false }, tooltip: { valueSuffix: '"' } } as SeriesColumnOptions);

            data.forEach(dataElement => {
                const date = Date.parse(dataElement.bucket!);
                seriesData[0].data!.push([date, dataElement.averageTemperature]);
                seriesData[1].data!.push([date, dataElement.averagePressure! / 33.864 / 100]);
                seriesData[2].data!.push([date, dataElement.averageHumidity]);
                seriesData[3].data!.push([date, dataElement.averageLightLevel]);
                seriesData[4].data!.push([date, dataElement.rainTotal]);
            });

            this.chart = new Chart({
                chart: {
                    zoomType: 'x',
                    spacingTop: 20
                },
                title: {
                    text: ''
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
                        }
                    },
                    {
                        visible: true,
                        labels: {
                            format: '{value}%'
                        },
                        title: {
                            text: 'Humidity'
                        },
                        min: 0,
                        max: 100,
                        opposite: true
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
                    },
                    {
                        labels: {
                            format: '{value} lx',
                        },
                        title: {
                            text: 'Light'
                        },
                        opposite: true
                    },
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

        const request = this.httpClient.get<WindHistoryGrouped[]>(`/api/weather/readings/wind-history-grouped?start=${startString}&end=${endString}&bucketMinutes=${this.timeInterval}`);

        request.subscribe(data => {
            const seriesData: Array<SeriesLineOptions | SeriesWindbarbOptions> = [];

            seriesData.push({ type: 'line', name: 'Minimum', data: [], yAxis: 0, marker: { enabled: false }, tooltip: { valueSuffix: ' MPH' } } as SeriesLineOptions);
            seriesData.push({ type: 'line', name: 'Average', data: [], yAxis: 0, marker: { enabled: false }, tooltip: { valueSuffix: ' MPH' } } as SeriesLineOptions);
            seriesData.push({ type: 'line', name: 'Maximum', data: [], yAxis: 0, marker: { enabled: false }, tooltip: { valueSuffix: ' MPH' } } as SeriesLineOptions);
            seriesData.push({ type: 'windbarb', name: 'Direction', data: [], marker: { enabled: false }, tooltip: { valueSuffix: ' MPH' } } as SeriesWindbarbOptions);

            data.forEach(dataElement => {
                const date = Date.parse(dataElement.bucket!);
                seriesData[0].data!.push([date, dataElement.minimumSpeed]);
                seriesData[1].data!.push([date, dataElement.averageSpeed]);
                seriesData[2].data!.push([date, dataElement.maximumSpeed]);
                seriesData[3].data!.push([date, dataElement.averageSpeed, dataElement.averageDirection]);
            });

            this.chart = new Chart({
                chart: {
                    zoomType: 'x',
                    spacingTop: 20
                },
                title: {
                    text: ''
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
                            format: '{value} MPH',
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

    public load() {
        let start: moment.Moment;
        let end: moment.Moment;

        this.loading = true;

        if (this.chart) {
            this.chart.ref$.subscribe(o => o.showLoading());
        }

        switch (this.timeSpan) {
            case TimeSpan.Last24Hours: {
                start = moment().subtract(24, 'hour');
                end = moment();

                break;
            }

            case TimeSpan.Day: {
                start = moment(this.date).startOf('day');
                end = moment(this.date).endOf('day');

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
