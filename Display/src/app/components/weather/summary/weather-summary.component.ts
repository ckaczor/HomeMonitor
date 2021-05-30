import { Component, OnInit } from '@angular/core';
import { TimeSpan } from 'src/app/models/time-span';
import { WeatherService } from 'src/app/services/weather/weather.service';
import { WeatherAggregates } from 'src/app/models/weather/weather-aggregates';

import * as moment from 'moment';

@Component({
    selector: 'app-weather-summary',
    templateUrl: './weather-summary.component.html',
    styleUrls: ['./weather-summary.component.scss']
})
export class WeatherSummaryComponent implements OnInit {

    public loading = true;
    public weatherAggregates: WeatherAggregates = null;

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

    constructor(private weatherService: WeatherService) { }

    ngOnInit(): void {
        this.load();
    }

    public async load() {
        let start: moment.Moment;
        let end: moment.Moment;

        this.loading = true;

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

        this.weatherAggregates = await this.weatherService.getReadingAggregate(start, end);

        this.loading = false;
    }
}
