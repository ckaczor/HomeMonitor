import { defineStore } from 'pinia';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import axios from 'axios';
import Environment from '@/environment';
import LaundryStatus from '@/models/laundry/laundry-status';
import DeviceMessage from '@/models/laundry/device-message';

export const useLaundryStore = defineStore('laundry', {
    state: () => {
        return {
            current: null as LaundryStatus | null,
            _connection: null as HubConnection | null
        };
    },
    actions: {
        async start() {
            if (this._connection) {
                return;
            }

            this._connection = new HubConnectionBuilder()
                .withUrl(
                    Environment.getUrlPrefix() + '/api/hub/device-status',
                    {
                        withCredentials: false
                    }
                )
                .build();

            await this._connection.start();

            this._connection.on('LatestStatus', (message: string) => {
                const deviceMessage = JSON.parse(message) as DeviceMessage;

                const newStatus = new LaundryStatus();

                newStatus.dryer = this.current?.dryer;
                newStatus.washer = this.current?.washer;

                if (deviceMessage.name === 'washer') {
                    newStatus.washer = deviceMessage.status;
                } else if (deviceMessage.name === 'dryer') {
                    newStatus.dryer = deviceMessage.status;
                }

                this.$patch({ current: newStatus });
            });

            this._connection.send('RequestLatestStatus');
        },
        async stop() {
            if (!this._connection) {
                return;
            }

            await this._connection.stop();

            this._connection = null;
        },
        async getLatest(): Promise<LaundryStatus> {
            const response = await axios.get<DeviceMessage[]>(
                Environment.getUrlPrefix() + `/api/device-status/status/recent`
            );

            const newStatus = new LaundryStatus();

            response.data.forEach((deviceMessage: DeviceMessage) => {
                if (deviceMessage.name === 'washer') {
                    newStatus.washer = deviceMessage.status;
                } else if (deviceMessage.name === 'dryer') {
                    newStatus.dryer = deviceMessage.status;
                }
            });

            return newStatus;
        }
    }
});
