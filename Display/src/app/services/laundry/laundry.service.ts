import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { HubConnectionBuilder, HubConnection } from '@aspnet/signalr';
import { LaundryStatus } from '../../models/laundry/laundry-status';
import { HttpClient } from '@angular/common/http';

class DeviceMessage {
    name: string;
    status: boolean;
}

@Injectable({
    providedIn: 'root'
})
export class LaundryService {
    private connection: HubConnection;
    private latestStatus: BehaviorSubject<LaundryStatus> = new BehaviorSubject<LaundryStatus>(new LaundryStatus());

    constructor(private httpClient: HttpClient) {
        this.loadLatestStatus().then();

        this.connection = new HubConnectionBuilder()
            .withUrl('/api/hub/device-status')
            .build();

        this.connection.on('LatestStatus', (message: string) => {
            const deviceMessage = JSON.parse(message) as DeviceMessage;

            const newStatus = new LaundryStatus();

            newStatus.dryer = this.latestStatus.getValue().dryer;
            newStatus.washer = this.latestStatus.getValue().washer;

            if (deviceMessage.name === 'washer') {
                newStatus.washer = deviceMessage.status;
            } else if (deviceMessage.name === 'dryer') {
                newStatus.dryer = deviceMessage.status;
            }

            this.latestStatus.next(newStatus);
        });

        this.connection.start();
    }

    getLatestStatus(): Observable<LaundryStatus> {
        return this.latestStatus.asObservable();
    }

    private async loadLatestStatus() {
        const data = await this.httpClient.get<DeviceMessage[]>(`/api/device-status/status/recent`).toPromise();

        const newStatus = new LaundryStatus();

        data.forEach(deviceMessage => {
            if (deviceMessage.name === 'washer') {
                newStatus.washer = deviceMessage.status;
            } else if (deviceMessage.name === 'dryer') {
                newStatus.dryer = deviceMessage.status;
            }
        });

        this.latestStatus.next(newStatus);
    }
}
