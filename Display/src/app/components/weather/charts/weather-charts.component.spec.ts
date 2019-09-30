import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { WeatherChartsComponent } from './weather-charts.component';

describe('WeatherChartsComponent', () => {
  let component: WeatherChartsComponent;
  let fixture: ComponentFixture<WeatherChartsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ WeatherChartsComponent ]
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
