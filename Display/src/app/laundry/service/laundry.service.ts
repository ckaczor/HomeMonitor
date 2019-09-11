import { Injectable } from '@angular/core';
import { Observable, BehaviorSubject } from 'rxjs';
import { Socket } from 'ngx-socket-io';
import { LaundryStatus } from './laundry-status';

@Injectable({
    providedIn: 'root'
})
export class LaundryService {
    private latestStatus: BehaviorSubject<LaundryStatus> = new BehaviorSubject<LaundryStatus>(new LaundryStatus());

    constructor(private socket: Socket) {
        this.socket.on('status', (statusString: string) => {
            const newStatus: LaundryStatus = JSON.parse(statusString);

            if (newStatus.washer !== undefined) {
               this.latestStatus.value.washer = newStatus.washer;
            }

            if (newStatus.dryer !== undefined) {
               this.latestStatus.value.dryer = newStatus.dryer;
            }
        });

        this.socket.emit('getStatus');
    }

    getLatestStatus(): Observable<LaundryStatus> {
        return this.latestStatus.asObservable();
    }
}
