import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { MatToolbarModule, MatIconModule, MatSidenavModule, MatListModule, MatButtonModule } from '@angular/material';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { WeatherComponent } from './weather/weather.component';
import { LaundryComponent } from './laundry/laundry.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { WeatherChartsComponent } from './weather-charts/weather-charts.component';

@NgModule({
    declarations: [
        AppComponent,
        WeatherComponent,
        LaundryComponent,
        DashboardComponent,
        WeatherChartsComponent
    ],
    imports: [
        BrowserModule,
        AppRoutingModule,
        BrowserAnimationsModule,
        MatToolbarModule,
        MatSidenavModule,
        MatListModule,
        MatButtonModule,
        MatIconModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
