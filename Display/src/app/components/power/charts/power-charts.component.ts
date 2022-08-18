import { Component, OnInit } from '@angular/core';
import { TimeSpan } from 'src/app/models/time-span';
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

@Component({
    selector: 'app-power-charts',
    templateUrl: './power-charts.component.html',
    styleUrls: ['./power-charts.component.scss']
})
export class PowerChartsComponent implements OnInit {

    public chart: Chart | undefined;
    public loading = true;

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

    public getSelectedDateDisplayString(): string {
        return moment(this.date).format('LL');
    }

    private async loadPowerChart(start: moment.Moment, end: moment.Moment) {
        const startString = start.toISOString();
        const endString = end.toISOString();

        forkJoin([
            this.httpClient.get<PowerStatusGrouped[]>(`/api/power/status/history-grouped?start=${startString}&end=${endString}&bucketMinutes=${this.timeInterval}`),
            this.httpClient.get<WeatherValueGrouped[]>(`/api/weather/readings/value-history-grouped?weatherValueType=LightLevel&start=${startString}&end=${endString}&bucketMinutes=${this.timeInterval}`)
        ]).subscribe(data => {
            const seriesData: Array<SeriesLineOptions> = [];

            seriesData.push({ type: 'line', name: 'Generation', data: [], yAxis: 0, marker: { enabled: false }, tooltip: { valueSuffix: ' W' } } as SeriesLineOptions);
            seriesData.push({ type: 'line', name: 'Consumption', data: [], yAxis: 0, marker: { enabled: false }, tooltip: { valueSuffix: ' W' } } as SeriesLineOptions);
            seriesData.push({ type: 'line', name: 'Light', data: [], yAxis: 1, marker: { enabled: false }, tooltip: { valueSuffix: ' lx' } } as SeriesLineOptions);

            data[0].forEach(dataElement => {
                const date = Date.parse(dataElement.bucket!);
                seriesData[0].data!.push([date, dataElement.averageGeneration! < 0 ? 0 : dataElement.averageGeneration]);
                seriesData[1].data!.push([date, dataElement.averageConsumption! < 0 ? 0 : dataElement.averageConsumption]);
            });

            data[1].forEach(dataElement => {
                const date = Date.parse(dataElement.bucket!);
                seriesData[2].data!.push([date, dataElement.averageValue]);
            });

            this.chart = new Chart({
                chart: {
                    type: 'line',
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
                            format: '{value} lx'
                        },
                        title: {
                            text: 'Light'
                        },
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

        this.loadPowerChart(start, end);
    }
}
