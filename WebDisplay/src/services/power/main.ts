import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import PowerStatus from './power-status';
import Environment from '../../environment';

export default class PowerService {
    private connection: HubConnection;
    private started: boolean = false;

    constructor() {
        this.connection = new HubConnectionBuilder()
            .withUrl(Environment.getUrlPrefix() + '/api/hub/power', {
                withCredentials: false,
            })
            .build();
    }

    async start(callback: (powerStatus: PowerStatus) => void) {
        if (this.started) {
            return;
        }

        this.started = true;

        await this.connection.start();

        this.connection!.on('LatestSample', (message: string) => {
            callback(JSON.parse(message));
        });
    }
}
