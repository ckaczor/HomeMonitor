export default class Environment {
    public static getUrlPrefix(): string {
        return '%API_PREFIX%';
    }

    public static getHomeAssistantUrl(): string {
        return '%HOME_ASSISTANT_URL%';
    }

    public static getHomeAssistantToken(): string {
        return '%HOME_ASSISTANT_TOKEN%';
    }

    public static getGarageDevice(): string {
        return '%GARAGE_DEVICE%';
    }

    public static getAlarmDevice(): string {
        return '%ALARM_DEVICE%';
    }
}
