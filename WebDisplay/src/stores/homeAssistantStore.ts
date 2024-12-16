import { defineStore } from 'pinia';
import { WebSocketStream } from 'websocketstream-polyfill';
import { MessageType, AuthMessage, IncomingMessage, SubscribeEntitiesMessage, EventMessage } from '@/models/home-assistant/home-assistant';

import Environment from '@/environment';

export const useHomeAssistantStore = defineStore('home-assistant', {
    state: () => {
        return {
            garageState: null as string | null,
            houseAlarmState: null as string | null,
            _wss: null as WebSocketStream | null
        };
    },
    actions: {
        async start() {
            if (!Environment.getHomeAssistantUrl() || !Environment.getHomeAssistantToken()) {
                return;
            }

            const garageDevice = Environment.getGarageDevice();
            const alarmDevice = Environment.getAlarmDevice();

            this._wss = new WebSocketStream(Environment.getHomeAssistantUrl());

            const { readable, writable } = await this._wss.opened;

            const reader = readable.getReader();
            const writer = writable.getWriter();

            while (true) {
                const { value, done } = await reader.read();

                const message = JSON.parse(value as string) as IncomingMessage;

                console.info(message);

                switch (message.type) {
                    case MessageType.auth_required:
                        const authMessage = new AuthMessage(Environment.getHomeAssistantToken());
                        writer.write(JSON.stringify(authMessage));
                        break;
                    case MessageType.auth_ok:
                        const subscribeEntitiesMessage = new SubscribeEntitiesMessage([
                            garageDevice,
                            alarmDevice
                        ]);
                        writer.write(JSON.stringify(subscribeEntitiesMessage));
                        break;
                    case MessageType.event:
                        const eventMessage = message as EventMessage;

                        if (!eventMessage?.event?.a) {
                            break;
                        }

                        const garageEntity = eventMessage.event.a[garageDevice];

                        if (garageEntity) {
                            this.$patch({ garageState: garageEntity.s });
                        }

                        const houseAlarmEntity = eventMessage.event.a[alarmDevice];

                        if (houseAlarmEntity) {
                            this.$patch({ houseAlarmState: houseAlarmEntity.s });
                        }

                        break;
                    case MessageType.result:
                        // Handle result type
                        break;
                    default:
                        // Handle unknown message type
                        break;
                }

                if (done) {
                    break;
                }
            }
        },
        async stop() {
            if (!this._wss) {
                return;
            }

            this._wss.close();
            this._wss = null;
        }
    }
});
