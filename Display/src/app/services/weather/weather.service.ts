import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { WeatherReading } from '../../models/weather/weather-reading';

@Injectable({
    providedIn: 'root'
})
export class WeatherService {
    private connection: HubConnection;
    private latestReading: BehaviorSubject<WeatherReading> = new BehaviorSubject<WeatherReading>(null);

    constructor() {
        this.connection = new HubConnectionBuilder()
            .withUrl('/api/hub/weather')
            .build();

        this.connection.on('LatestReading', (message: string) => {
            this.latestReading.next(JSON.parse(message));
        });

        this.connection.start();
    }

    getLatestReading(): Observable<WeatherReading> {
        return this.latestReading.asObservable();
    }
}
