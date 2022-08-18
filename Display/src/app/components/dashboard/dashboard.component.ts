import { Component, OnInit } from '@angular/core';
import { GridsterConfig, GridsterItem, GridsterItemComponentInterface } from 'angular-gridster2';
import { DashboardLayout } from 'src/app/models/dashboard/dashboard-layout';

@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
    public options: GridsterConfig | undefined;
    public dashboardLayout: DashboardLayout | undefined;
    public locked = true;

    private defaultLayout: DashboardLayout = {
        version: 1,
        layout: [
            { cols: 5, rows: 4, y: 0, x: 0 },
            { cols: 3, rows: 2, y: 0, x: 5 },
            { cols: 5, rows: 4, y: 0, x: 8 },
            { cols: 3, rows: 2, y: 2, x: 5 }
        ]
    };

    constructor() { }

    ngOnInit() {
        this.options = {
            itemChangeCallback: () => this.saveOptions(),
            itemResizeCallback: () => this.saveOptions(),
            resizable: {
                enabled: !this.locked
            },
            draggable: {
                enabled: !this.locked
            },
            gridType: 'fixed',
            fixedColWidth: 55,
            fixedRowHeight: 55,
            displayGrid: 'none',
            minCols: 50,
            minRows: 50
        };

        this.loadOptions();
    }

    changedOptions() {
        this.options!.api!.optionsChanged!();
    }

    toggleLocked() {
        this.locked = !this.locked;

        this.options!.resizable!.enabled = !this.locked;
        this.options!.draggable!.enabled = !this.locked;

        this.changedOptions();
    }

    saveOptions() {
        localStorage.setItem('dashboard-layout', JSON.stringify(this.dashboardLayout));
    }

    loadOptions() {
        const savedLayoutString = localStorage.getItem('dashboard-layout');

        if (savedLayoutString == null) {
            this.dashboardLayout = this.defaultLayout;
            return;
        }

        try {
            const savedLayout = JSON.parse(savedLayoutString) as DashboardLayout;

            if (savedLayout.version === this.defaultLayout.version) {
                this.dashboardLayout = savedLayout;
            } else {
                this.dashboardLayout = this.defaultLayout;
            }
        } catch (error) {
            this.dashboardLayout = this.defaultLayout;
        }
    }
}
