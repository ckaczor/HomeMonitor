import { Component, OnInit } from '@angular/core';
import { Chart } from 'angular-highcharts';
import { SeriesLineOptions } from 'highcharts';
import { HttpClient } from '@angular/common/http';
import { WeatherValue } from '../../../models/weather/weather-value';
import { ActivatedRoute, ParamMap } from '@angular/router';
import * as moment from 'moment';

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
    private chartType: string;

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

            const chartType = params.type;

            switch (chartType) {
                case 'temperature':
                    this.chartType = 'PressureTemperature';
                    break;

                case 'humidity':
                    this.chartType = 'Humidity';
                    break;

                case 'pressure':
                    this.chartType = 'Pressure';
                    break;
            }

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

        const request = this.httpClient.get<WeatherValue[]>(`http://172.23.10.3/api/weather/readings/value-history?weatherValueType=${this.chartType}&start=${startString}&end=${endString}&bucketMinutes=5`);

        request.subscribe(data => {
            const array = [];

            let divisor = 1;

            if (this.chartType === 'Pressure') {
                divisor = 100;
            }

            data.forEach(dataElement => array.push([Date.parse(dataElement.bucket), dataElement.averageValue / divisor]));

            this.chart = new Chart({
                chart: {
                    type: 'line'
                },
                title: {
                    text: 'Linechart'
                },
                credits: {
                    enabled: true
                },
                xAxis: {
                    type: 'datetime',
                },
                yAxis: {
                    labels: {
                        format: '{value:.2f}'
                    }
                },
                time: {
                    useUTC: false
                },
                tooltip: {
                    valueDecimals: 2
                },
                series: [
                    {
                        name: 'Line 1',
                        data: array
                    } as SeriesLineOptions
                ]
            });

            this.loading = false;
        });
    }
}
