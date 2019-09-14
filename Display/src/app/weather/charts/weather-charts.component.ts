import { Component, OnInit } from '@angular/core';
import { Chart } from 'angular-highcharts';
import { SeriesLineOptions } from 'highcharts';
import { HttpClient } from '@angular/common/http';
import { WeatherValue } from 'src/app/weather/service/weather-value';
import { ActivatedRoute, ParamMap } from '@angular/router';

@Component({
    selector: 'app-weather-charts',
    templateUrl: './weather-charts.component.html',
    styleUrls: ['./weather-charts.component.scss']
})
export class WeatherChartsComponent implements OnInit {

    private loading = true;

    constructor(private route: ActivatedRoute, private httpClient: HttpClient) { }

    ngOnInit() {
        this.route.params.subscribe(params => {
            this.loading = true;

            const chartType = params.type;

            switch (chartType) {
                case 'temperature':
                    this.loadChart('PressureTemperature');
                    return;

                case 'humidity':
                    this.loadChart('Humidity');
                    return;

                case 'pressure':
                    this.loadChart('Pressure');
                    return;
            }
        });
    }

    private loadChart(chartType: string) {
        const request = this.httpClient.get<WeatherValue[]>(`http://172.23.10.3/api/weather/readings/value-history?weatherValueType=${chartType}&start=2019-09-11T00:00:00-04:00&end=2019-09-12T00:00:00-04:00&bucketMinutes=5`);

        request.subscribe(data => {
            const array = [];

            data.forEach(dataElement => array.push([Date.parse(dataElement.bucket), dataElement.averageValue / 100]));

            const chart = new Chart({
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
