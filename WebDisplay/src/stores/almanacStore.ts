import { defineStore } from 'pinia';
import * as SunCalc from 'suncalc';
import WeatherRecent from '@/models/weather/weather-recent';
import { useWeatherStore } from './weatherStore';

export const useAlmanacStore = defineStore('almanac', {
    state: () => {
        return {
            sunTimes: null as SunCalc.GetTimesResult | null,
            moonTimes: null as SunCalc.GetMoonTimes | null,
            moonIllumination: null as SunCalc.GetMoonIlluminationResult | null
        };
    },
    actions: {
        async load() {
            const weatherStore = useWeatherStore();

            weatherStore.getLatest().then((weatherRecent: WeatherRecent) => {
                const date = new Date();

                this.sunTimes = SunCalc.getTimes(
                    date,
                    weatherRecent?.latitude!,
                    weatherRecent?.longitude!
                );

                this.moonTimes = SunCalc.getMoonTimes(
                    date,
                    weatherRecent?.latitude!,
                    weatherRecent?.longitude!
                );

                this.moonIllumination = SunCalc.getMoonIllumination(date);
            });
        }
    }
});
