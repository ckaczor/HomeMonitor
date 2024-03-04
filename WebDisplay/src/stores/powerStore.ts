import { defineStore } from 'pinia';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import Environment from '@/environment';
import PowerStatus from '@/models/power/power-status';

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
        }
    }
});
