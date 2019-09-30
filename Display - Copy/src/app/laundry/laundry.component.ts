import { Component, OnInit } from '@angular/core';
import { LaundryService } from './service/laundry.service';
import { LaundryStatus } from './service/laundry-status';

@Component({
    selector: 'app-laundry',
    templateUrl: './laundry.component.html',
    styleUrls: ['./laundry.component.scss']
})
export class LaundryComponent implements OnInit {
    public latestStatus: LaundryStatus;
    constructor(private laundryService: LaundryService) { }

    ngOnInit() {
        this.laundryService.getLatestStatus().subscribe(s => this.latestStatus = s);
    }
}
