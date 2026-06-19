import { defineStore } from 'pinia';
import { createConnection, subscribeEntities, createLongLivedTokenAuth, Connection, callService, HassEntities, HassEntity } from 'home-assistant-js-websocket';
import Environment from '@/environment';
import { HomeAssistantRegistryEntity } from '@/models/homeAssistant/registryEntity';

export const useHomeAssistantStore = defineStore('home-assistant', {
    state: () => {
        return {
            garageState: null as string | null,
            houseAlarmState: null as string | null,
            alarmEntities: {} as Record<string, HassEntity>,
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
            const alarmIntegration = Environment.getAlarmIntegration();

            const auth = createLongLivedTokenAuth(Environment.getHomeAssistantUrl(), Environment.getHomeAssistantToken());

            this._connection = await createConnection({ auth });

            const homeAssistantRegistryEntries: HomeAssistantRegistryEntity[] = await this._connection.sendMessagePromise({
                type: 'config/entity_registry/list'
            });

            const alarmEntityList = homeAssistantRegistryEntries.filter((entity) => entity.platform === alarmIntegration);

            const alarmEntities: Record<string, HomeAssistantRegistryEntity> = Object.fromEntries(alarmEntityList.map((entity) => [entity.entity_id, entity]));

            subscribeEntities(this._connection as Connection, (entities: HassEntities) => {
                const garageEntity = entities[garageDevice];

                if (garageEntity) {
                    this.$patch({ garageState: garageEntity.state });
                }

                const houseAlarmEntity = entities[alarmDevice];

                if (houseAlarmEntity) {
                    this.$patch({ houseAlarmState: houseAlarmEntity.state });
                }

                Object.entries(entities).forEach(([entityId, entity]) => {
                    if (alarmEntities[entityId]) {
                        this.$patch((state) => (state.alarmEntities[entityId] = entity));
                    }
                });
            });

            setInterval(async () => await this._connection?.ping(), 5000);
        },
        async stop() {
            this._connection?.close();
            this._connection = null;
        },
        async toggleGarage() {
            const garageDevice = Environment.getGarageDevice();

            let action: string | null;

            switch (this.garageState) {
                case 'closed':
                    action = 'open_cover';
                    break;
                case 'open':
                    action = 'close_cover';
                    break;
                case 'closing':
                case 'opening':
                    action = 'stop_cover';
                    break;
                default:
                    action = null;
                    break;
            }

            if (!action) {
                return;
            }

            callService(this._connection as Connection, 'cover', action, { entity_id: garageDevice });
        }
    }
});
