import { Component, OnInit } from '@angular/core';
import { PowerService } from '../../services/power/power.service';
import { PowerStatus } from '../../models/power/power-status';

@Component({
    selector: 'app-power',
    templateUrl: './power.component.html',
    styleUrls: ['./power.component.scss']
})
export class PowerComponent implements OnInit {
    public latestStatus: PowerStatus;
    constructor(private powerService: PowerService) { }

    ngOnInit() {
        this.powerService.getLatestStatus().subscribe(s => this.latestStatus = s);
    }
}
