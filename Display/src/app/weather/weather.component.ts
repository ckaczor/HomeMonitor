import { Component, OnInit } from '@angular/core';
import { WeatherService } from '../weather.service';
import { WeatherReading } from '../weather-reading';

@Component({
    selector: 'app-weather',
    templateUrl: './weather.component.html',
    styleUrls: ['./weather.component.scss']
})
export class WeatherComponent implements OnInit {
    public latestReading: WeatherReading;

    constructor(private weatherService: WeatherService) { }

    ngOnInit() {
        this.weatherService.getLatestReading().subscribe(r => this.latestReading = r);
    }
}
