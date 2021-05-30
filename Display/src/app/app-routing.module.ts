import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { WeatherChartsComponent } from './components/weather/charts/weather-charts.component';
import { PowerChartsComponent } from './components/power/charts/power-charts.component';
import { WeatherSummaryComponent } from './components/weather/summary/weather-summary.component';

const routes: Routes = [
    {
        path: '',
        component: DashboardComponent
    },
    {
        path: 'weather-charts',
        component: WeatherChartsComponent
    },
    {
        path: 'power-charts',
        component: PowerChartsComponent
    },
    {
        path: 'weather-summary',
        component: WeatherSummaryComponent
    }
];

@NgModule({
  imports: [RouterModule.forRoot(routes, { relativeLinkResolution: 'legacy' })],
  exports: [RouterModule]
})
export class AppRoutingModule { }
