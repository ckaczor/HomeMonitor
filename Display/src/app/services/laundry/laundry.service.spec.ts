import { TestBed } from '@angular/core/testing';

import { LaundryService } from './laundry.service';

describe('LaundryService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: LaundryService = TestBed.get(LaundryService);
    expect(service).toBeTruthy();
  });
});
