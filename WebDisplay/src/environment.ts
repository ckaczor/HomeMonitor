import config from './config.json';

export default class Environment {
    public static getUrlPrefix(): string {
        return config.API_PREFIX;
    }
}
