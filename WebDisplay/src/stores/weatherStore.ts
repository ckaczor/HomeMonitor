import { defineStore } from 'pinia';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import axios from 'axios';
import Environment from '@/environment';
import WeatherUpdate from '@/models/weather/weather-update';
import WeatherRecent from '@/models/weather/weather-recent';
import WeatherValueType from '@/models/weather/weather-value-type';
import WeatherValueGrouped from '@/models/weather/weather-value-grouped';
import WeatherHistoryGrouped from '@/models/weather/weather-history-grouped';
import WindHistoryGrouped from '@/models/weather/wind-history-grouped';
import WeatherAggregates from '@/models/weather/weather-aggregates';

export const useWeatherStore = defineStore('weather', {
    state: () => {
        return {
            current: null as WeatherUpdate | null,
            _connection: null as HubConnection | null
        };
    },
    actions: {
        async start() {
            if (this._connection) {
                return;
            }

            this._connection = new HubConnectionBuilder()
                .withUrl(Environment.getUrlPrefix() + '/api/hub/weather', {
                    withCredentials: false
                })
                .build();

            await this._connection.start();

            this._connection.on('LatestReading', (message: string) => {
                const json: WeatherUpdate = JSON.parse(message);

                this.$patch({ current: json });
            });
        },
        async stop() {
            if (!this._connection) {
                return;
            }

            await this._connection.stop();

            this._connection = null;
        },
        async getLatest(): Promise<WeatherRecent> {
            const response = await axios.get<WeatherRecent>(Environment.getUrlPrefix() + `/api/weather/readings/recent`);

            return response.data;
        },
        async getReadingValueHistoryGrouped(valueType: WeatherValueType, start: Date, end: Date, bucketMinutes: number): Promise<WeatherValueGrouped[]> {
            const startString = start.toISOString();
            const endString = end.toISOString();

            const response = await axios.get<WeatherValueGrouped[]>(
                Environment.getUrlPrefix() +
                    `/api/weather/readings/value-history-grouped?weatherValueType=${valueType}&start=${startString}&end=${endString}&bucketMinutes=${bucketMinutes}`
            );

            return response.data;
        },
        async getReadingAggregate(start: Date, end: Date): Promise<WeatherAggregates | undefined> {
            const startString = start.toISOString();
            const endString = end.toISOString();

            const response = await axios.get<WeatherAggregates>(
                Environment.getUrlPrefix() + `/api/weather/readings/aggregate?start=${startString}&end=${endString}`
            );

            return response.data;
        },
        async getReadingHistoryGrouped(start: Date, end: Date, bucketMinutes: number): Promise<WeatherHistoryGrouped[]> {
            const startString = start.toISOString();
            const endString = end.toISOString();

            const response = await axios.get<WeatherHistoryGrouped[]>(
                Environment.getUrlPrefix() + `/api/weather/readings/history-grouped?start=${startString}&end=${endString}&bucketMinutes=${bucketMinutes}`
            );

            return response.data;
        },
        async getWindHistoryGrouped(start: Date, end: Date, bucketMinutes: number): Promise<WindHistoryGrouped[]> {
            const startString = start.toISOString();
            const endString = end.toISOString();

            const response = await axios.get<WindHistoryGrouped[]>(
                Environment.getUrlPrefix() + `/api/weather/readings/wind-history-grouped?start=${startString}&end=${endString}&bucketMinutes=${bucketMinutes}`
            );

            return response.data;
        }
    }
});
