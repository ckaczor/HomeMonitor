import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PressureTrendComponent } from './pressure-trend.component';

describe('PressureTrendComponent', () => {
    let component: PressureTrendComponent;
    let fixture: ComponentFixture<PressureTrendComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [PressureTrendComponent]
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(PressureTrendComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
