import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ChartModule } from 'angular-highcharts';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './components/app/app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatToolbarModule, MatIconModule, MatMenuModule, MatButtonModule, MatExpansionModule, MatSelectModule, MatDatepickerModule, MatInputModule } from '@angular/material';
import { NavComponent } from './components/nav/nav.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatMomentDateModule } from '@angular/material-moment-adapter';

import { SocketIoModule, SocketIoConfig } from 'ngx-socket-io';

import { LaundryComponent } from './components/laundry/laundry.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { WeatherChartsComponent } from './components/weather/charts/weather-charts.component';
import { WeatherCurrentComponent } from './components/weather/current/weather-current.component';

const config: SocketIoConfig = { url: 'http://home.kaczorzoo.net:9091', options: {} };

@NgModule({
    declarations: [
        AppComponent,
        NavComponent,
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
        MatIconModule,
        MatButtonModule,
        MatToolbarModule,
        MatMenuModule,
        LayoutModule,
        MatSidenavModule,
        MatListModule,
        MatExpansionModule,
        MatSelectModule,
        MatDatepickerModule,
        MatInputModule,
        FormsModule,
        ReactiveFormsModule,
        MatMomentDateModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
