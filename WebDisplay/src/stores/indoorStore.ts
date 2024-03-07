import { defineStore } from 'pinia';
import axios from 'axios';
import Environment from '@/environment';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { LatestReadings } from '@/models/environment.ts/latestReadings';
import { ReadingsGrouped } from '@/models/environment.ts/readingsGrouped';

export function createIndoorStore(name: string) {
    return defineStore(`indoor-${name}`, {
        state: () => {
            return {
                current: null as LatestReadings | null,
                _connection: null as HubConnection | null
            };
        },
        actions: {
            async start() {
                if (this._connection) {
                    return;
                }

                this._connection = new HubConnectionBuilder()
                    .withUrl(Environment.getUrlPrefix() + '/api/hub/environment', {
                        withCredentials: false
                    })
                    .build();

                await this._connection.start();

                this._connection.on('Latest', (message: string) => {
                    const latestReadings = JSON.parse(message) as LatestReadings;

                    if (latestReadings.name === name) {
                        this.$patch({ current: latestReadings });
                    }
                });

                this._connection.send('RequestLatest');
            },
            async stop() {
                if (!this._connection) {
                    return;
                }

                await this._connection.stop();

                this._connection = null;
            },
            async getReadingValueHistoryGrouped(start: Date, end: Date, bucketMinutes: number): Promise<ReadingsGrouped[]> {
                const startString = start.toISOString();
                const endString = end.toISOString();

                const response = await axios.get<ReadingsGrouped[]>(
                    Environment.getUrlPrefix() +
                        `/api/environment/readings/history-grouped?start=${startString}&end=${endString}&bucketMinutes=${bucketMinutes}`
                );

                return response.data;
            }
        }
    })();
}
