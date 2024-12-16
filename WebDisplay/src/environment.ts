import config from './config.json';

export default class Environment {
    public static getUrlPrefix(): string {
        return config.API_PREFIX;
    }

    public static getHomeAssistantUrl(): string {
        return config.HOME_ASSISTANT_URL;
    }

    public static getHomeAssistantToken(): string {
        return config.HOME_ASSISTANT_TOKEN;
    }

    public static getGarageDevice(): string {
        return config.GARAGE_DEVICE;
    }

    public static getAlarmDevice(): string {
        return config.ALARM_DEVICE;
    }
}
