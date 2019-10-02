import { Component, OnInit } from '@angular/core';
import { Chart } from 'angular-highcharts';
import { SeriesLineOptions } from 'highcharts';
import { HttpClient } from '@angular/common/http';
import { WeatherReadingGrouped } from '../../../models/weather/weather-reading-grouped';
import { ActivatedRoute, ParamMap } from '@angular/router';
import * as moment from 'moment';

import * as Highcharts from 'highcharts';
import HC_exporting from 'highcharts/modules/exporting';
HC_exporting(Highcharts);

enum TimeSpan {
    Last24Hours,
    Day,
    Custom
}

@Component({
    selector: 'app-weather-charts',
    templateUrl: './weather-charts.component.html',
    styleUrls: ['./weather-charts.component.scss']
})
export class WeatherChartsComponent implements OnInit {

    public chart: Chart;

    private loading = true;

    public timeSpanItems: { [value: number]: string } = {};
    public selectedTimeSpan: TimeSpan = TimeSpan.Last24Hours;
    public selectedDate: Date = moment().startOf('day').toDate();
    public timeSpans: typeof TimeSpan = TimeSpan;

    constructor(private route: ActivatedRoute, private httpClient: HttpClient) { }

    ngOnInit() {
        this.timeSpanItems[TimeSpan.Last24Hours] = 'Last 24 hours';
        this.timeSpanItems[TimeSpan.Day] = 'Day';

        this.route.params.subscribe(params => {
            this.loading = true;

            this.loadChart();
        });
    }

    public handleDateArrowClick(value: number) {
        this.selectedDate = moment(this.selectedDate).add(value, 'day').toDate();

        this.loadChart();
    }

    public isSelectedDateToday(): boolean {
        const isToday = moment(this.selectedDate).startOf('day').isSame(moment().startOf('day'));

        return isToday;
    }

    public resetToToday() {
        this.selectedDate = moment().startOf('day').toDate();

        this.loadChart();
    }

    public getSelectedDateDisplayString(): string {
        return moment(this.selectedDate).format('LL');
    }

    private loadChart() {
        let start: Date;
        let end: Date;

        switch (this.selectedTimeSpan) {
            case TimeSpan.Last24Hours: {
                start = moment().subtract(24, 'h').toDate();
                end = moment().toDate();

                break;
            }

            case TimeSpan.Day: {
                start = moment(this.selectedDate).startOf('d').toDate();
                end = moment(this.selectedDate).endOf('d').toDate();

                break;
            }

            default: {
                return;
            }
        }

        const startString = moment(start).toISOString();
        const endString = moment(end).toISOString();

        const request = this.httpClient.get<WeatherReadingGrouped[]>(`http://172.23.10.3/api/weather/readings/history-grouped?start=${startString}&end=${endString}&bucketMinutes=5`);

        request.subscribe(data => {
            const seriesData: Array<SeriesLineOptions> = [];

            seriesData.push({ name: 'Temperature', data: [], yAxis: 0, tooltip: { valueSuffix: '°F' } } as SeriesLineOptions);
            seriesData.push({ name: 'Pressure', data: [], yAxis: 1, tooltip: { valueSuffix: '"' } } as SeriesLineOptions);
            seriesData.push({ name: 'Humidity', data: [], yAxis: 2, tooltip: { valueSuffix: '%' } } as SeriesLineOptions);
            seriesData.push({ name: 'Light', data: [], yAxis: 2, tooltip: { valueSuffix: '%' } } as SeriesLineOptions);

            data.forEach(dataElement => {
                const date = Date.parse(dataElement.bucket);
                seriesData[0].data.push([date, dataElement.averagePressureTemperature]);
                seriesData[1].data.push([date, dataElement.averagePressure / 33.864]);
                seriesData[2].data.push([date, dataElement.averageHumidity]);
                seriesData[3].data.push([date, dataElement.averageLightLevel]);
            });

            this.chart = new Chart({
                chart: {
                    type: 'line'
                },
                title: {
                    text: this.timeSpanItems[TimeSpan.Last24Hours]
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
                            text: ''
                        }
                    },
                    {
                        labels: {
                            format: '{value:.2f}"'
                        },
                        title: {
                            text: ''
                        },
                        opposite: true
                    },
                    {
                        visible: false,
                        min: 0,
                        max: 100
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
}
