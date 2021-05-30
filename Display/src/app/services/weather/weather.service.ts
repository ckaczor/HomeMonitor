import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { WeatherUpdate } from 'src/app/models/weather/weather-update';
import { WeatherValue } from 'src/app/models/weather/weather-value';
import { HttpClient } from '@angular/common/http';
import { WeatherValueType } from 'src/app/models/weather/weather-value-type';
import { WeatherAggregates } from 'src/app/models/weather/weather-aggregates';

import * as moment from 'moment';

@Injectable({
    providedIn: 'root'
})
export class WeatherService {
    private connection: HubConnection;
    private latestReading: BehaviorSubject<WeatherUpdate> = new BehaviorSubject<WeatherUpdate>(null);

    constructor(private httpClient: HttpClient) {
        this.connection = new HubConnectionBuilder()
            .withUrl('/api/hub/weather')
            .build();

        this.connection.on('LatestReading', (message: string) => {
            this.latestReading.next(JSON.parse(message));
        });

        this.connection.start();
    }

    getLatestReading(): Observable<WeatherUpdate> {
        return this.latestReading.asObservable();
    }

    async getReadingValueHistory(valueType: WeatherValueType, start: moment.Moment, end: moment.Moment): Promise<WeatherValue[]> {
        const startString = start.toISOString();
        const endString = end.toISOString();

        const data = await this.httpClient.get<WeatherValue[]>(`/api/weather/readings/value-history?weatherValueType=${valueType}&start=${startString}&end=${endString}`).toPromise();

        return data;
    }

    async getReadingAggregate(start: moment.Moment, end: moment.Moment): Promise<WeatherAggregates> {
        const startString = start.toISOString();
        const endString = end.toISOString();

        const data = await this.httpClient.get<WeatherAggregates>(`/api/weather/readings/aggregate?start=${startString}&end=${endString}`).toPromise();

        return data;
    }
}
