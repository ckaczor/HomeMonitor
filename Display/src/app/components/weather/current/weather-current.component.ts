import { Component, OnInit } from '@angular/core';
import { WeatherReading } from '../../../services/weather/weather-reading';
import { WeatherService } from '../../../services/weather/weather.service';

@Component({
    selector: 'app-weather-current',
    templateUrl: './weather-current.component.html',
    styleUrls: ['./weather-current.component.scss']
})
export class WeatherCurrentComponent implements OnInit {

    public latestReading: WeatherReading;

    constructor(private weatherService: WeatherService) { }

    ngOnInit() {
        this.weatherService.getLatestReading().subscribe(r => this.latestReading = r);
    }
}
