import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { PowerStatus } from 'src/app/models/power/power-status';

@Injectable({
    providedIn: 'root'
})
export class PowerService {
    private connection: HubConnection;
    private latestStatus: BehaviorSubject<PowerStatus> = new BehaviorSubject<PowerStatus>(null);

    constructor() {
        this.connection = new HubConnectionBuilder()
            .withUrl('/api/hub/power')
            .build();

        this.connection.on('LatestSample', (message: string) => {
            this.latestStatus.next(JSON.parse(message));
        });

        this.connection.start();
    }

    getLatestStatus(): Observable<PowerStatus> {
        return this.latestStatus.asObservable();
    }
}
