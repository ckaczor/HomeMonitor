import { defineStore } from 'pinia';
import { createConnection, subscribeEntities, createLongLivedTokenAuth, Connection, callService } from 'home-assistant-js-websocket';
import Environment from '@/environment';

export const useHomeAssistantStore = defineStore('home-assistant', {
    state: () => {
        return {
            garageState: null as string | null,
            houseAlarmState: null as string | null,
            _connection: null as Connection | null
        };
    },
    actions: {
        async start() {
            if (!Environment.getHomeAssistantUrl() || !Environment.getHomeAssistantToken()) {
                return;
            }

            const garageDevice = Environment.getGarageDevice();
            const alarmDevice = Environment.getAlarmDevice();

            const auth = createLongLivedTokenAuth(Environment.getHomeAssistantUrl(), Environment.getHomeAssistantToken());

            this._connection = await createConnection({ auth });

            subscribeEntities(this._connection as Connection, entities => {
                const garageEntity = entities[garageDevice];

                if (garageEntity) {
                    this.$patch({ garageState: garageEntity.state });
                }

                const houseAlarmEntity = entities[alarmDevice];

                if (houseAlarmEntity) {
                    this.$patch({ houseAlarmState: houseAlarmEntity.state });
                }
            });

            setInterval(async () => await this._connection?.ping(), 5000);
        },
        async stop() {
            this._connection?.close();
            this._connection = null;
        },
        async toggleGarage() {
            const garageDevice = Environment.getGarageDevice();

            callService(this._connection as Connection, 'cover', this.garageState === 'closed' ? 'open_cover' : 'close_cover', { entity_id: garageDevice });
        }
    }
});
