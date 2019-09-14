import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ChartModule } from 'angular-highcharts';
import { HttpClientModule } from '@angular/common/http';
import { ClarityModule } from '@clr/angular';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SocketIoModule, SocketIoConfig } from 'ngx-socket-io';

import { WeatherComponent } from './weather/weather.component';
import { LaundryComponent } from './laundry/laundry.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { WeatherChartsComponent } from './weather/charts/weather-charts.component';
import { WeatherCurrentComponent } from './weather/current/weather-current.component';

const config: SocketIoConfig = { url: 'http://home.kaczorzoo.net:9091', options: {} };

@NgModule({
    declarations: [
        AppComponent,
        WeatherComponent,
        LaundryComponent,
        DashboardComponent,
        WeatherChartsComponent,
        WeatherCurrentComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        SocketIoModule.forRoot(config),
        ChartModule,
        HttpClientModule,
        ClarityModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
