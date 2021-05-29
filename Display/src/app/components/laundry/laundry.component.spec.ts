import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { LaundryComponent } from './laundry.component';

describe('LaundryComponent', () => {
    let component: LaundryComponent;
    let fixture: ComponentFixture<LaundryComponent>;

    beforeEach(waitForAsync(() => {
        TestBed.configureTestingModule({
            declarations: [LaundryComponent]
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(LaundryComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
