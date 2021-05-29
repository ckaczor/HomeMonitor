import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { WeatherChartsComponent } from './weather-charts.component';

describe('WeatherChartsComponent', () => {
    let component: WeatherChartsComponent;
    let fixture: ComponentFixture<WeatherChartsComponent>;

    beforeEach(waitForAsync(() => {
        TestBed.configureTestingModule({
            declarations: [WeatherChartsComponent]
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(WeatherChartsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
