import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { PowerChartsComponent } from './power-charts.component';

describe('PowerChartsComponent', () => {
    let component: PowerChartsComponent;
    let fixture: ComponentFixture<PowerChartsComponent>;

    beforeEach(waitForAsync(() => {
        TestBed.configureTestingModule({
            declarations: [PowerChartsComponent]
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(PowerChartsComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
