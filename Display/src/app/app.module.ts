import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ChartModule } from 'angular-highcharts';
import { HttpClientModule } from '@angular/common/http';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './components/app/app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatExpansionModule } from '@angular/material/expansion';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { MatSelectModule } from '@angular/material/select';
import { MatToolbarModule } from '@angular/material/toolbar';
import { NavComponent } from './components/nav/nav.component';
import { LayoutModule } from '@angular/cdk/layout';
import { MatSidenavModule } from '@angular/material/sidenav';
import { MatListModule } from '@angular/material/list';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import { MomentModule } from 'ngx-moment';

import { SocketIoModule, SocketIoConfig } from 'ngx-socket-io';
import { GridsterModule } from 'angular-gridster2';

import { LaundryComponent } from './components/laundry/laundry.component';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { WeatherChartsComponent } from './components/weather/charts/weather-charts.component';
import { WeatherCurrentComponent } from './components/weather/current/weather-current.component';
import { AlmanacComponent } from './components/weather/almanac/almanac.component';
import { PowerComponent } from './components/power/current/power.component';
import { PowerChartsComponent } from './components/power/charts/power-charts.component';
import { WeatherSummaryComponent } from './components/weather/summary/weather-summary.component';
import { TimeRangeComponent } from './components/time-range/time-range.component';

const config: SocketIoConfig = { url: '/', options: {} };

@NgModule({
    declarations: [
        AppComponent,
        NavComponent,
        LaundryComponent,
        DashboardComponent,
        WeatherChartsComponent,
        WeatherCurrentComponent,
        AlmanacComponent,
        PowerComponent,
        PowerChartsComponent,
        WeatherSummaryComponent,
        TimeRangeComponent
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
        MatMomentDateModule,
        MatProgressSpinnerModule,
        GridsterModule,
        MomentModule
    ],
    providers: [],
    bootstrap: [AppComponent]
})
export class AppModule { }
