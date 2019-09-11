import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MatToolbarModule, MatIconModule, MatSidenavModule, MatListModule, MatButtonModule } from '@angular/material';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { SocketIoModule, SocketIoConfig } from 'ngx-socket-io';

import { WeatherComponent } from './weather/weather.component';
import { LaundryComponent } from './laundry/laundry.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { WeatherChartsComponent } from './weather-charts/weather-charts.component';
import { WeatherCurrentComponent } from './Weather/weather-current/weather-current.component';

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
        MatToolbarModule,
        MatSidenavModule,
        MatListModule,
        MatButtonModule,
        MatIconModule,
        SocketIoModule.forRoot(config)
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
