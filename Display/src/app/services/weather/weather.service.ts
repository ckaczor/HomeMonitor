import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject, firstValueFrom } from 'rxjs';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { WeatherUpdate } from 'src/app/models/weather/weather-update';
import { WeatherValue } from 'src/app/models/weather/weather-value';
import { HttpClient } from '@angular/common/http';
import { WeatherValueType } from 'src/app/models/weather/weather-value-type';
import { WeatherAggregates } from 'src/app/models/weather/weather-aggregates';

import moment from 'moment';

@Injectable({
    providedIn: 'root'
})
export class WeatherService {
    private connection: HubConnection;
    private latestReading: BehaviorSubject<WeatherUpdate | null> = new BehaviorSubject<WeatherUpdate | null>(null);

    constructor(private httpClient: HttpClient) {
        this.connection = new HubConnectionBuilder()
            .withUrl('/api/hub/weather')
            .build();

        this.connection.on('LatestReading', (message: string) => {
            this.latestReading.next(JSON.parse(message));
        });

        this.connection.start();
    }

    getLatestReading(): Observable<WeatherUpdate | null> {
        return this.latestReading.asObservable();
    }

    async getReadingValueHistory(valueType: WeatherValueType, start: moment.Moment, end: moment.Moment): Promise<WeatherValue[] | undefined> {
        const startString = start.toISOString();
        const endString = end.toISOString();

        const data = await firstValueFrom(this.httpClient.get<WeatherValue[]>(`/api/weather/readings/value-history?weatherValueType=${valueType}&start=${startString}&end=${endString}`));

        return data;
    }

    async getReadingAggregate(start: moment.Moment, end: moment.Moment): Promise<WeatherAggregates | undefined> {
        const startString = start.toISOString();
        const endString = end.toISOString();

        const data = await firstValueFrom(this.httpClient.get<WeatherAggregates>(`/api/weather/readings/aggregate?start=${startString}&end=${endString}`));

        return data;
    }
}
