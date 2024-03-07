import { defineStore } from 'pinia';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import axios from 'axios';
import Environment from '@/environment';
import PowerStatus from '@/models/power/power-status';
import PowerHistoryGrouped from '@/models/power/power-history-grouped';

export const usePowerStore = defineStore('power', {
    state: () => {
        return {
            current: null as PowerStatus | null,
            _connection: null as HubConnection | null
        };
    },
    actions: {
        async start() {
            if (this._connection) {
                return;
            }

            this._connection = new HubConnectionBuilder()
                .withUrl(Environment.getUrlPrefix() + '/api/hub/power', {
                    withCredentials: false
                })
                .build();

            await this._connection.start();

            this._connection.on('LatestSample', (message: string) => {
                this.$patch({ current: JSON.parse(message) });
            });
        },
        async stop() {
            if (!this._connection) {
                return;
            }

            await this._connection.stop();

            this._connection = null;
        },
        async getReadingHistoryGrouped(start: Date, end: Date, bucketMinutes: number): Promise<PowerHistoryGrouped[]> {
            const startString = start.toISOString();
            const endString = end.toISOString();

            const response = await axios.get<PowerHistoryGrouped[]>(
                Environment.getUrlPrefix() + `/api/power/status/history-grouped?start=${startString}&end=${endString}&bucketMinutes=${bucketMinutes}`
            );

            return response.data;
        }
    }
});
