import { HubConnectionBuilder, HubConnection } from '@microsoft/signalr';
import WeatherUpdate from './weather-update';
import WeatherRecent from './weather-recent';
import Environment from '../../environment';
import axios from 'axios';

export default class WeatherService {
    private connection: HubConnection;
    private started: boolean = false;

    constructor() {
        this.connection = new HubConnectionBuilder()
            .withUrl(Environment.getUrlPrefix() + '/api/hub/weather', {
                withCredentials: false,
            })
            .build();
    }

    async getLatest(): Promise<WeatherRecent> {
        const response = await axios.get<WeatherRecent>(Environment.getUrlPrefix() + `/api/weather/readings/recent`);

        return response.data;
    }

    async start(callback: (weatherUpdate: WeatherUpdate) => void) {
        if (this.started) {
            return;
        }

        this.started = true;

        await this.connection.start();

        this.connection!.on('LatestReading', (message: string) => {
            callback(JSON.parse(message));
        });
    }
}
