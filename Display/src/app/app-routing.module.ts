import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WeatherComponent } from './weather/weather.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LaundryComponent } from './laundry/laundry.component';
import { WeatherChartsComponent } from './weather-charts/weather-charts.component';

const routes: Routes = [
    {
        path: '',
        component: DashboardComponent
    },
    {
        path: 'dashboard',
        component: DashboardComponent
    },
    {
        path: 'weather',
        component: WeatherComponent
    },
    {
        path: 'weather/charts',
        component: WeatherChartsComponent
    },
    {
        path: 'laundry',
        component: LaundryComponent
    }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule { }
