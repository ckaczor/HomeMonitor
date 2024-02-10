import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import axios from 'axios';
import DeviceMessage from './device-message';
import LaundryStatus from './laundry-status';
import Environment from '../../environment';

export default class LaundryService {
    private connection: HubConnection;
    private started: boolean = false;
    private latestStatus: LaundryStatus = new LaundryStatus();

    constructor() {
        this.connection = new HubConnectionBuilder()
            .withUrl(Environment.getUrlPrefix() + '/api/hub/device-status', {
                withCredentials: false,
            })
            .build();
    }

    async getLatest(): Promise<LaundryStatus> {
        const response = await axios.get<DeviceMessage[]>(Environment.getUrlPrefix() + `/api/device-status/status/recent`);

        const newStatus = new LaundryStatus();

        response.data.forEach((deviceMessage) => {
            if (deviceMessage.name === 'washer') {
                newStatus.washer = deviceMessage.status;
            } else if (deviceMessage.name === 'dryer') {
                newStatus.dryer = deviceMessage.status;
            }
        });

        return newStatus;
    }

    async start(callback: (laundryStatus: LaundryStatus) => void) {
        if (this.started) {
            return;
        }

        this.started = true;

        this.latestStatus = await this.getLatest();

        await this.connection.start();

        this.connection!.on('LatestStatus', (message: string) => {
            const deviceMessage = JSON.parse(message) as DeviceMessage;

            const newStatus = new LaundryStatus();

            newStatus.dryer = this.latestStatus.dryer;
            newStatus.washer = this.latestStatus.washer;

            if (deviceMessage.name === 'washer') {
                newStatus.washer = deviceMessage.status;
            } else if (deviceMessage.name === 'dryer') {
                newStatus.dryer = deviceMessage.status;
            }

            callback(newStatus);
        });
    }
}
