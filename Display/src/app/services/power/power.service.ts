import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import { PowerStatus } from 'src/app/models/power/power-status';

@Injectable({
    providedIn: 'root'
})
export class PowerService {
    private connection: HubConnection;
    private latestStatus: BehaviorSubject<PowerStatus | null> = new BehaviorSubject<PowerStatus | null>(null);

    constructor() {
        this.connection = new HubConnectionBuilder()
            .withUrl('/api/hub/power')
            .build();

        this.connection.on('LatestSample', (message: string) => {
            this.latestStatus.next(JSON.parse(message));
        });

        this.connection.start();
    }

    getLatestStatus(): Observable<PowerStatus | null> {
        return this.latestStatus.asObservable();
    }
}
