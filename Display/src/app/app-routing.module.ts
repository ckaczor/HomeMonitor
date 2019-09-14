import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './dashboard/dashboard.component';
import { LaundryComponent } from './laundry/laundry.component';
import { WeatherChartsComponent } from './weather/charts/weather-charts.component';
import { WeatherCurrentComponent } from './weather/current/weather-current.component';

const routes: Routes = [
    {
        path: '',
        component: DashboardComponent
    },
    {
        path: 'weather',
        component: WeatherCurrentComponent
    },
    {
        path: 'weather-charts/:type',
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
