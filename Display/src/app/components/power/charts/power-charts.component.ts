import { Component, OnInit } from '@angular/core';
import { Chart } from 'angular-highcharts';
import { SeriesLineOptions } from 'highcharts';
import { HttpClient } from '@angular/common/http';
import { forkJoin } from 'rxjs';
import { WeatherValueGrouped } from 'src/app/models/weather/weather-value-grouped';
import { PowerStatusGrouped } from 'src/app/models/power/power-status-grouped';

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
    selector: 'app-power-charts',
    templateUrl: './power-charts.component.html',
    styleUrls: ['./power-charts.component.scss']
})
export class PowerChartsComponent implements OnInit {

    public chart: Chart;
    public loading = true;
    public timeSpanItems: { [value: number]: string } = {};
    public timeSpans: typeof TimeSpan = TimeSpan;
    public maxDate: moment.Moment = moment().endOf('day');

    private selectedTimeSpanValue: TimeSpan = TimeSpan.Last24Hours;
    private selectedDateValue: moment.Moment = moment().startOf('day');

    private timeInterval = 15;

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

    private async loadPowerChart(start: moment.Moment, end: moment.Moment) {
        const startString = start.toISOString();
        const endString = end.toISOString();

        forkJoin([
            this.httpClient.get<PowerStatusGrouped[]>(`/api/power/status/history-grouped?start=${startString}&end=${endString}&bucketMinutes=${this.timeInterval}`),
            this.httpClient.get<WeatherValueGrouped[]>(`/api/weather/readings/value-history-grouped?weatherValueType=LightLevel&start=${startString}&end=${endString}&bucketMinutes=${this.timeInterval}`)
        ]).subscribe(data => {
            const seriesData: Array<SeriesLineOptions> = [];

            seriesData.push({ name: 'Generation', data: [], yAxis: 0, marker: { enabled: false }, tooltip: { valueSuffix: ' W' } } as SeriesLineOptions);
            seriesData.push({ name: 'Consumption', data: [], yAxis: 0, marker: { enabled: false }, tooltip: { valueSuffix: ' W' } } as SeriesLineOptions);
            seriesData.push({ name: 'Light', data: [], yAxis: 1, marker: { enabled: false }, tooltip: { valueSuffix: '%' } } as SeriesLineOptions);

            data[0].forEach(dataElement => {
                const date = Date.parse(dataElement.bucket);
                seriesData[0].data.push([date, dataElement.averageGeneration < 0 ? 0 : dataElement.averageGeneration]);
                seriesData[1].data.push([date, dataElement.averageConsumption < 0 ? 0 : dataElement.averageConsumption]);
            });

            data[1].forEach(dataElement => {
                const date = Date.parse(dataElement.bucket);
                seriesData[2].data.push([date, dataElement.averageValue]);
            });

            const title = this.selectedTimeSpan === TimeSpan.Last24Hours ? this.timeSpanItems[TimeSpan.Last24Hours] : this.getSelectedDateDisplayString();

            this.chart = new Chart({
                chart: {
                    type: 'line',
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
                        minute: '%l:%M %P',
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
                            format: '{value} W',
                        },
                        title: {
                            text: 'Power'
                        }
                    },
                    {
                        visible: true,
                        labels: {
                            format: '{value:.2f}%'
                        },
                        title: {
                            text: 'Light'
                        },
                        min: 0,
                        max: 100,
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

        this.loadPowerChart(start, end);
    }
}
